using System.Collections.Generic;
using System.Linq;
using Descriptor;
using Descriptor.Grid;
using Descriptors;
using Entity.Eros;
using UnityEngine;

namespace Entity.Grid
{
    public class GridLayer : AbstractEntity<GridLayer>
    {
        private readonly GridLayerDescriptor _descriptor;

        private List<List<GridCell>> objects;

        public GridLayer(GridLayerDescriptor descriptor)
        {
            _descriptor = descriptor;
        }

        public override AbstractDescriptor<GridLayer> GetDescriptor()
        {
            return _descriptor;
        }

        public bool SetPositionTo(Vector2Int position, ErosObject erosObject)
        {
            if (!IsPositionEmpty(position)) return false;

            objects[position.x][position.y].SetObject(erosObject);
            return true;
        }

        public bool IsPositionEmpty(Vector2Int position)
        {
            return objects[position.x][position.y].IsEmpty;
        }

        public override void Destroy()
        {
            foreach (var erosObject in objects.SelectMany(erosObjects => erosObjects))
            {
                erosObject.Destroy();
            }
        }
    }
}