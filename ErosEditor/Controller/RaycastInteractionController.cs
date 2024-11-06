using Descriptors;
using Interactive;
using Mapping;
using UnityEngine;

namespace Controller
{
    /// <summary>
    /// Essa classe é responsável pelas interações baseadas na câmera em primeira
    /// pessoa, por exemplo o sistema de dialogo exige que o jogador esteja olhando
    /// para a caixa de diálogo para que a interação aconteça.
    /// </summary>
    public class CameraRayCasterInteractionController : MonoBehaviour
    {
        [SerializeField] [Range(0.5f, 10.0f)] private float rayCastRange;
        [SerializeField] private LayerMask interactiveLayerMask;

        private InteractiveObject lastInteractiveObject;
        private CameraDataDescriptor _cameraDataDescriptor;

        private void Start()
        {
            _cameraDataDescriptor = new CameraDataDescriptor();
        }

        private void Update()
        {
            Ray ray = GetRayFromCamera();
            DrawRay(ray);

            if (!TryHitInteractiveObject(ray, out InteractiveObject interactiveObject)) return;

            interactiveObject.BeforeInteraction();
            interactiveObject.OnInteract();
            interactiveObject.AfterInteraction();
        }

        /// <summary>
        /// Este método tenta ver se algum objeto interativo está cruzando com ray, caso esteja, significa
        /// que uma interação está sendo detectada, neste caso, o objeto interativo é notificado, o mesmo
        /// acontece ao sair de uma interação, o objeto também é notificado.
        /// </summary>
        /// <param name="ray">A reta com a direção onde sse deseja verificar uma intersecção.</param>
        /// <param name="interactiveObject">A saída para caso um objeto seja encontrado.</param>
        /// <returns>True se algum objeto estiver na mira e false caso contrário.</returns>
        private bool TryHitInteractiveObject(Ray ray, out InteractiveObject interactiveObject)
        {
            interactiveObject = null;

            if (!Physics.Raycast(ray, out RaycastHit hit, rayCastRange, interactiveLayerMask))
            {
                UpdateLastInteractiveObject(interactiveObject);
                return false;
            }

            hit.collider.gameObject.TryGetComponent(out interactiveObject);

            UpdateLastInteractiveObject(interactiveObject);

            if (InputController.GetTrigger(KeyboardAction.Interact))
                return true;

            interactiveObject = null;
            return false;
        }

        private void UpdateLastInteractiveObject(InteractiveObject interactiveObject)
        {
            if (interactiveObject is not null)
            {
                if (interactiveObject != lastInteractiveObject)
                {
                    interactiveObject.OnInteractionRayCastEnter();
                }
            }

            if (interactiveObject is null && lastInteractiveObject is not null)
            {
                lastInteractiveObject.OnInteractionRayCastExit();
            }

            lastInteractiveObject = interactiveObject;
        }


        private Ray GetRayFromCamera()
        {
            Quaternion cameraRotation = _cameraDataDescriptor.Transform.rotation;
            Vector3 forwardDirection = cameraRotation * Vector3.forward;
            return new Ray(_cameraDataDescriptor.Transform.position, forwardDirection);
        }

        private void DrawRay(Ray ray)
        {
            Debug.DrawRay(ray.origin, ray.direction * rayCastRange, Color.red);
        }
    }
}