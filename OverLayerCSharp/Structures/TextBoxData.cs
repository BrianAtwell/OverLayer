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
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
    }
}
