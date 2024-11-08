using Controller;
using ErosEditor.Event.Grid;
using ErosEditor.EventSystem;

namespace ErosEditor.Controller.Grid
{
    public class GridObjectController : ApplicationController<GridObjectController>
    {
        [Reactive]
        public void OnGridCellClicked(OnGridCellClickedEvent e)
        {
            
        }
    }
}