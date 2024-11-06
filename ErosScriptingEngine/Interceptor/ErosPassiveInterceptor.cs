namespace ErosScriptingEngine.Interceptor
{
    public interface ErosPassiveInterceptor<I, O>
    {
        void BeforeState(I input);

        void AfterState(O input);

        string GetName();
    }
}