using System.Collections.Generic;
using Controller.Grid;
using Descriptor.Grid;
using Entity.Grid;
using Event.Grid;
using EventSystem;
using Service.Game;

namespace Service.Application
{
    public class GridManagementService : ApplicationService
    {
        private ErosGrid _globalGrid;
        private GridRenderingController _renderingController;
        private GridMovementController _movementController;
        private ErosCursorWarper _cursorWarper;

        public GridLayerDescriptor GetGridLayerDescriptor()
        {
            return _renderingController.activeLayer;
        }

        public override void Init()
        {
            _renderingController = GameManager.GridRenderingController;
            _movementController = GameManager.GridMovementController;
            _cursorWarper = new ErosCursorWarper();

            GridLayerDescriptor descriptor = new GridLayerDescriptor(20, 20, 1f, 1f, new List<GridCellDescriptor>());
            EventAPI.DispatchEvent(new ChangeGridActiveLayerEvent(descriptor));
        }

        public override void Update()
        {
            _cursorWarper.Update();
        }

        public override void Dispose()
        {
        }

        public void SetGlobalGrid(ErosGrid grid)
        {
            _globalGrid = grid;
        }
    }
}