using Descriptors;
using UnityEngine;

namespace ErosScriptingEngine.Component
{
    public class ErosObjectDescriptor : AbstractDescriptor<ErosObject>
    {
        public readonly string Name;
        public readonly float Id;
        public readonly GameObject PhysicalObject;

        public ErosObjectDescriptor(string name, float id, GameObject physicalObject)
        {
            Name = name;
            Id = id;
            PhysicalObject = physicalObject;
        }

        public ErosObjectDescriptor(ErosObjectDescriptor other)
        {
            PhysicalObject = other.PhysicalObject;
            Name = other.Name;
            Id = other.Id;
        }
    }
}