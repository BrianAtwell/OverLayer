/**
 * <summary>
 * This file holds our implmentation of the OverlayWindow. This is a Win32 window with
 * special flags that can create a transparent window on top of all windows. When text is
 * drawn it will be on top of everything.
 * License MIT 2020
 * </summary>
 * <author>Brian Atwell</author>
 */


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
    /**
     * OverLayWindow class currently MessageLoop is uses static variables in as
     * as an object due to a NullReferenceException work around.
     */
    public class OverLayWindow
    {
        // Red brush used as our background to enable transparancy
        private static IntPtr redBrush = IntPtr.Zero;
        private Win32Windows.WndProc delegWndProc = myWndProc;
        private const string win32ClassName = "OverLayerWindow\0";
        private const string wndTitle = "OverLayer\0";
        // C++ leftover
        //private UInt32 IDI_OVERLAYER=107;
        private ServerThread serverThread;

        // Lock
        static readonly object _serverLock = new object();

        //Has to be static to avoid NullReference Exception
        private static IntPtr wndHwnd;
        private static IntPtr hInstance;
        private static volatile TextBoxPreprocess _serverTextBox =null;
        private static volatile TextBoxData _textBox;
        private static volatile TextBoxData _prevTextBox;
        private static Win32Painting.LOGFONT _fontParams;
        private static IntPtr _font=IntPtr.Zero;

        public static bool IsRunning;

        /**
         * <summary>This is called by serverThread</summary>
         * <param name="rtextBox">This is the TextBox string sent by the client.</param>
         */
        public void CallbackServerTextBoxUpdate(TextBoxPreprocess rtextBox)
        {
            lock(_serverLock)
            {
                _serverTextBox = rtextBox;
            }
        }

        /**
         * <summary>This is called by serverThread</summary>
         * <param name="rtextBox">This is the TextBox string sent by the client.</param>
         */
        public void BindIPAddressPort(IPAddress ip, int port)
        {
            lock (_serverLock)
            {
                Console.WriteLine("IP Address: {0} port {1}", ip, port);
            }
        }

        /**
         * <summary>Constructor</summary>
         */
        public OverLayWindow()
        {
            IsRunning = true;

            _font = IntPtr.Zero;

            _textBox = new TextBoxData();
            _serverTextBox = null;
            _prevTextBox = null;
            _textBox.Text = "test";
            _textBox.X = 0;
            _textBox.Y = 0;
            _textBox.Width = 200;
            _textBox.Height = 200;

            serverThread = new ServerThread();
            serverThread.UpdateTextBox += CallbackServerTextBoxUpdate;
            //serverThread.BindAddressPort += BindIPAddressPort;
            serverThread.Start();

        }


        /**
         * <summary>Get the width and height of the Screen</summary>
         */
        public static Win32Windows.SIZE GetScreenSize()
        {
            Win32Windows.SIZE size;
            size.cx = Win32Windows.GetSystemMetrics(Win32Windows.SM_CXSCREEN);
            size.cy = Win32Windows.GetSystemMetrics(Win32Windows.SM_CYSCREEN);

            return size;
        }

        /**
         * <summary>Create the Window</summary>
         */
        public bool CreateWindow()
        {
            Win32Windows.SIZE screenSize = GetScreenSize();
            wndHwnd = Win32Windows.CreateWindowEx(0, win32ClassName, wndTitle, (UInt32)Win32WindowStyles.WS_POPUP, 0, 0, (int)screenSize.cx, (int)screenSize.cy, IntPtr.Zero, IntPtr.Zero, hInstance, IntPtr.Zero);

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

        /**
         * <summary>Register the Window</summary>
         */
        public System.UInt16 RegisterWindow()
        {
            ushort result = 0;
            hInstance = Marshal.GetHINSTANCE(this.GetType().Module);
            Win32Windows.WNDCLASSEX wcex;

            //redBrush = Win32Painting.CreateSolidBrush(Win32Painting.RGB(255, 0, 0));

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

        /**
         * <summary>Set Default font parameters and set the fontsize an fontName</summary>
         */
        public static Win32Painting.LOGFONT SetFont(int fontsize, string fontName, Nullable<int> format)
        {
            Win32Painting.LOGFONT fontParams;
            fontParams.Height = fontsize;
            fontParams.Width = 0;
            fontParams.Escapement = 0;
            fontParams.Orientation = 0;
            if (format.HasValue)
            {
                fontParams.Weight = format.Value;
            }
            else
            {
                fontParams.Weight = Win32Painting.FW_DONTCARE;
            }
            fontParams.Italic = 0;
            fontParams.Underline = 0;
            fontParams.StrikeOut = 0;
            fontParams.CharSet = Win32Painting.ANSI_CHARSET;
            fontParams.OutPrecision = Win32Painting.OUT_DEFAULT_PRECIS;
            fontParams.ClipPrecision = Win32Painting.CLIP_DEFAULT_PRECIS;
            fontParams.Quality = Win32Painting.DEFAULT_QUALITY;
            fontParams.PitchAndFamily = Win32Painting.DEFAULT_PITCH | Win32Painting.FF_SWISS;
            fontParams.FaceName = fontName;

            return fontParams;
        }

        /**
         * <summary>
         * This function is a demonstration of how to get the current font.
         * This does not work when the current font is a system font.
         * So this method is not to be used but remains as an example.
         * </summary>
         * <param name="hdc">hdc from beginPaint or similiar API</param>
         * <param name="isSecondMethod">False it will use method 2 and true it will use method 1</param>
         */
        public static unsafe Win32Painting.LOGFONT GetDefaultFont(IntPtr hdc, bool isSecondMethod)
        {
            Win32Painting.LOGFONT_CHAR tempLogFontChar = new Win32Painting.LOGFONT_CHAR();

            IntPtr pointer;
            GCHandle hndl = new GCHandle();

            if (isSecondMethod)
            {
                // Method 2 
                hndl = GCHandle.Alloc(tempLogFontChar, GCHandleType.Pinned);
                pointer = hndl.AddrOfPinnedObject();
            }
            else
            {
                // Method 1
                pointer = new IntPtr(&tempLogFontChar);

                Marshal.StructureToPtr(tempLogFontChar, pointer, fDeleteOld: false);
            }

            if (Win32Painting.GetObject(hdc, Marshal.SizeOf(typeof(Win32Painting.LOGFONT)), pointer) == 0)
            {
                Debug.WriteLine("GetObject Failed to get font.");
            }

            tempLogFontChar = Marshal.PtrToStructure<Win32Painting.LOGFONT_CHAR>(pointer);

            Win32Painting.LOGFONT tempLogFont;

            // We have to convert from an unmanaged type where we have fixed char array
            //_defaultLogFont = tempLogFont;
            tempLogFont.Height = tempLogFontChar.Height;
            tempLogFont.Width = tempLogFontChar.Width;
            tempLogFont.Escapement = tempLogFontChar.Escapement;
            tempLogFont.Orientation = tempLogFontChar.Orientation;
            tempLogFont.Weight = tempLogFontChar.Weight;
            tempLogFont.Italic = tempLogFontChar.Italic;
            tempLogFont.Underline = tempLogFontChar.Underline;
            tempLogFont.StrikeOut = tempLogFontChar.StrikeOut;
            tempLogFont.CharSet = tempLogFontChar.CharSet;
            tempLogFont.OutPrecision = tempLogFontChar.OutPrecision;
            tempLogFont.ClipPrecision = tempLogFontChar.ClipPrecision;
            tempLogFont.Quality = tempLogFontChar.Quality;
            tempLogFont.PitchAndFamily = tempLogFontChar.PitchAndFamily;
            tempLogFont.FaceName = new string(tempLogFontChar.FaceName);

            hndl.Free();

            return tempLogFont;
        }

        /**
         * <summary>This is the window Message Loop.</summary>
         */
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
                        _prevTextBox = _textBox;
                        _textBox = _serverTextBox.ToTextBoxData(wndHwnd);
                        if(!string.IsNullOrEmpty(_serverTextBox.status) && _serverTextBox.status.ToLower().Equals("stop"))
                        {
                            Win32Windows.DestroyWindow(wndHwnd);
                        }

                        if (_textBox != null)
                        {
                            Debug.WriteLine("Received text data {0}", _textBox.Text);

                            if (!string.IsNullOrEmpty(_serverTextBox.fontName) && _serverTextBox.fontSize.HasValue)
                            {
                                if(_font != IntPtr.Zero)
                                {
                                    Win32Painting.DeleteObject(_font);
                                    
                                }
                                _fontParams = SetFont(_serverTextBox.fontSize.Value, _serverTextBox.fontName, _serverTextBox.fontWeight);
                                _font = Win32Painting.CreateFontIndirectW(ref _fontParams);
                            }
                            
                            Win32Windows.InvalidateRect(wndHwnd, IntPtr.Zero, true);
                        }

                        _serverTextBox = null;
                    }
                }
            }
            return gmRet;
        }

        /**
         * <summary>This is window procedure pointed by the register window.</summary>
         */
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
                            IntPtr oldColor = IntPtr.Zero;
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

                            if(_textBox != null && _textBox.Color.HasValue)
                            {
                                Win32Painting.SetTextColor(hdc, _textBox.Color.Value);
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

                    // Delete font
                    if (_font != IntPtr.Zero)
                    {
                        Win32Painting.DeleteObject(_font);

                    }

                    if (redBrush != IntPtr.Zero)
                    {
                        Win32Painting.DeleteObject(redBrush);
                    }

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
