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

            return textBox;
        }
    }
}
