using System.Collections.Generic;
using System.Threading.Tasks;
using ErosScriptingEngine.Component;
using ErosScriptingEngine.Engine;
using ErosScriptingEngine.Parse;
using Service.Application;

namespace ErosEditor.Service.Scripting
{
    public class ScriptingEngineService : ApplicationService
    {
        private ErosScriptingManager _erosScriptingManager;
        private Dictionary<int, List<ErosObject>> _erosObjectChunks;
        private const int ChunkSize = 100;
        private int objectCount;

        public override void Init()
        {
            _erosScriptingManager = new ErosScriptingManager();
            _erosObjectChunks = new Dictionary<int, List<ErosObject>>();

            ErosObject instance = Instantiate("player");
            instance.AttachScript(GetScriptByName("test.eros"));
            instance.RunScriptScriptBody();
            instance.Start();
        }

        public ErosObject Instantiate(string name)
        {
            objectCount++;
            ErosObject erosObject = new ErosObject(name);
            int chunkId = objectCount / ChunkSize;

            if (!_erosObjectChunks.ContainsKey(chunkId))
                _erosObjectChunks[chunkId] = new List<ErosObject>();

            _erosObjectChunks[chunkId].Add(erosObject);

            return erosObject;
        }

        public ErosExecutableScript GetScriptByName(string name)
        {
            return _erosScriptingManager.GetScriptByName(name);
        }

        public override async void Update()
        {
            await UpdateObjectsAsync();
        }

        private async Task UpdateObjectsAsync()
        {
            List<Task> chunkTasks = new List<Task>();

            foreach (var chunk in _erosObjectChunks.Values)
            {
                chunkTasks.Add(Task.Run(() =>
                {
                    foreach (var obj in chunk)
                    {
                        obj.Update();
                    }
                }));
            }

            await Task.WhenAll(chunkTasks);
        }

        public override void Dispose()
        {
            // Dispose logic, if needed
        }
    }
}