using ErosScriptingEngine.Executor;
using ErosScriptingEngine.Parse;

namespace ErosScriptingEngine.Component
{
    public class ErosObject
    {
        private readonly ErosScriptExecutor _scriptExecutor = new();
        public string Name { get; private set; }
        public float Id { get; private set; }

        private static float _nextId;

        public ErosObject(string name)
        {
            Name = name;
            Id = _nextId++;
            _scriptExecutor.AttachObject(this);
        }

        public void AttachScript(ErosExecutableScript script)
        {
            _scriptExecutor.AttachScript(script);
        }

        public void Start()
        {
            _scriptExecutor.CallStartEventIfDeclared();
        }

        public void Update()
        {
            _scriptExecutor.CallUpdateEventIfDeclared();
        }

        public void RunScriptScriptBody()
        {
            _scriptExecutor.RunScriptBody();
        }
    }
}