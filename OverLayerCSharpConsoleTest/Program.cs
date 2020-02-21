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
            string mathStr = "7+8+5";

            data = parser.Eval(mathStr);

            Console.WriteLine(string.Format("{0} == {1}", mathStr, data));

            MathNamedVariable[] myVariables = new MathNamedVariable[5];
            myVariables[0] = new MathNamedVariable("ABC", 1.5f);
            myVariables[1] = new MathNamedVariable("ABD", 3f);
            myVariables[2] = new MathNamedVariable("AC", 4.5f);
            myVariables[3] = new MathNamedVariable("ACA", 6f);
            myVariables[4] = new MathNamedVariable("ACB", 7.5f);

            ObjectStringSearch mySearch = new ObjectStringSearch(myVariables, "Name");

            mySearch.Reset();
            mySearch.Start();
            foreach (char c in "AC")
            {
                mySearch.SearchChar(c);
            }

            int pos = mySearch.End();
            Console.WriteLine("Position {0} string {1}", pos, pos>0 && pos<myVariables.Length ? myVariables[pos].Name : "[no string]");
        }
    }
}
