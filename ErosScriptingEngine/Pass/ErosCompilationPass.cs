using System;
using System.Collections.Generic;
using ErosScriptingEngine.Component;
using ErosScriptingEngine.Interceptor;

namespace ErosScriptingEngine.Pass
{
    public abstract class ErosCompilationPass<I, O> : IErosCompilationPass
        where I : ErosScriptingIOComponent<I>
        where O : ErosScriptingIOComponent<O>, ICloneable
    {
        private readonly List<ErosPassiveInterceptor<I, O>> interceptors = new List<ErosPassiveInterceptor<I, O>>();

        public ErosCompilationPass<I, O> AddInterceptor(ErosPassiveInterceptor<I, O> interceptor)
        {
            interceptors.Add(interceptor);
            return this;
        }

        object IErosCompilationPass.Run(object input)
        {
            if (input is I typedInput)
            {
                return Run(typedInput);
            }
            throw new ArgumentException($"Input type mismatch. Expected: {typeof(I)}, but got: {input.GetType()}");
        }

        object IErosCompilationPass.RunWithInterceptors(object input)
        {
            if (input is I typedInput)
            {
                return RunWithInterceptors(typedInput);
            }
            throw new ArgumentException($"Input type mismatch. Expected: {typeof(I)}, but got: {input.GetType()}");
        }

        public O Run(I input)
        {
            return Pass(input);
        }

        public O RunWithInterceptors(I input)
        {
            foreach (var interceptor in interceptors)
            {
                interceptor.BeforeState(input);
            }

            O output = Pass(input);

            foreach (var interceptor in interceptors)
            {
                interceptor.AfterState(output);
            }

            return output;
        }

        public abstract Type GetInputType();
        public abstract Type GetOutputType();

        public abstract string GetDebugName();

        protected abstract O Pass(I input);
    }
}
