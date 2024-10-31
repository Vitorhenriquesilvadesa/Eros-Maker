using Model;
using Provider;

namespace Controller
{
    public class PlayerController
    {
        private readonly Player _player;

        public PlayerController()
        {
            _player = new Player
            {
                MovementSpeed = 3f,
                WalkSpeedMultiplier = 1f,
                RunSpeedMultiplier = 1.5f
            };
            
            SingletonModelProvider.Set(_player);
        }
    }
}