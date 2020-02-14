using OverLayerCSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverLayerCSharpConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            MathParser parser = new MathParser();

            float data;

            data = parser.Eval("7+8+5");

            Console.WriteLine("Hello World");
        }
    }
}
