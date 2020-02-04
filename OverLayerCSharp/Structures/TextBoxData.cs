/**
 * <summary>
 * This is a TextBoxData class handles values after processing by the OverLayWindow.
 * License MIT 2020
 * </summary>
 * <author>Brian Atwell</author>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverLayerCSharp.Structures
{
    public class TextBoxData
    {
        public string Text { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Nullable<UInt32> Color { get; set; }
        public Nullable<int> FontSize;

    }
}
