using System;
using Descriptors;
using Event;
using EventSystem;
using Model;
using Service.Application;
using Service.Game;
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

        [Reactive]
        public void SayHelloViaEvent(SayHelloEvent e)
        {
            GameManager.FromService<WhoSayHelloService>().SayHello();
        }
    }
}