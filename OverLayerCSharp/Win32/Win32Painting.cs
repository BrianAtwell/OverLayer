using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OverLayerCSharp.Win32
{
    public static class Win32Painting
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct LOGFONT
        {
            public int lfHeight;
            public int lfWidth;
            public int lfEscapement;
            public int lfOrientation;
            public int lfWeight;
            public byte lfItalic;
            public byte lfUnderline;
            public byte lfStrikeOut;
            public byte lfCharSet;
            public byte lfOutPrecision;
            public byte lfClipPrecision;
            public byte lfQuality;
            public byte lfPitchAndFamily;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string lfFaceName;
        }

        public static UInt32 RGB(byte red, byte green, byte blue)
        {
            UInt32 result = 0;
            result = (UInt32) red & 0xFF;
            result |= (UInt32)(((int)green&0xFF) << 8) & 0xFFFF;
            result |= (UInt32)(((int)blue&0xFF) << 16);
            return (UInt32)result;
        }

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateSolidBrush(UInt32 ColorRef);

        [DllImport("gdi32.dll")]
        public static extern int SetBkMode(IntPtr hdc, int mode);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateFontIndirectW(ref LOGFONT lplf);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr obj);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SetTextColor(IntPtr hdc, IntPtr color);

        public static class BackgroundMode
        {
            public const int TRANSPARENT = 1;
            public const int OPAQUE = 2;
            public const int BKMODE_LAST = 2;
        };
    }
}
