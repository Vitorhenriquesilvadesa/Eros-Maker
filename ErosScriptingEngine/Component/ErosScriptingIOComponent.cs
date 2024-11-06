using System;

namespace ErosScriptingEngine.Component
{
    public abstract class ErosScriptingIOComponent<T> : ICloneable, IErosScriptingIOComponent where T : ErosScriptingIOComponent<T>
    {
        public abstract object Clone();
    }
}