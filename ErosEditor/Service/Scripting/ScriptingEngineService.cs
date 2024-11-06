using ErosScriptingEngine.Engine;
using Service.Application;

namespace ErosEditor.Service.Scripting
{
    public class ScriptingEngineService : ApplicationService
    {
        private ErosScriptingManager _erosScriptingManager;

        public override void Init()
        {
            _erosScriptingManager = new ErosScriptingManager();
        }

        public override void Update()
        {
            
        }

        public override void Dispose()
        {
        }
    }
}