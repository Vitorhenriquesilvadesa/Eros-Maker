using System;
using UnityEngine;

namespace Entity.Grid
{
    public class GridCellInfo : MonoBehaviour
    {
        private Vector2 _position;

        private Material originalMaterial;
        public Material selectedMaterial;

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