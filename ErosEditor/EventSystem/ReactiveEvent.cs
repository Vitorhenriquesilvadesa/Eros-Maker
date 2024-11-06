namespace EventSystem
{
    namespace EventSystem
    {
        /// <summary>
        /// A classe <see cref="ReactiveEvent"/> é uma classe base para eventos reativos no sistema de eventos.
        /// </summary>
        public abstract class ReactiveEvent
        {
            /// <summary>
            /// Indica se o evento foi tratado ou não.
            /// </summary>
            /// <remarks>
            /// Quando um evento é tratado por um ou mais ouvintes, esta propriedade é marcada como <c>true</c>.
            /// Isso pode ser utilizado para evitar o processamento de eventos múltiplas vezes ou para verificar se o evento já foi manipulado.
            /// </remarks>
            public bool Handled { get; set; }
        }
    }
}