using System;
using System.Collections.Generic;
using Descriptor.Grid;
using Entity.Grid;
using ErosEditor.Entity.Grid;
using ErosEditor.EventSystem;
using Event.Grid;
using EventSystem;
using Scriptable;
using UnityEngine;

namespace Controller.Grid
{
    public class GridRenderingController : ApplicationController<GridRenderingController>
    {
        public GridLayerDescriptor activeLayer { get; private set; }
        private List<GameObject> gridLines = new();
        private List<GameObject> backgroundQuads = new();

        private int ignoredLayerMask;
        private int gridLayerMask;

        [SerializeField] private GridLineInfo lineInfo;
        [SerializeField] private GridAppearanceInfo appearanceInfo;

        private bool setupGridLayer;
        private bool setupIgnoredLayer;

        public void SetActiveLayer(GridLayerDescriptor descriptor)
        {
            activeLayer = descriptor;
            DestroyGridLines();
            DestroyBackgroundQuads();
            InitLayerRenderData(descriptor);
        }


        private void InitLayerRenderData(GridLayerDescriptor descriptor)
        {
            GenerateLines(descriptor);
            GenerateLayerBackground(descriptor);
        }

        private void GenerateLayerBackground(GridLayerDescriptor descriptor)
        {
            bool lineStartIsDark = false;

            float halfWidth = descriptor.cellWidth / 2f;
            float halfHeight = descriptor.cellHeight / 2f;

            if (!setupGridLayer)
            {
                gridLayerMask = LayerMask.NameToLayer("Grid");
                setupGridLayer = true;
            }

            for (int x = 0; x < descriptor.width; x++)
            {
                bool isDark = lineStartIsDark;

                for (int z = 0; z < descriptor.height; z++)
                {
                    isDark = !isDark;

                    GameObject quad = new GameObject
                    {
                        layer = gridLayerMask
                    };

                    MeshFilter meshFilter = quad.AddComponent<MeshFilter>();
                    MeshRenderer meshRenderer = quad.AddComponent<MeshRenderer>();
                    meshFilter.mesh = appearanceInfo.mesh;
                    meshRenderer.material = isDark ? appearanceInfo.darkMaterial : appearanceInfo.lightMaterial;
                    quad.transform.localScale = new Vector3(descriptor.cellWidth, descriptor.cellHeight, 1f);
                    quad.transform.position = new Vector3(x * descriptor.cellWidth + halfWidth, 0,
                        z * descriptor.cellHeight + halfHeight);
                    quad.transform.eulerAngles = new Vector3(90f, 0f, 0f);

                    quad.AddComponent<MeshCollider>();

                    GridCellInfo cellInfo = quad.AddComponent<GridCellInfo>();
                    cellInfo.SetPosition(new Vector2(x, z));
                    cellInfo.selectedMaterial = appearanceInfo.selectedMaterial;

                    backgroundQuads.Add(quad);
                }

                lineStartIsDark = !lineStartIsDark;
            }
        }

        private void GenerateLines(GridLayerDescriptor descriptor)
        {
            gridLines = new List<GameObject>();

            if (!setupIgnoredLayer)
            {
                ignoredLayerMask = LayerMask.NameToLayer("Ignore Raycast");
                setupIgnoredLayer = true;
            }

            for (int y = 0; y <= descriptor.height; y++)
            {
                GameObject horizontalLine = new GameObject($"HorizontalLine_{y}")
                {
                    layer = ignoredLayerMask
                };

                LineRenderer lineRenderer = horizontalLine.AddComponent<LineRenderer>();
                lineRenderer.positionCount = 2;
                lineRenderer.startWidth = lineInfo.lineStartWidth;
                lineRenderer.endWidth = lineInfo.lineEndWidth;
                lineRenderer.material = lineInfo.lineMaterial;

                Vector3 start = new Vector3(0, 0, y * descriptor.cellHeight);
                Vector3 end = new Vector3(descriptor.width * descriptor.cellWidth, 0, y * descriptor.cellHeight);

                lineRenderer.SetPosition(0, start);
                lineRenderer.SetPosition(1, end);

                gridLines.Add(horizontalLine);
            }

            for (int x = 0; x <= descriptor.width; x++)
            {
                GameObject verticalLine = new GameObject($"VerticalLine_{x}");

                LineRenderer lineRenderer = verticalLine.AddComponent<LineRenderer>();
                lineRenderer.positionCount = 2;
                lineRenderer.startWidth = lineInfo.lineStartWidth;
                lineRenderer.endWidth = lineInfo.lineEndWidth;
                lineRenderer.material = lineInfo.lineMaterial;

                Vector3 start = new Vector3(x * descriptor.cellWidth, 0, 0);
                Vector3 end = new Vector3(x * descriptor.cellWidth, 0, descriptor.height * descriptor.cellHeight);

                lineRenderer.SetPosition(0, start);
                lineRenderer.SetPosition(1, end);

                gridLines.Add(verticalLine);
            }
        }


        private void DestroyGridLines()
        {
            foreach (GameObject line in gridLines)
            {
                Destroy(line);
            }

            gridLines.Clear();
        }

        private void DestroyBackgroundQuads()
        {
            foreach (GameObject quad in backgroundQuads)
            {
                Destroy(quad);
            }

            backgroundQuads.Clear();
        }

        [Reactive]
        public void OnGridActiveLayerChange(ChangeGridActiveLayerEvent e)
        {
            activeLayer = e.Descriptor;
            SetActiveLayer(activeLayer);
        }
    }
}