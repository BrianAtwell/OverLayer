﻿using OverLayerCSharp.Network;
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
        private IntPtr wndHwnd;
        private IntPtr hInstance;
        private ServerThread serverThread;

        //Has to be static to avoid NullReference Exception
        public static volatile TextBoxPreprocess _serverTextBox =null;
        public static volatile TextBoxPreprocess _textBox;

        public void CallbackServerTextBoxUpdate(TextBoxPreprocess rtextBox)
        {
            _serverTextBox = rtextBox;
        }

        public void BindIPAddressPort(IPAddress ip, int port)
        {
            Console.WriteLine("IP Address: {0} port {1}", ip, port);
        }

        public OverLayWindow()
        {
            _textBox = null;
            _serverTextBox = null;
            serverThread = new ServerThread();
            serverThread.UpdateTextBox += CallbackServerTextBoxUpdate;
            //serverThread.BindAddressPort += BindIPAddressPort;
            serverThread.Start();
        }

        public bool CreateWindow()
        {
            wndHwnd = Win32Windows.CreateWindowEx(0, win32ClassName, wndTitle, (UInt32)Win32WindowStyles.WS_POPUP, 0, 0, 300, 300, IntPtr.Zero, IntPtr.Zero, hInstance, IntPtr.Zero);

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

            while (gmRet != -1)
            {
                //gmRet = Win32Windows.GetMessage(out msg, IntPtr.Zero, 0, 0);
                gmRet = Win32Windows.PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
                if(gmRet != 0)
                {
                    gmRet = Win32Windows.GetMessage(out msg, IntPtr.Zero, 0, 0);

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

                if (_serverTextBox != null)
                {
                    _textBox = _serverTextBox;
                    _serverTextBox = null;

                    if (_textBox != null)
                    {
                        Debug.WriteLine("Received text data {0}", _textBox.text);
                        MessageBox.Show(string.Format("Received text data {0}", _textBox.text));
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
                        string text = "Test";
                        Win32Windows.RECT rect;
                        rect.left = 0;
                        rect.right = 200;
                        rect.bottom = 200;
                        rect.top = 0;

                        Win32Windows.PAINTSTRUCT ps;
                        IntPtr hdc = Win32Windows.BeginPaint(hWnd, out ps);
                        // TODO: Add any drawing code that uses hdc here...4
                        Win32Painting.SetBkMode(hdc, Win32Painting.BackgroundMode.TRANSPARENT);
                        Win32Windows.DrawText(hdc, text, text.Length, out rect, 0);
                        Win32Windows.EndPaint(hWnd, out ps);
                    }
                    break;

                case Win32Messages.WM_LBUTTONDOWN:
                    Debug.WriteLine(string.Format("MOUSE Single Click"));
                    //MessageBox.Show("Singleclick");
                    break;

                case Win32Messages.WM_DESTROY:
                    //Win32Windows.DestroyWindow(hWnd);

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
