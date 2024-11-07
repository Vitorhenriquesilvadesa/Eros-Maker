using Controller;
using Entity.Grid;
using Event.Grid;
using EventSystem;
using UnityEngine;

namespace ErosEditor.Controller.Grid
{
    public class GridInteractionController : ApplicationController<GridInteractionController>
    {
        [SerializeField] private LayerMask clickableLayer;
        [SerializeField] private Camera mainCamera;

        private GridCellInfo lastCellSelected;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                DetectGridCellClick();
            }
        }

        private void DetectGridCellClick()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit, Mathf.Infinity, clickableLayer)) return;


            GridCellInfo gridCell = hit.collider.GetComponent<GridCellInfo>();

            if (gridCell is null)
            {
                return;
            }

            if (lastCellSelected == gridCell)
            {
                return;
            }

            if (lastCellSelected is not null)
            {
                lastCellSelected.Unselect();
            }

            lastCellSelected = gridCell;
            gridCell.Select();

            Vector2 cellPosition = gridCell.GetPosition();
            EventAPI.DispatchEvent(new OnGridCellClickedEvent(cellPosition));
        }
    }
}