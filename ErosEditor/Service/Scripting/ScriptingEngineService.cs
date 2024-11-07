using ErosScriptingEngine.Component;
using ErosScriptingEngine.Engine;
using ErosScriptingEngine.Parse;
using Service.Application;

namespace ErosEditor.Service.Scripting
{
    public class ScriptingEngineService : ApplicationService
    {
        private ErosScriptingManager _erosScriptingManager;
        private ErosObject testObject;

        public override void Init()
        {
            _erosScriptingManager = new ErosScriptingManager();

            testObject = new ErosObject("Test Object");
            testObject.AttachScript(GetScriptByName("test.eros"));

            testObject.Start();
            testObject.RunScriptScriptBody();
        }

        public ErosExecutableScript GetScriptByName(string name)
        {
            return _erosScriptingManager.GetScriptByName(name);
        }

        public override void Update()
        {
            testObject.Update();
        }

        public override void Dispose()
        {
        }
    }
}