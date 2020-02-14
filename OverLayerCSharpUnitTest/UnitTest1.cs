using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OverLayerCSharp.Processing;

namespace OverLayerCSharpUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            MathParser parser = new MathParser();

            float data;

            data = parser.Eval("7+8+5");

            Console.WriteLine("Hello World");

            Assert.AreEqual((7 + 8 + 5), data);
        }
    }
}
