using OverLayerCSharp.Network;
using OverLayerCSharp.Structures;
using OverLayerCSharp.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OverLayerCSharp
{
    public class OverLayWindow
    {
        private IntPtr redBrush;
        private Win32Windows.WndProc delegWndProc = myWndProc;
        private const string win32ClassName = "OverLayerWindow\0";
        private const string wndTitle = "OverLayer\0";
        private UInt32 IDI_OVERLAYER=107;
        public static IntPtr wndHwnd;
        private IntPtr hInstance;
        private ServerThread serverThread;

        static readonly object _serverLock = new object();

        //Has to be static to avoid NullReference Exception
        public static volatile TextBoxPreprocess _serverTextBox =null;
        public static volatile TextBoxData _textBox;
        public static Win32Painting.LOGFONT _fontParams;
        public static volatile IntPtr _font;

        public static bool IsRunning;

        public void CallbackServerTextBoxUpdate(TextBoxPreprocess rtextBox)
        {
            lock(_serverLock)
            {
                _serverTextBox = rtextBox;
            }
        }

        public void BindIPAddressPort(IPAddress ip, int port)
        {
            lock (_serverLock)
            {
                Console.WriteLine("IP Address: {0} port {1}", ip, port);
            }
        }

        public OverLayWindow()
        {
            IsRunning = true;
            _textBox = new TextBoxData();
            _serverTextBox = null;
            serverThread = new ServerThread();
            serverThread.UpdateTextBox += CallbackServerTextBoxUpdate;
            //serverThread.BindAddressPort += BindIPAddressPort;
            serverThread.Start();

            _textBox.Text = "test";
            _textBox.X = 0;
            _textBox.Y = 0;
            _textBox.Width = 200;
            _textBox.Height = 200;
        }

        public static Win32Windows.SIZE GetWindowSize()
        {
            Win32Windows.SIZE size;
            size.cx = Win32Windows.GetSystemMetrics(Win32Windows.SM_CXSCREEN);
            size.cy = Win32Windows.GetSystemMetrics(Win32Windows.SM_CYSCREEN);

            return size;
        }

        public bool CreateWindow()
        {
            Win32Windows.SIZE size = GetWindowSize();
            wndHwnd = Win32Windows.CreateWindowEx(0, win32ClassName, wndTitle, (UInt32)Win32WindowStyles.WS_POPUP, 0, 0, (int)size.cx, (int)size.cy, IntPtr.Zero, IntPtr.Zero, hInstance, IntPtr.Zero);

            if (wndHwnd == ((IntPtr)0))
            {
                uint error = Win32Windows.GetLastError();
                Debug.WriteLine(string.Format("CreateWindowEx Error: {0}", error));
                return false;
            }
            Win32Windows.ShowWindow(wndHwnd, 1);
            Win32Windows.UpdateWindow(wndHwnd);

            return true;
        }

        //
        //  FUNCTION: MyRegisterClass()
        //
        //  PURPOSE: Registers the window class.
        //
        public System.UInt16 RegisterWindow()
        {
            ushort result = 0;
            hInstance = Marshal.GetHINSTANCE(this.GetType().Module);
            Win32Windows.WNDCLASSEX wcex;

            uint color = Win32Painting.RGB(255, 0, 0);
            Debug.WriteLine(string.Format("color {0}",color));

            redBrush = Win32Painting.CreateSolidBrush(Win32Painting.RGB(255, 0, 0));

            wcex.cbSize = Marshal.SizeOf(typeof(Win32Windows.WNDCLASSEX));

            wcex.style = (int)(Win32ClassStyles.CS_HREDRAW | Win32ClassStyles.CS_VREDRAW);
            wcex.lpfnWndProc = Marshal.GetFunctionPointerForDelegate(delegWndProc);
            wcex.cbClsExtra = 0;
            wcex.cbWndExtra = 0;
            wcex.hInstance = hInstance;
            wcex.hIcon = IntPtr.Zero; //Win32Windows.LoadIcon(IntPtr.Zero, (int)IDI_OVERLAYER);
            wcex.hCursor = Win32Windows.LoadCursor(IntPtr.Zero, (int)Win32Windows.IDC_ARROW);
            wcex.hbrBackground = Win32Windows.GetSysColorBrush(Win32Windows.COLOR_WINDOW); // = redBrush;
            wcex.lpszMenuName = null;//= MAKEINTRESOURCEW(IDC_OVERLAYER);
            wcex.lpszClassName = win32ClassName;
            wcex.hIconSm = IntPtr.Zero; //Win32Windows.LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

            result = Win32Windows.RegisterClassEx(ref wcex);

            if(result == 0)
            {
                uint error = Win32Windows.GetLastError();
                Debug.WriteLine(string.Format("RegisterClassEx Error: {0}", error));
            }

            return result;
        }

        public static int MessageLoop()
        {
            uint msg = 0;
            sbyte gmRet = 0;

            while (IsRunning && gmRet != -1)
            {
                //gmRet = Win32Windows.GetMessage(out msg, wndHwnd, 0, 0);
                gmRet = Win32Windows.PeekMessage(out msg, wndHwnd, 0, 0, 0);
                if(gmRet != 0)
                {
                    gmRet = Win32Windows.GetMessage(out msg, wndHwnd, 0, 0);

                    if (gmRet == -1)
                    {
                        return 0;
                    }
                    else
                    {
                        Win32Windows.TranslateMessage(ref msg);
                        Win32Windows.DispatchMessage(ref msg);
                    }
                }

                lock(_serverLock)
                {
                    if (_serverTextBox != null)
                    {
                        _textBox = _serverTextBox.ToTextBoxData(wndHwnd);
                        _serverTextBox = null;

                        if (_textBox != null)
                        {
                            Debug.WriteLine("Received text data {0}", _textBox.Text);
                            //MessageBox.Show(string.Format("Received text data {0}", _textBox.Text));

                            //Win32Windows.RedrawWindow(wndHwnd, ref rect, IntPtr.Zero, Win32Windows.RDW_INTERNALPAINT);
                            Win32Windows.InvalidateRect(wndHwnd, IntPtr.Zero, true);

                            //Win32Windows.SendMessage(wndHwnd, Win32Messages.WM_PAINT, IntPtr.Zero, IntPtr.Zero);
                        }
                        //Win32Windows.UpdateWindow(wndHwnd);
                        //Win32Windows.RedrawWindow(wndHwnd, IntPtr.Zero, IntPtr.Zero, Win32Windows.RDW_INTERNALPAINT);
                        //Win32Windows.SendMessage(wndHwnd, Win32Messages.WM_PAINT, IntPtr.Zero, IntPtr.Zero);
                    }
                }
            }
            return gmRet;
        }

        public static IntPtr myWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                case Win32Messages.WM_CREATE:
                    {
                        IntPtr curExStyle;
                        curExStyle = Win32Windows.GetWindowLongPtr(hWnd, Win32Windows.GWL_EXSTYLE);
                        Win32Windows.SetWindowLongPtr(hWnd, Win32Windows.GWL_EXSTYLE, (IntPtr)(((int)curExStyle) | Win32ExtendedStyles.WS_EX_LAYERED));
                        //Win32Messages.SetWindowLong(hWnd, GWL_EXSTYLE, GetWindowLong(hWnd, GWL_EXSTYLE) | WS_EX_LAYERED);
                        // Make red pixels transparent:
                        Win32Windows.SetLayeredWindowAttributes(hWnd, Win32Windows.GetSysColor(Win32Windows.COLOR_WINDOW), 0, Win32Windows.LWA_COLORKEY);
                    }
                    break;
                // All GUI painting must be done here
                case Win32Messages.WM_PAINT:
                    {
                        lock(_serverLock)
                        {
                            IntPtr oldFont = IntPtr.Zero;
                            Debug.WriteLine("Window Paint");
                            Win32Windows.RECT rect;
                            rect.left = (int)_textBox.X;
                            rect.right = (int)_textBox.Width;
                            rect.bottom = (int)_textBox.Height;
                            rect.top = (int)_textBox.Y;

                            Win32Windows.PAINTSTRUCT ps;
                            IntPtr hdc = Win32Windows.BeginPaint(hWnd, out ps);

                            Win32Painting.SetBkMode(hdc, Win32Painting.BackgroundMode.TRANSPARENT);
                            if (_font != IntPtr.Zero)
                            {
                                oldFont = Win32Painting.SelectObject(hdc, _font);
                            }
                            Win32Windows.DrawText(hdc, _textBox.Text, _textBox.Text.Length, out rect, 0);

                            if (_font != IntPtr.Zero)
                            {
                                Win32Painting.SelectObject(hdc, oldFont);
                            }
                            Win32Windows.EndPaint(hWnd, out ps);
                        }
                    }
                    break;

                case Win32Messages.WM_LBUTTONDOWN:
                    Debug.WriteLine(string.Format("MOUSE Single Click"));
                    //MessageBox.Show("Singleclick");
                    break;

                case Win32Messages.WM_DESTROY:
                    //Win32Windows.DestroyWindow(hWnd);
                    Debug.WriteLine("Msg: WM_DESTROY");
                    IsRunning = false;

                    //If you want to shutdown the application, call the next function instead of DestroyWindow
                    Win32Windows.PostQuitMessage(0);
                    break;

                default:
                    return Win32Windows.DefWindowProc(hWnd, msg, wParam, lParam);
            }
            return IntPtr.Zero;
        }
    }
}
