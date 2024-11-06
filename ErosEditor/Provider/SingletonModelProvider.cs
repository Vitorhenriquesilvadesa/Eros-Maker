using System;
using System.Collections.Generic;
using Model;
using UnityEngine;

namespace Provider
{
    public sealed class SingletonModelProvider
    {
        private static readonly SingletonModelFactory SingletonModelFactory = new();
        private static readonly Dictionary<Type, AbstractModel> RegisteredModels = new();

        public static T Get<T>() where T : AbstractModel
        {
            if (!RegisteredModels.ContainsKey(typeof(T)))
            {
                RegisteredModels.Add(typeof(T), SingletonModelFactory.Create<T>());
            }

            return RegisteredModels[typeof(T)] as T;
        }

        public static void Set<T>(T instance) where T : AbstractModel
        {
            if (!RegisteredModels.ContainsKey(typeof(T)))
            {
                RegisteredModels.Add(typeof(T), instance);
            }
            else
            {
                RegisteredModels[typeof(T)] = instance;
            }
        }
    }
}