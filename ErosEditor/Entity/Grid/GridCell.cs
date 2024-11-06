using Descriptor.Grid;
using Descriptors;
using Entity.Eros;
using UnityEngine;

namespace Entity.Grid
{
    public class GridCell : AbstractEntity<GridCell>
    {
        private ErosObject _erosObject;
        private GridCellDescriptor _descriptor;
        public bool IsEmpty => _erosObject is NullErosObject;

        public override AbstractDescriptor<GridCell> GetDescriptor()
        {
            return new GridCellDescriptor(_descriptor.Position, _descriptor.ErosObject);
        }

        public void SetObject(ErosObject erosObject)
        {
            if (_erosObject is not NullErosObject)
            {
                _erosObject.Destroy();
            }

            _erosObject = erosObject;
            erosObject.Init();
        }

        public override void Destroy()
        {
            _erosObject.Destroy();
        }
    }
}