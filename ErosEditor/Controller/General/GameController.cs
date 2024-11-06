using Controller.Grid;
using UnityEngine;

namespace Controller.General
{
    public class GameController : MonoBehaviour
    {
        private PlayerController _playerController;

        [SerializeField] private GridRenderingController gridRenderingController;
        [SerializeField] private MovementController movementController;

        private static GameController _instance;

        private static GameController Instance
        {
            get
            {
                if (_instance is not null) return _instance;

                _instance = FindFirstObjectByType<GameController>();

                if (_instance is not null) return _instance;

                GameObject obj = new GameObject("GameController");
                _instance = obj.AddComponent<GameController>();

                return _instance;
            }
        }

        public static MovementController MovementController => Instance.movementController;
        public static GridRenderingController GridRenderingController => Instance.gridRenderingController;


        public void Awake()
        {
            _playerController = new PlayerController();
        }
    }
}