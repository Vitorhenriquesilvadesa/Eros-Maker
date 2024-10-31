using System.Collections.Generic;
using EventSystem.EventSystem;

namespace EventSystem
{
    /// <summary>
    /// Representa uma fila de eventos para armazenar e gerenciar eventos de forma sequencial.
    /// </summary>
    public class EventQueue
    {
        private readonly Queue<ReactiveEvent> _events = new();

        /// <summary>
        /// Adiciona um evento à fila.
        /// </summary>
        /// <param name="reactiveEvent">O evento a ser adicionado à fila.</param>
        public void PushEvent(ReactiveEvent @event)
        {
            _events.Enqueue(@event);
        }

        /// <summary>
        /// Remove todos os eventos da fila.
        /// </summary>
        public void ClearEvents()
        {
            _events.Clear();
        }

        /// <summary>
        /// Verifica se a fila de eventos está vazia.
        /// </summary>
        public bool IsEmpty => _events.Count == 0;

        /// <summary>
        /// Remove e retorna o próximo evento da fila.
        /// </summary>
        /// <returns>O próximo evento na fila.</returns>
        /// <exception cref="EmptyEventQueueException">Lançado se a fila estiver vazia.</exception>
        public ReactiveEvent PopEvent()
        {
            if (!IsEmpty)
            {
                return _events.Dequeue();
            }

            throw new EmptyEventQueueException();
        }
    }
}