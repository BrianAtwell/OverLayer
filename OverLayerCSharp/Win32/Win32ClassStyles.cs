/**
 * <summary>
 * This is a Win32 windows class styles
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
    public static class Win32ClassStyles
    {
        /*
         * Class styles
         */
        public const UInt32 CS_VREDRAW = 0x0001;
        public const UInt32 CS_HREDRAW = 0x0002;
        public const UInt32 CS_DBLCLKS = 0x0008;
        public const UInt32 CS_OWNDC = 0x0020;
        public const UInt32 CS_CLASSDC = 0x0040;
        public const UInt32 CS_PARENTDC = 0x0080;
        public const UInt32 CS_NOCLOSE = 0x0200;
        public const UInt32 CS_SAVEBITS = 0x0800;
        public const UInt32 CS_BYTEALIGNCLIENT = 0x1000;
        public const UInt32 CS_BYTEALIGNWINDOW = 0x2000;
        public const UInt32 CS_GLOBALCLASS = 0x4000;
        public const UInt32 CS_IME = 0x00010000;
        public const UInt32 CS_DROPSHADOW = 0x00020000;
    }
}
