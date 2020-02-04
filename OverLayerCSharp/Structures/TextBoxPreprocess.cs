

using OverLayerCSharp.Win32;
/**
* <summary>
* This is a TextBoxPreprocess class handles initial conversion from XML string.
* Values must be keep as a string for additional processing.
* License MIT 2020
* </summary>
* <author>Brian Atwell</author>
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace OverLayerCSharp.Structures
{
    public class TextBoxPreprocess
    {
        public string text;
        public string x;
        public string y;
        public string width;
        public string height;
        public string color;
        public string fontName;
        public Nullable<int> fontSize;
        public string status;

        public TextBoxPreprocess()
        {

        }

        public static TextBoxPreprocess Deserialize(string xmlString)
        {
            TextBoxPreprocess textbox = null;
            XmlSerializer serializer = new XmlSerializer(typeof(TextBoxPreprocess));
            using (var reader = new StringReader(xmlString))
            {
                textbox = (TextBoxPreprocess)serializer.Deserialize(reader);
            }

            return textbox;
        }

        public string Serialize()
        {
            StringBuilder sb = new StringBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof(TextBoxPreprocess));
            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, this);
            }

            return sb.ToString();
        }

        public TextBoxData ToTextBoxData(IntPtr hWnd)
        {
            TextBoxData textBox = new TextBoxData();

            textBox.Text = text;
            textBox.X = int.Parse(x);
            textBox.Y = int.Parse(y);
            textBox.Width = int.Parse(width);
            textBox.Height = int.Parse(height);

            textBox.Color = null;

            try {
                textBox.Color = Convert.ToUInt32(color, 16);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Exception {0} Color {1}", ex, color);
            }
            
            if(textBox.Color.HasValue)
            {
                Debug.WriteLine("Color RED {0}, GREEN{1}, BLUE{2}",textBox.Color&0xFF, (textBox.Color>>8) & 0xFF, (textBox.Color>>16) & 0xFF);
            }

            return textBox;
        }
    }
}
