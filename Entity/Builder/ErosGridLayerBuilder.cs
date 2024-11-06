using System.Collections.Generic;
using Descriptor.Grid;
using Entity.Grid;

namespace Entity.Builder
{
    public class ErosGridLayerBuilder : AbstractEntityBuilder<GridLayer>
    {
        private GridLayerDescriptor _descriptor;

        private int width;
        private int height;
        private float cellWidth;
        private float cellHeight;

        public ErosGridLayerBuilder()
        {
        }

        public ErosGridLayerBuilder SetWidth(int gridWidth)
        {
            width = gridWidth;
            return this;
        }

        public ErosGridLayerBuilder SetHeight(int gridHeight)
        {
            height = gridHeight;
            return this;
        }

        public ErosGridLayerBuilder SetCellWidth(float gridCellWidth)
        {
            cellWidth = gridCellWidth;
            return this;
        }

        public ErosGridLayerBuilder SetCellHeight(int gridCellHeight)
        {
            cellHeight = gridCellHeight;
            return this;
        }

        public override GridLayer Build()
        {
            _descriptor = new GridLayerDescriptor(width, height, cellWidth, cellHeight, new List<GridCellDescriptor>());
            return new GridLayer(_descriptor);
        }
    }
}