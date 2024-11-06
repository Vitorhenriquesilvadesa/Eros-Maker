using UnityEngine;

namespace Entity.Eros
{
    public abstract class ErosObject
    {
        protected GameObject physicalObject;

        public abstract void Destroy();
        public abstract void Init();
    }
}