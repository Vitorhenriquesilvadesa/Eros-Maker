using System;

namespace Model
{
    public class Player : AbstractModel
    {
        private float _movementSpeed;
        private float _runSpeedMultiplier;
        private float _walkSpeedMultiplier;

        public float MovementSpeed
        {
            get => _movementSpeed;
            set
            {
                if (value < 0) throw new ArgumentException("Cannot set velocity less than zero.");
                _movementSpeed = value;
            }
        }

        public float RunSpeedMultiplier
        {
            get => _runSpeedMultiplier;
            set
            {
                if (value < 0) throw new ArgumentException("Cannot set velocity less than zero.");
                _runSpeedMultiplier = value;
            }
        }

        public float WalkSpeedMultiplier
        {
            get => _walkSpeedMultiplier;
            set
            {
                if (value < 0) throw new ArgumentException("Cannot set velocity less than zero.");
                _walkSpeedMultiplier = value;
            }
        }
    }
}