using Descriptors;
using Entity.Eros;
using Entity.Grid;
using UnityEngine;

namespace Descriptor.Grid
{
    public class GridCellDescriptor : AbstractDescriptor<GridCell>
    {
        public readonly Vector2Int Position;
        public readonly ErosObject ErosObject;

        public GridCellDescriptor(Vector2Int position, ErosObject erosObject)
        {
            Position = position;
            ErosObject = erosObject;
        }
    }
}