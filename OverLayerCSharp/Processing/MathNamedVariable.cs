using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverLayerCSharp.Processing
{
    public class MathNamedVariable
    {
        public readonly float Value;

        public readonly string Name;

        public MathNamedVariable(string name, float value)
        {
            Value = value;
            Name = name;
        }
    }
}
