using Descriptors;
using Entity;
using ErosScriptingEngine.Executor;
using ErosScriptingEngine.Parse;
using Service.Game;
using UnityEngine;

namespace ErosScriptingEngine.Component
{
    public class ErosObject : AbstractEntity<ErosObject>
    {
        private readonly ErosScriptExecutor _scriptExecutor = new();
        private ErosObjectDescriptor internalProperties;
        private static float _nextId;

        public ErosObject(string name)
        {
            _scriptExecutor.AttachObject(this);
            GameObject gameObject = new GameObject(name);
            MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
            MeshFilter filter = gameObject.AddComponent<MeshFilter>();
            filter.mesh = GameManager.BaseMesh;
            renderer.material = GameManager.BaseMaterial;
            gameObject.transform.position = Vector3.zero;
            internalProperties = new ErosObjectDescriptor(name, _nextId++, gameObject);
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

        public override AbstractDescriptor<ErosObject> GetDescriptor()
        {
            return new ErosObjectDescriptor(internalProperties);
        }

        public override void Destroy()
        {
            _scriptExecutor.CallDestroyEventIfDeclared();
        }
    }
}