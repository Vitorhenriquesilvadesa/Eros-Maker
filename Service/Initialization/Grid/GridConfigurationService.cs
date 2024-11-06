using System.Collections.Generic;
using Descriptor.Grid;
using Entity.Grid;
using NUnit.Framework;
using Service.Application;
using Service.Game;

namespace Service.Initialization.Grid
{
    public class GridConfigurationService : InitializationService
    {
        public void Init()
        {
            List<GridLayerDescriptor> gridLayerDescriptors =
                new()
                {
                    new GridLayerDescriptor(10, 10, 1f, 1f,
                        new List<GridCellDescriptor>())
                };


            GridDescriptor gridDescriptor = new GridDescriptor(gridLayerDescriptors);
            GameManager.FromService<GridManagementService>().SetGlobalGrid(new ErosGrid(gridDescriptor));
        }
    }
}