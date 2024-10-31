using UnityEngine;

namespace Service.Application
{
    public class WhoSayHelloService : ApplicationService
    {
        public override void Init()
        {
        }

        public override void Update()
        {
        }

        public override void Dispose()
        {
        }

        public void SayHello()
        {
            Debug.Log("Who Say Hello?");
        }
    }
}