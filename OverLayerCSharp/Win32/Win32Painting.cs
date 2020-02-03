/**
 * <summary>
 * This is a Win32 unmanaged code compatible class.
 * This code mostly links to functions from the gdi32.dll and that is why it is call Win32Painting.
 * As these functions are used for Win32 drawing and code used in WM_PAINT message.
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
    public static class Win32Painting
    {
        [StructLayout(LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Auto, Pack = 1), Serializable]
        public struct LOGFONT
        {
            public int Height;
            public int Width;
            public int Escapement;
            public int Orientation;
            public int Weight;
            public byte Italic;
            public byte Underline;
            public byte StrikeOut;
            public byte CharSet;
            public byte OutPrecision;
            public byte ClipPrecision;
            public byte Quality;
            public byte PitchAndFamily;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string FaceName;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Auto, Pack = 1)]
        unsafe public struct LOGFONT_CHAR
        {
            public int Height;
            public int Width;
            public int Escapement;
            public int Orientation;
            public int Weight;
            public byte Italic;
            public byte Underline;
            public byte StrikeOut;
            public byte CharSet;
            public byte OutPrecision;
            public byte ClipPrecision;
            public byte Quality;
            public byte PitchAndFamily;
            //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public fixed char FaceName[32];
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
        public static extern int GetObject(IntPtr hdc, int size, IntPtr ptr);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hobj);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SetTextColor(IntPtr hdc, IntPtr color);

        public static class BackgroundMode
        {
            public const int TRANSPARENT = 1;
            public const int OPAQUE = 2;
            public const int BKMODE_LAST = 2;
        };

        // Font Weights //
        public const int FW_DONTCARE = 0;
        public const int FW_THIN = 100;
        public const int FW_EXTRALIGHT = 200;
        public const int FW_LIGHT = 300;
        public const int FW_NORMAL = 400;
        public const int FW_MEDIUM = 500;
        public const int FW_SEMIBOLD = 600;
        public const int FW_BOLD = 700;
        public const int FW_EXTRABOLD = 800;
        public const int FW_HEAVY = 900;
        public const int FW_ULTRALIGHT = FW_EXTRALIGHT;
        public const int FW_REGULAR = FW_NORMAL;
        public const int FW_DEMIBOLD = FW_SEMIBOLD;
        public const int FW_ULTRABOLD = FW_EXTRABOLD;
        public const int FW_BLACK = FW_HEAVY;

        // Character set Constants
        public const int ANSI_CHARSET = 0;
        public const int DEFAULT_CHARSET = 1;
        public const int SYMBOL_CHARSET = 2;
        public const int SHIFTJIS_CHARSET = 128;
        public const int HANGEUL_CHARSET = 129;
        public const int HANGUL_CHARSET = 129;
        public const int GB2312_CHARSET = 134;
        public const int CHINESEBIG5_CHARSET = 136;
        public const int OEM_CHARSET = 255;

        // Out Precision
        public const int OUT_DEFAULT_PRECIS = 0;
        public const int OUT_STRING_PRECIS = 1;
        public const int OUT_CHARACTER_PRECIS = 2;
        public const int OUT_STROKE_PRECIS = 3;
        public const int OUT_TT_PRECIS = 4;
        public const int OUT_DEVICE_PRECIS = 5;
        public const int OUT_RASTER_PRECIS = 6;
        public const int OUT_TT_ONLY_PRECIS = 7;
        public const int OUT_OUTLINE_PRECIS = 8;
        public const int OUT_SCREEN_OUTLINE_PRECIS = 9;
        public const int OUT_PS_ONLY_PRECIS = 10;

        // Clip
        public const int CLIP_DEFAULT_PRECIS = 0;
        public const int CLIP_CHARACTER_PRECIS = 1;
        public const int CLIP_STROKE_PRECIS = 2;
        public const int CLIP_MASK = 0xf;
        public const int CLIP_LH_ANGLES = (1 << 4);
        public const int CLIP_TT_ALWAYS = (2 << 4);
        public const int CLIP_EMBEDDED = (8 << 4);

        //Quality
        public const int DEFAULT_QUALITY = 0;
        public const int DRAFT_QUALITY = 1;
        public const int PROOF_QUALITY = 2;
        public const int NONANTIALIASED_QUALITY = 3;
        public const int ANTIALIASED_QUALITY = 4;

        //Pitch
        public const int DEFAULT_PITCH = 0;
        public const int FIXED_PITCH = 1;
        public const int VARIABLE_PITCH = 2;
        public const int MONO_FONT = 8;

        // Font Families 
        public const int FF_DONTCARE = (0 << 4);  /* Don't care or don't know. */
        public const int FF_ROMAN = (1 << 4);  /* Variable stroke width, serifed. */
                                               /* Times Roman, Century Schoolbook, etc. */
        public const int FF_SWISS = (2 << 4);  /* Variable stroke width, sans-serifed. */
                                               /* Helvetica, Swiss, etc. */
        public const int FF_MODERN = (3 << 4);  /* Constant stroke width, serifed or sans-serifed. */
                                                /* Pica, Elite, Courier, etc. */
        public const int FF_SCRIPT = (4 << 4);  /* Cursive, etc. */
        public const int FF_DECORATIVE = (5 << 4);  /* Old English, etc. */
    }
}
