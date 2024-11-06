using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(fileName = "GridLineInfo", menuName = "ScriptableObjects/GridData", order = 1)]
    public class GridLineInfo : ScriptableObject
    {
        public Material lineMaterial;
        public float lineStartWidth;
        public float lineEndWidth;
    }
}