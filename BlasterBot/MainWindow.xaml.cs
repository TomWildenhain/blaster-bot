using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Keys = System.Windows.Forms.Keys;

namespace BlasterBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // <for reading keys>
        [StructLayout(LayoutKind.Sequential)]
        private struct KBDLLHOOKSTRUCT
        {
            public Keys key;
            public int scanCode;
            public int flags;
            public int time;
            public IntPtr extra;
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int id, LowLevelKeyboardProc callback, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hook);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hook, int nCode, IntPtr wp, IntPtr lp);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string name);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern short GetAsyncKeyState(Keys key);

        private IntPtr ptrHook;
        private LowLevelKeyboardProc objKeyboardProcess;

        // </for reding keys>


        BoardPosition boardPosition;
        ScreenReader screenReader;
        MouseController mouseController;

        bool point1Assigned = false;
        bool point2Assigned = false;
        bool running = false;
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            buildColorGrid();
            setBoardPosition(0, 0, 0, 0);
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dispatcherTimer.Start();
            registerHotkeys();
        }
        private void buildColorGrid()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Rectangle currentRectangle = new Rectangle();
                    currentRectangle.Name = "R" + x + "x" + y;
                    RegisterName(currentRectangle.Name, currentRectangle);

                    TextBlock currentTextBlock = new TextBlock();
                    currentTextBlock.Name = "T" + x + "x" + y;
                    RegisterName(currentTextBlock.Name, currentTextBlock);

                    colorGrid.Children.Add(currentRectangle);
                    colorGrid.Children.Add(currentTextBlock);
                    Grid.SetColumn(currentRectangle, x);
                    Grid.SetRow(currentRectangle, y);
                    Grid.SetColumn(currentTextBlock, x);
                    Grid.SetRow(currentTextBlock, y);
                }
            }
        }

        private void Classic_Click(object sender, RoutedEventArgs e)
        {
            setGameMode(1060, 93, 1852, 886);
        }
        private void Zen_Click(object sender, RoutedEventArgs e)
        {
            Classic_Click(sender, e);
        }
        private void Butterflies_Click(object sender, RoutedEventArgs e)
        {
            Lightning_Click(sender, e);
        }
        private void Lightning_Click(object sender, RoutedEventArgs e)
        {
            setGameMode(1060, 137, 1848, 925);
        }
        private void IceStorm_Click(object sender, RoutedEventArgs e)
        {
            setGameMode(1060, 124, 1850, 919);
        }
        private void Poker_Click(object sender, RoutedEventArgs e)
        {
            Classic_Click(sender, e);
        }
        private void DiamondMine_Click(object sender, RoutedEventArgs e)
        {
            setGameMode(1038, 136, 1824, 931);
        }
        private void ReadScreen_Click(object sender, RoutedEventArgs e)
        {
            showPreviewIfAssigned();
        }
        private void Begin_Click(object sender, RoutedEventArgs e)
        {
            if (!running)
            {
                dispatcherTimer.Stop();
                int interval = Convert.ToInt32(TBXdelayTime.Text);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, interval);
                dispatcherTimer.Start();
                running = true;
                BNbegin.Content = "Stop";
            }
            else
            {
                dispatcherTimer.Stop();
                int interval = Convert.ToInt32(TBXdelayTime.Text);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
                dispatcherTimer.Start();
                running = false;
                BNbegin.Content = "Begin";
            }
        }

        private void showPreviewIfAssigned()
        {
            if (point1Assigned && point2Assigned)
            {
                displayColorGrid(screenReader.readScreen());
            }
        }

        private void setGameMode(int x1, int y1, int x2, int y2)
        {
            setBoardPosition(x1, y1, x2, y2);
            point1Assigned = true;
            point2Assigned = true;
            TBtopLeft.Text = "Top left: [auto]";
            TBbottomRight.Text = "Bottom right: [auto]";
            if (!BejeweledController.positionBejeweled())
            {
                MessageBox.Show("Bejeweled not open.");
            }
            displayColorGrid(screenReader.readScreen());
        }
        private void displayColorGrid(GemBoard gemBoard)
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Rectangle currentRectangle = (Rectangle)FindName("R" + x + "x" + y);
                    TextBlock currentTextBlock = (TextBlock)FindName("T" + x + "x" + y);
                    currentRectangle.Fill = new SolidColorBrush(gemBoard.getColor(x, y));
                    string type = GemInfo.gemToString(gemBoard.getGem(x, y));
                    currentTextBlock.Text = type;
                }
            }
        }
        private void setBoardPosition(int x1, int y1, int x2, int y2)
        {
            boardPosition = new BoardPosition(x1, y1, x2, y2);
            screenReader = new ScreenReader(boardPosition);
            mouseController = new MouseController(boardPosition);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (point1Assigned && point2Assigned)
            {
                if (running)
                {
                    int x1 = 0;
                    int y1 = 0;
                    int x2 = 0;
                    int y2 = 0;
                    GemBoard gemBoard = screenReader.readScreen();
                    displayColorGrid(gemBoard);
                    if (gemBoard.findMove(ref x1, ref y1, ref x2, ref y2))
                    {
                        mouseController.matchTwo(x1, y1, x2, y2);
                    }
                }
                else
                {
                    displayColorGrid(screenReader.readScreen());
                }
            }
        }
        private void registerHotkeys()
        {
            ProcessModule objCurrentModule = Process.GetCurrentProcess().MainModule; //Get Current Module
            objKeyboardProcess = new LowLevelKeyboardProc(captureKey); //Assign callback function each time keyboard process
            ptrHook = SetWindowsHookEx(13, objKeyboardProcess, GetModuleHandle(objCurrentModule.ModuleName), 0); //Setting Hook of Keyboard Process for current module
        }
        private IntPtr captureKey(int nCode, IntPtr wp, IntPtr lp)
        {
            if (nCode >= 0)
            {
                KBDLLHOOKSTRUCT objKeyInfo = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lp, typeof(KBDLLHOOKSTRUCT));

                if (objKeyInfo.key == Keys.D1)
                {
                    setTopLeft();
                }
                else if (objKeyInfo.key == Keys.D2)
                {
                    setBottomRight();
                }
            }
            return CallNextHookEx(ptrHook, nCode, wp, lp);
        }
        private void setTopLeft()
        {
            int x1;
            int y1;
            int x2 = boardPosition.getX2();
            int y2 = boardPosition.getY2();
            mouseController.getMouseCoords(out x1, out y1);
            point1Assigned = true;
            setBoardPosition(x1, y1, x2, y2);
            TBtopLeft.Text = "Top left: (" + x1 + "," + y1 + ")";
            showPreviewIfAssigned();
        }
        private void setBottomRight()
        {
            int x1 = boardPosition.getX1();
            int y1 = boardPosition.getY1();
            int x2;
            int y2;
            mouseController.getMouseCoords(out x2, out y2);
            point2Assigned = true;
            setBoardPosition(x1, y1, x2, y2);
            TBbottomRight.Text = "Bottom right: (" + x2 + "," + y2 + ")";
            showPreviewIfAssigned();
        }

        private void MakeMove_Click(object sender, RoutedEventArgs e)
        {
            colsSmash(0, 0);
            /*
            if (point1Assigned && point2Assigned)
            {
                int x1 = 0;
                int y1 = 0;
                int x2 = 0;
                int y2 = 0;
                screenReader.readScreen().findMove(ref x1, ref y1, ref x2, ref y2);
                mouseController.matchTwo(x1, y1, x2, y2);
            }*/
        }
        private void rowsSmash(int offset, int evenOdd)
        {
            for(int y = 0; y < 8; y++)
            {
                if (y % 3 == offset)
                {
                    rowSmash(y, evenOdd);
                }
            }
        }
        private void colsSmash(int offset, int evenOdd)
        {
            for (int x = 0; x < 8; x++)
            {
                if (x % 3 == offset)
                {
                    colSmash(x, evenOdd);
                }
            }
        }
        private void rowSmash(int y, int evenOdd)
        {
            for(int x = 0; x < 7; x++)
            {
                if (x % 2 == evenOdd)
                {
                    mouseController.matchTwo(x, y, x + 1, y);
                }
            }
        }
        private void colSmash(int x, int evenOdd)
        {
            for (int y = 0; y < 7; y++)
            {
                if (y % 2 == evenOdd)
                {
                    mouseController.matchTwo(x, y, x, y + 1);
                }
            }
        }
    }
}
