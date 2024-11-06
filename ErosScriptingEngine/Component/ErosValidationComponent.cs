namespace ErosScriptingEngine.Component
{
    public interface ErosValidationComponent<in T>
    {
        public void Validate(T input);
    }
}