using Descriptor.Grid;
using Entity.Grid;
using UnityEngine;

namespace ErosEditor.Entity.Grid
{
    public class GridCellInfo : MonoBehaviour
    {
        private Vector2 _position;

        private Material originalMaterial;
        public Material selectedMaterial;
        public GridCell cell;

        private void Start()
        {
            originalMaterial = GetComponent<MeshRenderer>().material;
        }

        public void Select()
        {
            GetComponent<MeshRenderer>().material = selectedMaterial;
        }

        public void Unselect()
        {
            GetComponent<MeshRenderer>().material = originalMaterial;
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
        }

        public Vector2 GetPosition()
        {
            return _position;
        }
    }
}