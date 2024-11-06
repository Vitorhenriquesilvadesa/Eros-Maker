using UnityEngine;
using UnityEngine.Serialization;

namespace Scriptable
{
    [CreateAssetMenu(fileName = "GridAppearance", menuName = "ScriptableObjects/GridAppearance", order = 2)]
    public class GridAppearanceInfo : ScriptableObject
    {
        public Material lightMaterial;
        public Material darkMaterial;
        public Material selectedMaterial;
        public Mesh mesh;
    }
}