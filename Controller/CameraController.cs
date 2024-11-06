using Descriptors;
using EventSystem;
using Model;
using UnityEngine;

namespace Controller
{
    public class CameraController : MonoBehaviour
    {
        private CameraConfiguration _cameraConfiguration;
        private CameraDataDescriptor _cameraDataDescriptor;

        [SerializeField] private Transform cameraTransform;
        [SerializeField] private Transform targetTransform;

        private void Start()
        {
            EventAPI.Subscribe(this);
        }
    }
}