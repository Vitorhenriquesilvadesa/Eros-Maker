using Entity.Grid;
using EventSystem.EventSystem;
using UnityEngine;

namespace ErosEditor.Event.Grid
{
    public class OnGridCellClickedEvent : ReactiveEvent
    {
        public readonly Vector2 CellPosition;

        public OnGridCellClickedEvent(Vector2 cellPosition)
        {
            CellPosition = cellPosition;
        }
    }
}