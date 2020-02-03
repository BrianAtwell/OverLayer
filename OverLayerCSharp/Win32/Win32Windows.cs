/**
 * <summary>
 * This is a Win32 unmanaged code compatible class.
 * This code mostly links to functions from the user32.dll.
 * Most of the these functions are used when interacting with Win32 Windows.
 * License MIT 2020
 * </summary>
 * <author>Brian Atwell</author>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OverLayerCSharp.Win32
{
    public static class Win32Windows
    {

        [DllImport("user32.dll", SetLastError = true)]
        public static extern System.UInt16 RegisterClassEx([In] ref WNDCLASSEX lpWndClass);

        [DllImport("user32.dll")]
        public static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool DestroyWindow(IntPtr hWnd);
    
        [DllImport("user32.dll")]
        public static extern void PostQuitMessage(int nExitCode);

        [DllImport("user32.dll")]
        public static extern sbyte GetMessage(out uint lpMsg, IntPtr hWnd, uint wMsgFilterMin,
           uint wMsgFilterMax);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern sbyte PeekMessage(out uint lpMsg, IntPtr hWnd, uint wMsgFilterMin,
           uint wMsgFilterMax, uint wRemoveMsg);

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);

        [DllImport("user32.dll")]
        public static extern bool TranslateMessage([In] ref uint lpMsg);

        [DllImport("user32.dll")]
        public static extern IntPtr DispatchMessage([In] ref uint lpmsg);

        [DllImport("user32.dll")]
        public static extern uint PostMessageW(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int TranslateAccelerator(IntPtr hwnd, IntPtr hAccelTable, [In] ref uint msg);

        [DllImport("user32.dll")]
        public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, uint flags);

        [DllImport("user32.dll")]
        public static extern bool RedrawWindow(IntPtr hWnd, ref RECT lprcUpdate, IntPtr hrgnUpdate, uint flags);

        [DllImport("user32.dll")]
        public static extern bool InvalidateRect(IntPtr hWnd, ref RECT lpRect, bool bErase);

        [DllImport("user32.dll")]
        public static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "CreateWindowEx")]
        public static extern IntPtr CreateWindowEx(
           UInt32 dwExStyle,
           string lpClassName,
           string lpWindowName,
           UInt32 dwStyle,
           int x,
           int y,
           int nWidth,
           int nHeight,
           IntPtr hWndParent,
           IntPtr hMenu,
           IntPtr hInstance,
           IntPtr lpParam);

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        [StructLayout(LayoutKind.Sequential)]
        public struct WNDCLASSEX
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public int style;
            public IntPtr lpfnWndProc; // not WndProc
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            public string lpszMenuName;
            public string lpszClassName;
            public IntPtr hIconSm;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SIZE
        {
            public long cx;
            public long cy;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        public delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        //[DllImport("user32.dll")]
        //public static extern IntPtr LoadIconW(IntPtr hinstance, string iconName);

        //[DllImport("user32.dll")]
        //public static extern IntPtr LoadCursorW(IntPtr hinstance, string cursorName);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadIcon(IntPtr hinstance, int lpIconName);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        // This static method is required because Win32 does not support
        // GetWindowLongPtr directly
        public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 8)
                return GetWindowLongPtr64(hWnd, nIndex);
            else
                return GetWindowLongPtr32(hWnd, nIndex);
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        // This static method is required because Win32 does not support
        // GetWindowLongPtr directly
        public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 8)
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            else
                return SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
        }

        [DllImport("user32.dll")]
        public static extern int SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        [DllImport("user32.dll")]
        public static extern IntPtr BeginPaint(IntPtr hwnd, out PAINTSTRUCT lpPaint);

        [DllImport("user32.dll")]
        public static extern IntPtr EndPaint(IntPtr hwnd, out PAINTSTRUCT lpPaint);

        [DllImport("user32.dll")]
        public static extern int DrawText(IntPtr hdc, string lpchText, int cchText, out Win32Windows.RECT lprc, uint format);

        public struct PAINTSTRUCT
        {
            IntPtr hdc;
            int fErase;
            Win32Windows.RECT rcPaint;
            int fRestore;
            int fIncUpdate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            sbyte[] rgbReserved;
        }

        [DllImport("user32.dll")]
        public static extern uint GetSysColor(int nIndex);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSysColorBrush(int nIndex);

        /*
         * RedrawWindow() flags
         */
        public const uint RDW_INVALIDATE = 0x0001;
        public const uint RDW_INTERNALPAINT = 0x0002;
        public const uint RDW_ERASE = 0x0004;
        public const uint RDW_VALIDATE = 0x0008;
        public const uint RDW_NOINTERNALPAINT = 0x0010;
        public const uint RDW_NOERASE = 0x0020;
        public const uint RDW_NOCHILDREN = 0x0040;
        public const uint RDW_ALLCHILDREN = 0x0080;
        public const uint RDW_UPDATENOW = 0x0100;
        public const uint RDW_ERASENOW = 0x0200;
        public const uint RDW_FRAME = 0x0400;
        public const uint RDW_NOFRAME = 0x0800;

        public const int COLOR_SCROLLBAR = 0;
        public const int COLOR_BACKGROUND = 1;
        public const int COLOR_ACTIVECAPTION = 2;
        public const int COLOR_INACTIVECAPTION = 3;
        public const int COLOR_MENU = 4;
        public const int COLOR_WINDOW = 5;
        public const int COLOR_WINDOWFRAME = 6;
        public const int COLOR_MENUTEXT = 7;
        public const int COLOR_WINDOWTEXT = 8;
        public const int COLOR_CAPTIONTEXT = 9;
        public const int COLOR_ACTIVEBORDER = 10;
        public const int COLOR_INACTIVEBORDER = 11;
        public const int COLOR_APPWORKSPACE = 12;
        public const int COLOR_HIGHLIGHT = 13;
        public const int COLOR_HIGHLIGHTTEXT = 14;
        public const int COLOR_BTNFACE = 15;
        public const int COLOR_BTNSHADOW = 16;
        public const int COLOR_GRAYTEXT = 17;
        public const int COLOR_BTNTEXT = 18;
        public const int COLOR_INACTIVECAPTIONTEXT = 19;
        public const int COLOR_BTNHIGHLIGHT = 20;
        public const int COLOR_3DDKSHADOW = 21;
        public const int COLOR_3DLIGHT = 22;
        public const int COLOR_INFOTEXT = 23;
        public const int COLOR_INFOBK = 24;
        public const int COLOR_HOTLIGHT = 26;
        public const int COLOR_GRADIENTACTIVECAPTION = 27;
        public const int COLOR_GRADIENTINACTIVECAPTION = 28;
        public const int COLOR_MENUHILIGHT = 29;
        public const int COLOR_MENUBAR = 30;
        public const int COLOR_DESKTOP = COLOR_BACKGROUND;
        public const int COLOR_3DFACE = COLOR_BTNFACE;
        public const int COLOR_3DSHADOW = COLOR_BTNSHADOW;
        public const int COLOR_3DHIGHLIGHT = COLOR_BTNHIGHLIGHT;
        public const int COLOR_3DHILIGHT = COLOR_BTNHIGHLIGHT;
        public const int COLOR_BTNHILIGHT = COLOR_BTNHIGHLIGHT;


        /*
         * Standard Cursor IDs
         */
        public const UInt32 IDC_ARROW = 32512;
        public const UInt32 IDC_IBEAM = 32513;
        public const UInt32 IDC_WAIT = 32514;
        public const UInt32 IDC_CROSS = 32515;
        public const UInt32 IDC_UPARROW = 32516;
        public const UInt32 IDC_SIZE = 32640;  /* OBSOLETE: use IDC_SIZEALL */
        public const UInt32 IDC_ICON = 32641;  /* OBSOLETE: use IDC_ARROW */
        public const UInt32 IDC_SIZENWSE = 32642;
        public const UInt32 IDC_SIZENESW = 32643;
        public const UInt32 IDC_SIZEWE = 32644;
        public const UInt32 IDC_SIZENS = 32645;
        public const UInt32 IDC_SIZEALL = 32646;
        public const UInt32 IDC_NO = 32648; /*not in win3.1 */
        public const UInt32 IDC_HAND = 32649;
        public const UInt32 IDC_APPSTARTING = 32650; /*not in win3.1 */
        public const UInt32 IDC_HELP = 32651;

        public const int GWL_EXSTYLE = -20;
        public const int GWLP_HINSTANCE = -6;
        public const int GWLP_ID = -12;
        public const int GWL_STYLE = -16;
        public const int GWLP_USERDATA = -21;
        public const int GWLP_WNDPROC = -4;

        public const uint LWA_COLORKEY = 0x00000001;


        public const int SM_CXSCREEN = 0;
        public const int SM_CYSCREEN = 1;
    }
}
