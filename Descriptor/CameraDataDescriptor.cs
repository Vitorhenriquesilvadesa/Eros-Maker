using UnityEngine;

namespace Descriptors
{
    public class CameraDataDescriptor : AbstractDescriptor<Camera>
    {
        private readonly Camera _camera = Camera.main;
        public Transform Transform => _camera.transform;

        public float FOV
        {
            get => _camera.fieldOfView;
            set => _camera.fieldOfView = value;
        }
    }
}