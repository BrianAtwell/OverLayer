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
    }
}
