using System;
using Descriptor.Grid;
using ErosEditor.Event.Grid;
using ErosEditor.EventSystem;
using ErosEditor.Service.Application;
using Event.Grid;
using EventSystem;
using Service.Application;
using Service.Game;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Controller.Grid
{
    public class GridMovementController : ApplicationController<GridMovementController>
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float horizontalSpeed;
        [SerializeField] private float cameraMovementSpeed;
        [SerializeField] private float verticalSpeed;
        [SerializeField] private float scrollSpeed;

        private Vector2 cameraCenter;
        private float cameraDistance = 10f;
        private float cameraAngle;
        private float cameraHeight = 3f;
        private float cameraMaxHeight = 10f;
        private float cameraMinHeight = 0.1f;

        private GridLayerDescriptor _gridLayerDescriptor;

        private bool isAllDataLoaded;

        private Vector3 targetCenter;
        private Vector3 currentCenter;
        private Vector3 gridCellSize = Vector3.zero;
        private Vector3 targetPosition;
        private Vector3 currentTargetCenter = Vector3.zero;

        private void Update()
        {
            if (isAllDataLoaded)
            {
                UpdateMetrics();
                UpdateTargetCenter();
                UpdateCameraPosition();
                UpdateCameraRotation();
            }
            else
            {
                _gridLayerDescriptor = GameManager.FromService<GridManagementService>().GetGridLayerDescriptor();

                if (_gridLayerDescriptor is not null)
                {
                    isAllDataLoaded = true;
                }
            }
        }

        private void UpdateMetrics()
        {
            if (Input.GetMouseButton(1))
            {
                if (Input.GetKey(KeyCode.LeftAlt))
                {
                    cameraHeight += Input.GetAxis("Mouse Y") * verticalSpeed * Time.deltaTime;
                    cameraHeight = Mathf.Clamp(cameraHeight, cameraMinHeight, cameraMaxHeight);
                }
                else
                {
                    cameraAngle -= Input.GetAxis("Mouse X") * horizontalSpeed * Time.deltaTime;
                }
            }

            cameraDistance -= Input.mouseScrollDelta.y * scrollSpeed * Time.deltaTime;
            cameraDistance = Mathf.Clamp(cameraDistance, 0.1f, 100f);
        }

        private void UpdateTargetCenter()
        {
            targetCenter = new Vector3(cameraCenter.x, cameraHeight, cameraCenter.y);
            gridCellSize = new Vector3(_gridLayerDescriptor.cellWidth, 1f, _gridLayerDescriptor.cellHeight);

            targetCenter = Vector3.Scale(targetCenter, gridCellSize) + gridCellSize / 2f;
            currentTargetCenter = Vector3.Lerp(currentTargetCenter, targetCenter,
                0.006f * cameraMovementSpeed * Time.deltaTime);
        }

        private void UpdateCameraRotation()
        {
            cameraAngle %= 360f;
            _camera.transform.LookAt(currentCenter - new Vector3(0f, 1f, 0f));
        }

        private void UpdateCameraPosition()
        {
            targetPosition = new Vector3(Mathf.Cos(cameraAngle * Mathf.Deg2Rad) * cameraDistance, cameraHeight,
                Mathf.Sin(cameraAngle * Mathf.Deg2Rad) * cameraDistance);

            currentCenter = Vector3.Lerp(currentCenter, targetCenter, 0.003f + Time.deltaTime * cameraMovementSpeed);

            _camera.transform.position = targetPosition + currentCenter;
        }


        [Reactive]
        public void OnGridActiveLayerChange(ChangeGridActiveLayerEvent e)
        {
            _gridLayerDescriptor = e.Descriptor;
            gridCellSize = new Vector3(_gridLayerDescriptor.cellWidth, 0, _gridLayerDescriptor.cellHeight);
        }

        [Reactive]
        public void OnGridCellClicked(OnGridCellClickedEvent e)
        {
            cameraCenter = e.CellPosition;
        }
    }
}