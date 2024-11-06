using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller.Grid
{
    public class ErosCursorWarper
    {
        private int screenWidth = Screen.width;
        private int screenHeight = Screen.height;
        private int offsetX = 32;
        private int offsetY = 32;

        private Mouse mouse = Mouse.current;

        public void Update()
        {
            if (Input.GetMouseButton(1))
            {
                WarpCursor();
            }
        }

        private void WarpCursor()
        {
            Vector3 mousePosition = Input.mousePosition;

            if (mousePosition.x >= screenWidth - offsetX)
            {
                mousePosition.x = offsetX + 1;
            }
            else if (mousePosition.x <= offsetX)
            {
                mousePosition.x = screenWidth - 1 - offsetX;
            }

            if (mousePosition.y >= screenHeight - offsetY)
            {
                mousePosition.y = offsetY + 1;
            }
            else if (mousePosition.y <= offsetY)
            {
                mousePosition.y = screenHeight - 1- offsetY;
            }

            Cursor.visible = true;
            mouse.WarpCursorPosition(mousePosition);
        }
    }
}