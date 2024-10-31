using System;
using System.Collections.Generic;
using EventSystem;
using EventSystem.EventSystem;
using UnityEngine;

namespace Interactive
{
    /// <summary>
    /// Classe abstrata que define a base para objetos interativos no jogo.
    /// </summary>
    public abstract class InteractiveObject : MonoBehaviour
    {
        /// <summary>
        /// Método a ser chamado antes da interação com o objeto.
        /// Deve ser implementado pelas subclasses para definir ações a serem realizadas antes da interação.
        /// </summary>
        public abstract void BeforeInteraction();

        /// <summary>
        /// Método a ser chamado quando o objeto for interagido.
        /// Deve ser implementado pelas subclasses para definir as ações a serem realizadas durante a interação.
        /// </summary>
        public abstract void OnInteract();

        /// <summary>
        /// Método a ser chamado após a interação com o objeto.
        /// Deve ser implementado pelas subclasses para definir as ações a serem realizadas após a interação.
        /// </summary>
        public abstract void AfterInteraction();

        /// <summary>
        /// Método a ser chamado quando o raio de interação entra em contato com o objeto.
        /// Deve ser implementado pelas subclasses para definir ações a serem realizadas quando o raio de interação entra em contato.
        /// </summary>
        public abstract void OnInteractionRayCastEnter();

        /// <summary>
        /// Método a ser chamado quando o raio de interação sai de contato com o objeto.
        /// Deve ser implementado pelas subclasses para definir ações a serem realizadas quando o raio de interação sai de contato.
        /// </summary>
        public abstract void OnInteractionRayCastExit();

        /// <summary>
        /// Emite um evento reativo para o sistema de eventos.
        /// </summary>
        /// <param name="event">Evento a ser emitido.</param>
        public void OnEmit(ReactiveEvent @event)
        {
            EventAPI.DispatchEvent(@event);
        }
    }
}