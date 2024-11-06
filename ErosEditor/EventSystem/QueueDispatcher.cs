using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EventSystem.EventSystem;
using UnityEngine;

namespace EventSystem
{
    /// <summary>
    /// Exceção lançada quando a fila de eventos está vazia.
    /// </summary>
    public class EmptyEventQueueException : Exception
    {
        public EmptyEventQueueException() : base("The event queue is empty.")
        {
        }
    }

    public class QueueDispatcher
    {
        private readonly Dictionary<Type, Dictionary<MethodInfo, List<object>>> classMapMap;
        private readonly Dictionary<Type, EventQueue> eventQueueMap;
        private readonly Dictionary<Type, List<MethodInfo>> reactiveMethodCache;

        public QueueDispatcher()
        {
            this.classMapMap = new Dictionary<Type, Dictionary<MethodInfo, List<object>>>();
            this.eventQueueMap = new Dictionary<Type, EventQueue>();
            this.reactiveMethodCache = new Dictionary<Type, List<MethodInfo>>();
        }

        public void Subscribe(object listener)
        {
            var listenerType = listener.GetType();
            if (!reactiveMethodCache.ContainsKey(listenerType))
            {
                // Cache the reactive methods for this listener type
                var reactiveMethods = GetReactiveMethods(listener);
                reactiveMethodCache[listenerType] = reactiveMethods;
            }

            var methods = reactiveMethodCache[listenerType];

            foreach (var method in methods)
            {
                var parameters = method.GetParameters();

                if (parameters.Length == 1 && typeof(ReactiveEvent).IsAssignableFrom(parameters[0].ParameterType))
                {
                    var eventType = parameters[0].ParameterType;

                    if (!classMapMap.ContainsKey(eventType))
                    {
                        classMapMap[eventType] = new Dictionary<MethodInfo, List<object>>();
                    }

                    if (!classMapMap[eventType].ContainsKey(method))
                    {
                        classMapMap[eventType][method] = new List<object>();
                    }

                    classMapMap[eventType][method].Add(listener);
                }
            }
        }

        public void Unsubscribe(object listener)
        {
            var listenerType = listener.GetType();
            if (!reactiveMethodCache.ContainsKey(listenerType))
            {
                // If no cache exists, no need to proceed
                return;
            }

            var methods = reactiveMethodCache[listenerType];

            foreach (var method in methods)
            {
                RemoveListener(method, listener);
            }
        }

        public void DispatchEvent(ReactiveEvent @event)
        {
            var eventType = @event.GetType();

            if (!eventQueueMap.ContainsKey(eventType))
            {
                eventQueueMap[eventType] = new EventQueue();
            }

            eventQueueMap[eventType].PushEvent(@event);
        }

        public void DispatchQueues()
        {
            foreach (var eventType in classMapMap.Keys.ToList())
            {
                if (eventQueueMap.TryGetValue(eventType, out var eventQueue) && !eventQueue.IsEmpty)
                {
                    NotifyListeners(eventQueue, eventType);
                }
            }
        }

        private void RemoveListener(MethodInfo method, object instance)
        {
            if (method.GetCustomAttribute<Reactive>() != null)
            {
                var parameters = method.GetParameters();

                if (parameters.Length == 1 && typeof(ReactiveEvent).IsAssignableFrom(parameters[0].ParameterType))
                {
                    var eventType = parameters[0].ParameterType;

                    if (classMapMap.ContainsKey(eventType))
                    {
                        var methodList = classMapMap[eventType];

                        if (methodList.ContainsKey(method))
                        {
                            methodList[method].Remove(instance);

                            if (methodList[method].Count == 0)
                            {
                                methodList.Remove(method);
                            }
                        }
                    }
                }
            }
        }

        private List<MethodInfo> GetReactiveMethods(object listener)
        {
            var methods = listener.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var result = new List<MethodInfo>();

            foreach (var method in methods)
            {
                if (method.GetCustomAttribute<Reactive>() != null)
                {
                    var parameters = method.GetParameters();

                    if (parameters.Length == 1 &&
                        typeof(ReactiveEvent).IsAssignableFrom(parameters[0].ParameterType))
                    {
                        result.Add(method);
                    }
                }
            }

            return result;
        }

        private void NotifyListeners(EventQueue queue, Type eventType)
        {
            while (!queue.IsEmpty)
            {
                var ev = queue.PopEvent();

                foreach (var kvp in classMapMap[eventType])
                {
                    var method = kvp.Key;
                    var listeners = kvp.Value;

                    if (listeners.Count == 0 || ev.Handled)
                    {
                        continue;
                    }

                    foreach (var listener in listeners)
                    {
                        InvokeMethod(method, listener, ev);
                    }
                }
            }
        }

        private void InvokeMethod(MethodInfo method, object instance, ReactiveEvent param)
        {
            try
            {
                if (!method.IsPublic)
                {
                    throw new InvalidOperationException(
                        $"Method '{method.Name}' in class '{instance.GetType().Name}' must be PUBLIC.");
                }

                method.Invoke(instance, new object[] { param });
            }
            catch (TargetInvocationException e)
            {
                throw new InvalidOperationException($"Cannot invoke method '{method.Name}'", e.InnerException);
            }
        }
    }
}