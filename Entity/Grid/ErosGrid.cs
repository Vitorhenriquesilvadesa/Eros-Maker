using System.Collections.Generic;
using System.Linq;
using Descriptor.Grid;
using Descriptors;

namespace Entity.Grid
{
    public class ErosGrid : AbstractEntity<ErosGrid>
    {
        private GridDescriptor _descriptor;
        private List<GridLayer> _layers;

        public ErosGrid(GridDescriptor descriptor)
        {
            _descriptor = descriptor;
        }

        public override AbstractDescriptor<ErosGrid> GetDescriptor()
        {
            List<GridLayerDescriptor> descriptors =
                _layers.Select(layer => layer.GetDescriptor() as GridLayerDescriptor).ToList();

            return new GridDescriptor(descriptors);
        }

        public void AddNewLayer(GridLayerDescriptor descriptor)
        {
        }

        public override void Destroy()
        {
            foreach (GridLayer layer in _layers)
            {
                layer.Destroy();
            }
        }
    }
}