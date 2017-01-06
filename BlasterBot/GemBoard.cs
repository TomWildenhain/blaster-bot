using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BlasterBot
{
    class GemBoard
    {
        private static Random random = new Random();

        GemTypes[,] gemArray;
        Color[,] gemColors;
        public GemBoard()
        {
            gemArray = new GemTypes[8, 8];
            gemColors = new Color[8, 8];
        }
        public void setGem(int gridX, int gridY, GemTypes gem)
        {
            gemArray[gridX, gridY] = gem;
        }
        public GemTypes getGem(int gridX, int gridY)
        {
            return gemArray[gridX, gridY];
        }
        public Color getColor(int gridX, int gridY)
        {
            return gemColors[gridX, gridY];
        }
        public void setColor(int gridX, int gridY, Color color)
        {
            gemColors[gridX, gridY] = color;
            gemArray[gridX, gridY] = gemFromColor(color);
        }
        public bool findMove(ref int x1, ref int y1, ref int x2, ref int y2)
        {
            if(findMove(true, ref x1, ref y1, ref x2, ref y2))
            {
                return true;
            }
            else if(findMove(false, ref x1, ref y1, ref x2, ref y2))
            {
                return true;
            }
            return false;
        }
        private bool findMove(bool vertical, ref int x1, ref int y1, ref int x2, ref int y2)
        {
            Random random = new Random();
            int start1 = random.Next(0, 8);
            for (int Offset1 = 0; Offset1 < 8; Offset1++)
            {
                int search1 = (start1 + Offset1) % 8;
                int start2 = random.Next(0, 8);
                for (int Offset2 = 0; Offset2 < 6; Offset2++)
                {
                    int search2 = (start2 + Offset2) % 8;
                    if(findMove(vertical, search1, search2, ref x1, ref y1, ref x2, ref y2))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private bool findMove(bool vertical, int search1, int search2, 
                              ref int x1, ref int y1, ref int x2, ref int y2)
        {
            int x = search1;
            int y = search2;
            int longDirX = 1;
            int longDirY = 0;
            int shortDirX = 0;
            int shortDirY = 1;
            if (vertical)
            {
                y = search1;
                x = search2;
                longDirX = 0;
                longDirY = 1;
                shortDirX = 1;
                shortDirY = 0;
            }
            if (checkEqual(x, y, x + longDirX, y + longDirY))
            {
                setPoint(x + longDirX * 2, y + longDirY * 2, ref x1, ref y1);
                if (checkEqual(x, y, x + longDirX * 2 + shortDirX, y + longDirY * 2 + shortDirY))
                {
                    setPoint(x + longDirX * 2 + shortDirX, y + longDirY * 2 + shortDirY, ref x2, ref y2);
                    return true;
                }
                if (checkEqual(x, y, x + longDirX * 2 - shortDirX, y + longDirY * 2 - shortDirY))
                {
                    setPoint(x + longDirX * 2 - shortDirX, y + longDirY * 2 - shortDirY, ref x2, ref y2);
                    return true;
                }
                if(checkEqual(x, y, x + longDirX * 3, y + longDirY * 3))
                {
                    setPoint(x + longDirX * 3, y + longDirY * 3, ref x2, ref y2);
                    return true;
                }
            }
            else if (checkEqual(x, y, x + longDirX * 2, y + longDirY * 2))
            {
                setPoint(x + longDirX, y + longDirY, ref x1, ref y1);
                if (checkEqual(x, y, x + longDirX + shortDirX, y + longDirY + shortDirY))
                {
                    setPoint(x + longDirX + shortDirX, y + longDirY + shortDirY, ref x2, ref y2);
                    return true;
                }
                if (checkEqual(x, y, x + longDirX - shortDirX, y + longDirY - shortDirY))
                {
                    setPoint(x + longDirX - shortDirX, y + longDirY - shortDirY, ref x2, ref y2);
                    return true;
                }
            }
            else if (checkEqual(x + longDirX, y + longDirY, x + longDirX * 2, y + longDirY * 2))
            {
                setPoint(x, y, ref x1, ref y1);
                if (checkEqual(x + shortDirX, y + shortDirY, x + longDirX, y + longDirY))
                {
                    setPoint(x + shortDirX, y + shortDirY, ref x2, ref y2);
                    return true;
                }
                if (checkEqual(x - shortDirX, y - shortDirY, x + longDirX, y + longDirY))
                {
                    setPoint(x - shortDirX, y - shortDirY, ref x2, ref y2);
                    return true;
                }
                if (checkEqual(x - longDirX, y - longDirY, x + longDirX, y + longDirY))
                {
                    setPoint(x - longDirX, y - longDirY, ref x2, ref y2);
                    return true;
                }
            }
            return false;
        }
        private void setPoint(int x, int y, ref int rx, ref int ry)
        {
            rx = x;
            ry = y;
        }
        private bool checkEqual(int cx1, int cy1, int cx2, int cy2)
        {
            if(!isValidCoord(cx1)|| !isValidCoord(cy1) || 
               !isValidCoord(cx2) || !isValidCoord(cy2))
            {
                return false;
            }
            return gemArray[cx1, cy1] == gemArray[cx2, cy2];
        }
        private bool isValidCoord(int coord)
        {
            return 0 <= coord && coord < 8;
        }
        private GemTypes gemFromColor(Color color)
        {
            string identifyerString = "";
            identifyerString += getNumberFromColorComp(color.R);
            identifyerString += getNumberFromColorComp(color.G);
            identifyerString += getNumberFromColorComp(color.B);
            GemTypes resultGem = GemTypes.Unknown;
            switch (identifyerString)
            {
                case "333":
                    resultGem = GemTypes.White;
                    break;
                case "311":
                    resultGem = GemTypes.Red;
                    break;
                case "332":
                    resultGem = GemTypes.Orange;
                    break;
                case "331":
                    resultGem = GemTypes.Yellow;
                    break;
                case "232":
                    resultGem = GemTypes.Green;
                    break;
                case "132":
                    resultGem = GemTypes.Green;
                    break;
                case "123":
                    resultGem = GemTypes.Blue;
                    break;
                case "313":
                    resultGem = GemTypes.Pink;
                    break;
                case "212":
                    resultGem = GemTypes.Pink;
                    break;
                case "312":
                    resultGem = GemTypes.Pink;
                    break;
                case "213":
                    resultGem = GemTypes.Pink;
                    break;
            }
            return resultGem;
        }
        private string getNumberFromColorComp(int value)
        {
            if (value < 90)
            {
                return "1";
            }
            else if (value < 192)
            {
                return "2";
            }
            else
            {
                return "3";
            }
        }
    }
}
