using System.Collections.Generic;
using Descriptors;
using Entity.Grid;

namespace Descriptor.Grid
{
    public class GridDescriptor : AbstractDescriptor<ErosGrid>
    {
        public readonly List<GridLayerDescriptor> Descriptors;

        public GridDescriptor(List<GridLayerDescriptor> descriptors)
        {
            Descriptors = descriptors;
        }
    }
}