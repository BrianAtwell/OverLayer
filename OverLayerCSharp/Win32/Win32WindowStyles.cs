﻿/**
 * <summary>
 * This is a Win32 windows styles
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
    class Win32WindowStyles
    {
        public const UInt64 WS_OVERLAPPED = 0x00000000;
        public const UInt64 WS_POPUP = 0x80000000;
        public const UInt64 WS_CHILD = 0x40000000;
        public const UInt64 WS_MINIMIZE = 0x20000000;
        public const UInt64 WS_VISIBLE = 0x10000000;
        public const UInt64 WS_DISABLED = 0x08000000;
        public const UInt64 WS_CLIPSIBLINGS = 0x04000000;
        public const UInt64 WS_CLIPCHILDREN = 0x02000000;
        public const UInt64 WS_MAXIMIZE = 0x01000000;
        public const UInt64 WS_CAPTION = 0x00C00000;     /* WS_BORDER | WS_DLGFRAME  */
        public const UInt64 WS_BORDER = 0x00800000;
        public const UInt64 WS_DLGFRAME = 0x00400000;
        public const UInt64 WS_VSCROLL = 0x00200000;
        public const UInt64 WS_HSCROLL = 0x00100000;
        public const UInt64 WS_SYSMENU = 0x00080000;
        public const UInt64 WS_THICKFRAME = 0x00040000;
        public const UInt64 WS_GROUP = 0x00020000;
        public const UInt64 WS_TABSTOP = 0x00010000;

        public const UInt64 WS_MINIMIZEBOX = 0x00020000;
        public const UInt64 WS_MAXIMIZEBOX = 0x00010000;

        public const UInt64 WS_TILED = WS_OVERLAPPED;
        public const UInt64 WS_ICONIC = WS_MINIMIZE;
        public const UInt64 WS_SIZEBOX = WS_THICKFRAME;
        public const UInt64 WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW;

        public const UInt64 WS_OVERLAPPEDWINDOW  = (WS_OVERLAPPED | 
            WS_CAPTION | WS_SYSMENU | WS_THICKFRAME |
            WS_MINIMIZEBOX | WS_MAXIMIZEBOX);

        public const UInt64 WS_POPUPWINDOW = (WS_POPUP | WS_BORDER | WS_SYSMENU);

        public const UInt64 WS_CHILDWINDOW = (WS_CHILD);
    }
}
