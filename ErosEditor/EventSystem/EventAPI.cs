using System.Threading.Tasks;
using ErosEditor.EventSystem;
using EventSystem.EventSystem;
using UnityEngine;

namespace EventSystem
{
    /// <summary>
    /// A classe <see cref="EventAPI"/> fornece uma interface estática para o sistema de eventos,
    /// permitindo o despacho e a inscrição de eventos de maneira centralizada.
    /// </summary>
    public class EventAPI : MonoBehaviour
    {
        /// <summary>
        /// O despachante de eventos responsável por gerenciar e processar filas de eventos.
        /// </summary>
        private static readonly QueueDispatcher QueueDispatcher = new();

        /// <summary>
        /// A tarefa que gerencia o despacho assíncrono de eventos.
        /// </summary>
        private static Task _dispatchTask;

        /// <summary>
        /// Objeto de bloqueio usado para sincronização de acesso ao despacho de eventos.
        /// </summary>
        private static readonly object LockObject = new();

        /// <summary>
        /// Despacha um evento para o sistema de eventos.
        /// </summary>
        /// <param name="e">O evento a ser despachado.</param>
        /// <remarks>
        /// O evento é adicionado à fila de eventos e será processado de forma assíncrona.
        /// </remarks>
        public static void DispatchEvent(ReactiveEvent e)
        {
            QueueDispatcher.DispatchEvent(e);
        }

        /// <summary>
        /// Inscreve um objeto para receber eventos.
        /// </summary>
        /// <param name="obj">O objeto que será inscrito para receber eventos.</param>
        /// <remarks>
        /// O objeto deve ter métodos anotados com o atributo <see cref="Reactive"/> para receber eventos.
        /// </remarks>
        public static void Subscribe(object obj)
        {
            QueueDispatcher.Subscribe(obj);
        }

        /// <summary>
        /// Cancela a inscrição de um objeto para receber eventos.
        /// </summary>
        /// <param name="obj">O objeto que será removido da lista de ouvintes.</param>
        public static void Unsubscribe(object obj)
        {
            QueueDispatcher.Unsubscribe(obj);
        }

        /// <summary>
        /// Atualiza o estado do despachante de eventos e processa a fila de eventos.
        /// </summary>
        /// <remarks>
        /// Este método é chamado a cada quadro e garante que a fila de eventos seja processada
        /// de forma assíncrona. A sincronização é realizada usando um objeto de bloqueio para garantir
        /// que apenas uma tarefa de despacho esteja em execução por vez.
        /// </remarks>
        private void Update()
        {
            QueueDispatcher.DispatchQueues();
        }
    }
}