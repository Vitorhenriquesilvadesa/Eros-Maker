using Event;
using EventSystem;
using UnityEngine;

namespace Service.Application
{
    public class PrintHelloService : ApplicationService
    {
        public override void Init()
        {
            Debug.Log("Initializing Hello");
        }

        public override void Update()
        {
            EventAPI.DispatchEvent(new SayHelloEvent());
        }

        public override void Dispose()
        {
            Debug.Log("Disposing Hello");
        }
    }
}