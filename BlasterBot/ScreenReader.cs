using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sDrawing = System.Drawing;
using wMedia = System.Windows.Media;

namespace BlasterBot
{
    class ScreenReader
    {
        BoardPosition boardPosition;
        public ScreenReader(BoardPosition boardPosition)
        {
            this.boardPosition = boardPosition;
        }
        public GemBoard readScreen()
        {
            GemBoard gemBoard = new GemBoard();

            int width = boardPosition.getWidth();
            int height = boardPosition.getHeight();
            int x1 = boardPosition.getX1();
            int y1 = boardPosition.getY1();
            using (sDrawing.Bitmap screenBmp = new sDrawing.Bitmap(width, height+50, sDrawing.Imaging.PixelFormat.Format32bppArgb))
            {
                using (sDrawing.Graphics bmpGraphics = sDrawing.Graphics.FromImage(screenBmp))
                {
                    bmpGraphics.CopyFromScreen(x1, y1, 0, 0, screenBmp.Size);
                }

                for (int gridX = 0; gridX < 8; gridX++)
                {
                    for (int gridY = 0; gridY < 8; gridY++)
                    {
                        int screenX;
                        int screenY;
                        boardPosition.gridToScreenOffset(gridX, gridY, out screenX, out screenY);
                        wMedia.Color color = colorFromBitmap(screenBmp, screenX, screenY);
                        gemBoard.setColor(gridX, gridY, color);
                    }
                }
            }
            return gemBoard;
        }
        private wMedia.Color colorFromBitmap(sDrawing.Bitmap bitmap, int x, int y)
        {
            sDrawing.Color outputColor = bitmap.GetPixel(x, y);
            wMedia.Color color = wMedia.Color.FromArgb(outputColor.A, outputColor.R, outputColor.G, outputColor.B);
            return color;
        }
    }
}
