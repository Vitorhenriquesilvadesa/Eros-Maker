using System.Collections.Generic;
using Descriptors;
using Entity.Grid;

namespace Descriptor.Grid
{
    public class GridLayerDescriptor : AbstractDescriptor<GridLayer>
    {
        public readonly int width;
        public readonly int height;
        public readonly float cellWidth;
        public readonly float cellHeight;

        public readonly List<GridCellDescriptor> CellDescriptors;

        public GridLayerDescriptor(int width, int height, float cellWidth, float cellHeight, List<GridCellDescriptor> cellDescriptors)
        {
            this.width = width;
            this.height = height;
            this.cellWidth = cellWidth;
            this.cellHeight = cellHeight;
            CellDescriptors = cellDescriptors;
        }

        public override string ToString()
        {
            return $"{{width: {width}, height: {height}, cell_width: {cellWidth}, cell_height: {cellHeight}}}";
        }
    }
}