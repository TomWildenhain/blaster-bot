using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlasterBot
{
    class BoardPosition
    {
        private int x1;
        private int y1;
        private int x2;
        private int y2;
        private int xSpan;
        private int ySpan;
        private int xHalfStep;
        private int yHalfStep;
        public BoardPosition(int x1, int y1, int x2, int y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            xSpan = x2 - x1;
            ySpan = y2 - y1;
            xHalfStep = xSpan / 8 / 2;
            yHalfStep = ySpan / 8 / 2;
        }
        public void gridToScreenCoord(int gridX, int gridY, out int screenX, out int screenY)
        {
            //@REQUIRES 0 <= gridX <= 7, same for gridY
            screenX = xSpan * gridX / 8 + x1 + xHalfStep;
            screenY = ySpan * gridY / 8 + y1 + yHalfStep;
        }
        public void gridToScreenOffset(int gridX, int gridY, out int screenX, out int screenY)
        {
            //@REQUIRES 0 <= gridX <= 7, same for gridY
            screenX = xSpan * gridX / 8 + xHalfStep;
            screenY = ySpan * gridY / 8 + yHalfStep;
        }
        public int getWidth()
        {
            return xSpan;
        }
        public int getHeight()
        {
            return ySpan;
        }
        public int getX1()
        {
            return x1;
        }
        public int getY1()
        {
            return y1;
        }
        public int getX2()
        {
            return x2;
        }
        public int getY2()
        {
            return y2;
        }
    }
}
