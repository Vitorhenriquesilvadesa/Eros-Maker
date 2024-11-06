using System;

namespace ErosScriptingEngine.Pass
{
    public interface IErosCompilationPass
    {
        object Run(object input);
        object RunWithInterceptors(object input);
        Type GetInputType();
    }
}