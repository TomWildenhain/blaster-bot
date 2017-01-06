using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace BlasterBot
{
    class MouseController
    {
        private const int LEFTDOWN = 0x02;
        private const int LEFTUP = 0x04;
        private const int RIGHTDOWN = 0x08;
        private const int RIGHTUP = 0x10;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        private BoardPosition boardPosition;
        public MouseController(BoardPosition boardPosition)
        {
            this.boardPosition = boardPosition;
        }
        private void clickTwice(int screenX1, int screenY1, int screenX2, int screenY2)
        {
            int currentx = System.Windows.Forms.Cursor.Position.X;
            int currenty = System.Windows.Forms.Cursor.Position.Y;
            SetCursorPos(screenX1, screenY1);
            mouse_event(LEFTDOWN, screenX1, screenY1, 0, 0);
            mouse_event(LEFTUP, screenX1, screenY1, 0, 0);
            SetCursorPos(screenX2, screenY2);
            mouse_event(LEFTDOWN, screenX2, screenY2, 0, 0);
            mouse_event(LEFTUP, screenX2, screenY2, 0, 0);
            SetCursorPos(currentx, currenty);
        }
        public void matchTwo(int gridX1, int gridY1, int gridX2, int gridY2)
        {
            int screenX1;
            int screenY1;
            int screenX2;
            int screenY2;
            boardPosition.gridToScreenCoord(gridX1, gridY1, out screenX1, out screenY1);
            boardPosition.gridToScreenCoord(gridX2, gridY2, out screenX2, out screenY2);
            clickTwice(screenX1, screenY1, screenX2, screenY2);
        }
        public void getMouseCoords(out int x, out int y)
        {
            x = System.Windows.Forms.Cursor.Position.X;
            y = System.Windows.Forms.Cursor.Position.Y;
        }
    }
}
