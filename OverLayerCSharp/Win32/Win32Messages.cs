﻿/**
 * <summary>
 * This is a Win32 windows messages
 * License MIT 2020
 * </summary>
 * <author>Brian Atwell</author>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverLayerCSharp.Win32
{
    public static class Win32Messages
    {
        /*
 * Window Messages
 */

        public const UInt32 WM_NULL = 0x0000;
        public const UInt32 WM_CREATE = 0x0001;
        public const UInt32 WM_DESTROY = 0x0002;
        public const UInt32 WM_MOVE = 0x0003;
        public const UInt32 WM_SIZE = 0x0005;
        public const UInt32 WM_ACTIVATE = 0x0006;
        /*
         * WM_ACTIVATE state values
         */
        public const UInt32 WA_INACTIVE = 0;
        public const UInt32 WA_ACTIVE = 1;
        public const UInt32 WA_CLICKACTIVE = 2;
        public const UInt32 WM_SETFOCUS = 0x0007;
        public const UInt32 WM_KILLFOCUS = 0x0008;
        public const UInt32 WM_ENABLE = 0x000A;
        public const UInt32 WM_SETREDRAW = 0x000B;
        public const UInt32 WM_SETTEXT = 0x000C;
        public const UInt32 WM_GETTEXT = 0x000D;
        public const UInt32 WM_GETTEXTLENGTH = 0x000E;
        public const UInt32 WM_PAINT = 0x000F;
        public const UInt32 WM_CLOSE = 0x0010;
        public const UInt32 WM_QUERYENDSESSION = 0x0011;
        public const UInt32 WM_QUERYOPEN = 0x0013;
        public const UInt32 WM_ENDSESSION = 0x0016;
        public const UInt32 WM_QUIT = 0x0012;
        public const UInt32 WM_ERASEBKGND = 0x0014;
        public const UInt32 WM_SYSCOLORCHANGE = 0x0015;
        public const UInt32 WM_SHOWWINDOW = 0x0018;
        public const UInt32 WM_WININICHANGE = 0x001A;
        public const UInt32 WM_SETTINGCHANGE = WM_WININICHANGE;
        public const UInt32 WM_DEVMODECHANGE = 0x001B;
        public const UInt32 WM_ACTIVATEAPP = 0x001C;
        public const UInt32 WM_FONTCHANGE = 0x001D;
        public const UInt32 WM_TIMECHANGE = 0x001E;
        public const UInt32 WM_CANCELMODE = 0x001F;
        public const UInt32 WM_SETCURSOR = 0x0020;
        public const UInt32 WM_MOUSEACTIVATE = 0x0021;
        public const UInt32 WM_CHILDACTIVATE = 0x0022;
        public const UInt32 WM_QUEUESYNC = 0x0023;
        public const UInt32 WM_GETMINMAXINFO = 0x0024;

        public const UInt32 WM_NOTIFY = 0x004E;
        public const UInt32 WM_INPUTLANGCHANGEREQUEST = 0x0050;
        public const UInt32 WM_INPUTLANGCHANGE = 0x0051;
        public const UInt32 WM_TCARD = 0x0052;
        public const UInt32 WM_HELP = 0x0053;
        public const UInt32 WM_USERCHANGED = 0x0054;
        public const UInt32 WM_NOTIFYFORMAT = 0x0055;
        public const UInt32 WM_CONTEXTMENU = 0x007B;
        public const UInt32 WM_STYLECHANGING = 0x007C;
        public const UInt32 WM_STYLECHANGED = 0x007D;
        public const UInt32 WM_DISPLAYCHANGE = 0x007E;
        public const UInt32 WM_GETICON = 0x007F;
        public const UInt32 WM_SETICON = 0x0080;
        public const UInt32 WM_NCCREATE = 0x0081;
        public const UInt32 WM_NCDESTROY = 0x0082;
        public const UInt32 WM_NCCALCSIZE = 0x0083;
        public const UInt32 WM_NCHITTEST = 0x0084;
        public const UInt32 WM_NCPAINT = 0x0085;
        public const UInt32 WM_NCACTIVATE = 0x0086;
        public const UInt32 WM_GETDLGCODE = 0x0087;
        public const UInt32 WM_SYNCPAINT = 0x0088;
        public const UInt32 WM_NCMOUSEMOVE = 0x00A0;
        public const UInt32 WM_NCLBUTTONDOWN = 0x00A1;
        public const UInt32 WM_NCLBUTTONUP = 0x00A2;
        public const UInt32 WM_NCLBUTTONDBLCLK = 0x00A3;
        public const UInt32 WM_NCRBUTTONDOWN = 0x00A4;
        public const UInt32 WM_NCRBUTTONUP = 0x00A5;
        public const UInt32 WM_NCRBUTTONDBLCLK = 0x00A6;
        public const UInt32 WM_NCMBUTTONDOWN = 0x00A7;
        public const UInt32 WM_NCMBUTTONUP = 0x00A8;
        public const UInt32 WM_NCMBUTTONDBLCLK = 0x00A9;
        public const UInt32 WM_NCXBUTTONDOWN = 0x00AB;
        public const UInt32 WM_NCXBUTTONUP = 0x00AC;
        public const UInt32 WM_NCXBUTTONDBLCLK = 0x00AD;
        public const UInt32 WM_INPUT_DEVICE_CHANGE = 0x00FE;
        public const UInt32 WM_INPUT = 0x00FF;
        public const UInt32 WM_KEYFIRST = 0x0100;
        public const UInt32 WM_KEYDOWN = 0x0100;
        public const UInt32 WM_KEYUP = 0x0101;
        public const UInt32 WM_CHAR = 0x0102;
        public const UInt32 WM_DEADCHAR = 0x0103;
        public const UInt32 WM_SYSKEYDOWN = 0x0104;
        public const UInt32 WM_SYSKEYUP = 0x0105;
        public const UInt32 WM_SYSCHAR = 0x0106;
        public const UInt32 WM_SYSDEADCHAR = 0x0107;
        public const UInt32 WM_UNICHAR = 0x0109;
        public const UInt32 WM_KEYLAST = 0x0109;
        public const UInt32 UNICODE_NOCHAR = 0xFFFF;
        public const UInt32 WM_IME_STARTCOMPOSITION = 0x010D;
        public const UInt32 WM_IME_ENDCOMPOSITION = 0x010E;
        public const UInt32 WM_IME_COMPOSITION = 0x010F;
        public const UInt32 WM_IME_KEYLAST = 0x010F;
        public const UInt32 WM_INITDIALOG = 0x0110;
        public const UInt32 WM_COMMAND = 0x0111;
        public const UInt32 WM_SYSCOMMAND = 0x0112;
        public const UInt32 WM_TIMER = 0x0113;
        public const UInt32 WM_HSCROLL = 0x0114;
        public const UInt32 WM_VSCROLL = 0x0115;
        public const UInt32 WM_INITMENU = 0x0116;
        public const UInt32 WM_INITMENUPOPUP = 0x0117;
        public const UInt32 WM_GESTURE = 0x0119;
        public const UInt32 WM_GESTURENOTIFY = 0x011A;
        public const UInt32 WM_MENUSELECT = 0x011F;
        public const UInt32 WM_MENUCHAR = 0x0120;
        public const UInt32 WM_ENTERIDLE = 0x0121;
        public const UInt32 WM_MENURBUTTONUP = 0x0122;
        public const UInt32 WM_MENUDRAG = 0x0123;
        public const UInt32 WM_MENUGETOBJECT = 0x0124;
        public const UInt32 WM_UNINITMENUPOPUP = 0x0125;
        public const UInt32 WM_MENUCOMMAND = 0x0126;
        public const UInt32 WM_CHANGEUISTATE = 0x0127;
        public const UInt32 WM_UPDATEUISTATE = 0x0128;
        public const UInt32 WM_QUERYUISTATE = 0x0129;
        public const UInt32 WM_CTLCOLORMSGBOX = 0x0132;
        public const UInt32 WM_CTLCOLOREDIT = 0x0133;
        public const UInt32 WM_CTLCOLORLISTBOX = 0x0134;
        public const UInt32 WM_CTLCOLORBTN = 0x0135;
        public const UInt32 WM_CTLCOLORDLG = 0x0136;
        public const UInt32 WM_CTLCOLORSCROLLBAR = 0x0137;
        public const UInt32 WM_CTLCOLORSTATIC = 0x0138;
        public const UInt32 WM_MOUSEFIRST = 0x0200;
        public const UInt32 WM_MOUSEMOVE = 0x0200;
        public const UInt32 WM_LBUTTONDOWN = 0x0201;
        public const UInt32 WM_LBUTTONUP = 0x0202;
        public const UInt32 WM_LBUTTONDBLCLK = 0x0203;
        public const UInt32 WM_RBUTTONDOWN = 0x0204;
        public const UInt32 WM_RBUTTONUP = 0x0205;
        public const UInt32 WM_RBUTTONDBLCLK = 0x0206;
        public const UInt32 WM_MBUTTONDOWN = 0x0207;
        public const UInt32 WM_MBUTTONUP = 0x0208;
        public const UInt32 WM_MBUTTONDBLCLK = 0x0209;
        public const UInt32 WM_MOUSEWHEEL = 0x020A;
        public const UInt32 WM_XBUTTONDOWN = 0x020B;
        public const UInt32 WM_XBUTTONUP = 0x020C;
        public const UInt32 WM_XBUTTONDBLCLK = 0x020D;
        public const UInt32 WM_MOUSEHWHEEL = 0x020E;
        public const UInt32 WM_PARENTNOTIFY = 0x0210;
        public const UInt32 WM_ENTERMENULOOP = 0x0211;
        public const UInt32 WM_EXITMENULOOP = 0x0212;
        public const UInt32 WM_NEXTMENU = 0x0213;
        public const UInt32 WM_SIZING = 0x0214;
        public const UInt32 WM_CAPTURECHANGED = 0x0215;
        public const UInt32 WM_MOVING = 0x0216;
        public const UInt32 WM_POWERBROADCAST = 0x0218;
    }
}
