using System;
using System.Collections.Generic;
using Controller.Grid;
using ErosEditor.Service.Application;
using ErosEditor.Service.Scripting;
using Service.Application;
using Service.Application.Exception;
using Service.Initialization;
using Service.Initialization.Grid;
using Service.Provider;
using UnityEngine;

namespace Service.Game
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;

        [Header("Required Controllers for Initialization")] [SerializeField]
        private GridRenderingController gridRenderingController;

        [SerializeField] private GridMovementController gridMovementController;
        [SerializeField] private Mesh erosBaseMesh;
        [SerializeField] private Material erosBaseMaterial;

        private static GameManager Instance
        {
            get
            {
                if (_instance is not null) return _instance;

                _instance = FindFirstObjectByType<GameManager>();

                if (_instance is not null) return _instance;

                GameObject obj = new GameObject("GameManager");
                _instance = obj.AddComponent<GameManager>();

                return _instance;
            }
        }

        public static GridRenderingController GridRenderingController => Instance.gridRenderingController;
        public static GridMovementController GridMovementController => Instance.gridMovementController;
        public static Mesh BaseMesh => Instance.erosBaseMesh;
        public static Material BaseMaterial => Instance.erosBaseMaterial;

        private ServiceProvider<InitializationService> initializationServiceProvider;
        private ServiceProvider<ApplicationService> applicationServiceProvider;

        private List<InitializationService> initializationServices;
        private Dictionary<Type, ApplicationService> applicationServices;

        private void Awake()
        {
            initializationServiceProvider = new InitializationServiceProvider();
            applicationServiceProvider = new ApplicationServiceProvider();
            initializationServices = new List<InitializationService>();
            applicationServices = new Dictionary<Type, ApplicationService>();

            RegisterInitializationServices();
            RegisterApplicationServices();
            RunInitializationServices();
        }

        private void Start()
        {
            InitApplicationServices();
        }

        private void Update()
        {
            UpdateApplicationServices();
        }

        public static T FromService<T>() where T : ApplicationService
        {
            Dictionary<Type, ApplicationService> services = Instance.applicationServices;

            if (services.ContainsKey(typeof(T)))
            {
                return services[typeof(T)] as T;
            }

            throw new UnknownServiceException();
        }

        private void UpdateApplicationServices()
        {
            foreach (ApplicationService service in Instance.applicationServices.Values)
            {
                service.Update();
            }
        }

        private void InitApplicationServices()
        {
            foreach (ApplicationService service in Instance.applicationServices.Values)
            {
                service.Init();
            }
        }

        private void RunInitializationServices()
        {
            foreach (InitializationService service in Instance.initializationServices)
            {
                service.Init();
            }
        }

        private void RegisterInitializationServices()
        {
            RegisterInitializationService<GridConfigurationService>();
        }

        private void RegisterApplicationServices()
        {
            RegisterApplicationService<GridManagementService>();
            RegisterApplicationService<ScriptingEngineService>();
        }

        private void RegisterInitializationService<T>() where T : InitializationService
        {
            InitializationService service = initializationServiceProvider.Get<T>();
            Debug.Log($"Adding Initialization Service: {service.GetType().Name}");
            initializationServices.Add(service);
        }

        private void RegisterApplicationService<T>() where T : ApplicationService
        {
            ApplicationService service = applicationServiceProvider.Get<T>();
            Debug.Log($"Adding Application Service: {service.GetType().Name}");
            applicationServices[service.GetType()] = service;
        }

        public void OnDestroy()
        {
            foreach (ApplicationService service in applicationServices.Values)
            {
                service.Dispose();
            }
        }
    }
}