using UnityEngine;
using Random = UnityEngine.Random;

namespace Entity
{
    public class FireflyMovement : MonoBehaviour
    {
        [SerializeField] private Vector3 minBound;
        [SerializeField] private Vector3 maxBound;
        [SerializeField] private float movementSpeed;
        [SerializeField] private float rotationSpeed;

        [SerializeField] private Vector3 initialPosition;
        [SerializeField] private Vector3 target;
        [SerializeField] private Vector3 direction;
        [SerializeField] private Vector3 targetDirection;
        [SerializeField] private float timer;

        private void Start()
        {
            initialPosition = transform.position;
            timer = Random.value * 2f;
        }

        private void Update()
        {
            UpdateTimer();
            UpdatePosition();
            UpdateDirection();
        }

        private void UpdateDirection()
        {
            direction = Vector3.Lerp(direction, targetDirection, Time.deltaTime * rotationSpeed);
        }

        private void UpdateTimer()
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                timer = Random.value * 2;
                CalculateNextRandomPosition();
                targetDirection = CalculateTargetDirection();
            }
        }

        private void CalculateNextRandomPosition()
        {
            Vector3 delta = maxBound - minBound + initialPosition;

            target = new Vector3(
                Random.value * delta.x + minBound.x,
                Random.value * delta.y + minBound.y,
                Random.value * delta.z + minBound.z
            );
        }

        private void UpdatePosition()
        {
            transform.position += direction * (movementSpeed * Time.deltaTime);
        }

        private Vector3 CalculateTargetDirection()
        {
            return Vector3.Normalize(target - transform.position);
        }
    }
}