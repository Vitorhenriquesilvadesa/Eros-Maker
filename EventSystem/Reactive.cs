namespace EventSystem
{
    using System;

    /// <summary>
    /// O atributo <see cref="Reactive"/> é utilizado para marcar métodos que devem ser registrados como ouvintes (listeners) de eventos reativos.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class Reactive : Attribute
    {
    }
}