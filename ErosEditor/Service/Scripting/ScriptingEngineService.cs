using System.Collections.Generic;
using System.Threading.Tasks;
using ErosEditor.Event.Scripting;
using ErosEditor.EventSystem;
using ErosScriptingEngine.Component;
using ErosScriptingEngine.Engine;
using ErosScriptingEngine.Parse;
using EventSystem;
using Service.Application;
using UnityEngine;

namespace ErosEditor.Service.Scripting
{
    public class ScriptingEngineService : ApplicationService
    {
        private ErosScriptingManager _erosScriptingManager;
        private Dictionary<int, List<ErosObject>> _erosObjectChunks;
        private const int ChunkSize = 100;
        private int objectCount;
        private ErosObject instance;

        public override void Init()
        {
            EventAPI.Subscribe(this);
            _erosScriptingManager = new ErosScriptingManager();
            _erosObjectChunks = new Dictionary<int, List<ErosObject>>();

            instance = Instantiate("player");
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

        public override void Update()
        {
            UpdateObjects();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                EventAPI.DispatchEvent(new ScriptRecompilationRequestEvent());
            }
        }

        private void UpdateObjects()
        {
            foreach (List<ErosObject> erosObjects in _erosObjectChunks.Values)
            {
                foreach (ErosObject erosObject in erosObjects)
                {
                    erosObject.Update();
                }
            }
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

        [Reactive]
        public void OnScriptRecompileRequest(ScriptRecompilationRequestEvent e)
        {
            _erosScriptingManager.RecompileScripts();
            instance.AttachScript(GetScriptByName("test.eros"));
        }

        public override void Dispose()
        {
            // Dispose logic, if needed
        }
    }
}