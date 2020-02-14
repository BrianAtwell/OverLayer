using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverLayerCSharp.Processing
{
    public class MathParser
    {
        public MathNamedVariable[] SortedVariables;

        private const char LEFT_PARENTHESIS = '(';
        private const char RIGHT_PARENTHESIS = ')';
        private const char MATH_SUBTRACTION = '-';
        private const char MATH_ADDITION = '+';
        private const char MATH_MULTIPLICATION = '*';
        private const char MATH_DIVISION = '/';
        private const char DECIMAL_POINT = '.';

        private enum MathOpEnum { None, Add, Subtrack, Multiply, Divide };

        private enum MathParseStateEnum { Search, VariableNameA, NumberA, NumAPostDec, PreOp, PostOp, VariableNameB, NumberB, NumBPostDec };

        private class MathValueState
        {
            public Nullable<float> TermA;
            public Nullable<float> TermB;
            public MathOpEnum Op;
            public MathParseStateEnum State;

            public MathValueState()
            {
                TermA = null;
                TermB = null;
                Op = MathOpEnum.None;
                State = MathParseStateEnum.Search;
            }
        }

        private List<MathValueState> StateStack = new List<MathValueState>();


        public MathParser()
        {

        }

        private float CalculateFormula(ref MathValueState valueState)
        {
            if(valueState.TermA.HasValue && valueState.TermB.HasValue && valueState.Op != MathOpEnum.None)
            {
                switch(valueState.Op)
                {
                    case MathOpEnum.Add:
                        return valueState.TermA.Value + valueState.TermB.Value;
                    case MathOpEnum.Subtrack:
                        return valueState.TermA.Value - valueState.TermB.Value;
                    case MathOpEnum.Multiply:
                        return valueState.TermA.Value * valueState.TermB.Value;
                    case MathOpEnum.Divide:
                        if (valueState.TermA.Value != 0 && valueState.TermB.Value != 0)
                        {
                            return valueState.TermA.Value / valueState.TermB.Value;
                        }
                        else
                        {
                            return 0;
                        }
                }
            }

            return 0;
        }

        private bool CheckMathOperatorNumA(char val, ref string curParse, ref MathValueState valueState)
        {
            bool isMathOperator = false;
            if (val == MATH_ADDITION)
            {
                valueState.Op = MathOpEnum.Add;
                isMathOperator = true;
            }
            else if (val == MATH_SUBTRACTION)
            {
                valueState.Op = MathOpEnum.Subtrack;
                isMathOperator = true;
            }
            else if (val == MATH_MULTIPLICATION)
            {
                valueState.Op = MathOpEnum.Multiply;
                isMathOperator = true;
            }
            else if (val == MATH_DIVISION)
            {
                valueState.Op = MathOpEnum.Divide;
                isMathOperator = true;
            }

            if (isMathOperator)
            {
                if (!valueState.TermA.HasValue)
                {
                    valueState.TermA = float.Parse(curParse);
                }
                valueState.State = MathParseStateEnum.PostOp;
            }

            return isMathOperator;
        }

        private bool CheckMathOperatorNumB(char val, ref string curParse, ref MathValueState valueState)
        {
            MathOpEnum newOp = MathOpEnum.None;

            if (val == MATH_ADDITION)
            {
                newOp = MathOpEnum.Add;
            }
            else if (val == MATH_SUBTRACTION)
            {
                newOp = MathOpEnum.Subtrack;
            }
            else if (val == MATH_MULTIPLICATION)
            {
                newOp = MathOpEnum.Multiply;
            }
            else if (val == MATH_DIVISION)
            {
                newOp = MathOpEnum.Divide;
            }

            if(newOp != MathOpEnum.None)
            {
                if (!valueState.TermB.HasValue)
                {
                    valueState.TermB = float.Parse(curParse);
                }
                valueState.TermA = CalculateFormula(ref valueState);
                valueState.TermB = null;
                valueState.Op = newOp;
                valueState.State = MathParseStateEnum.NumberB;
                return true;
            }

            return false;
        }

        public float Eval(string mathStr)
        {
            int parenLevel = 0;
            int curPos = 0;
            bool negativeVal = false;

            string curParse = "";

            StateStack.Clear();

            MathValueState valueState = new MathValueState();

            while (curPos < mathStr.Length)
            {
                switch (valueState.State)
                {
                    case MathParseStateEnum.Search:
                        /*
                        if (mathStr[curPos] == LEFT_PARENTHESIS)
                        {
                            parenLevel++;
                            StateStack.Add(valueState);
                            valueState = new MathValueState();
                        }
                        else if (mathStr[curPos] == RIGHT_PARENTHESIS)
                        {
                            parenLevel--;
                            if (parenLevel < 0)
                            {
                                //Error but continue
                            }
                            
                            MathValueState temp = StateStack.Last();

                            if(temp.Op != MathOpEnum.None)
                            {
                                valueState.TermA = CalculateFormula(ref valueState);
                                valueState.Op = MathOpEnum.None;

                                if(temp.State == MathParseStateEnum.)
                            }

                            // Process current value
                        }
                        */
                        if(Char.IsDigit(mathStr[curPos]))
                        {
                            valueState.State = MathParseStateEnum.NumberA;
                            curParse = ""+ mathStr[curPos];
                        }
                        else if(Char.IsLetter(mathStr[curPos]))
                        {

                        }
                        break;
                    case MathParseStateEnum.NumberA:
                        if (Char.IsDigit(mathStr[curPos]))
                        {
                            curParse += mathStr[curPos];
                        }
                        else if(mathStr[curPos] == DECIMAL_POINT)
                        {
                            valueState.State = MathParseStateEnum.NumAPostDec;
                            curParse += mathStr[curPos];
                        }
                        else if(CheckMathOperatorNumA(mathStr[curPos], ref curParse, ref valueState))
                        {
                            curParse = "";
                        }
                        else if (char.IsWhiteSpace(mathStr[curPos]))
                        {
                            valueState.State = MathParseStateEnum.PreOp;
                            valueState.TermA = float.Parse(curParse);
                            curParse = "";
                        }
                        break;
                    case MathParseStateEnum.NumAPostDec:
                        if (Char.IsDigit(mathStr[curPos]))
                        {
                            curParse += mathStr[curPos];
                        }
                        else if (mathStr[curPos] == DECIMAL_POINT)
                        {
                            //Error
                        }
                        else if (CheckMathOperatorNumA(mathStr[curPos], ref curParse, ref valueState))
                        {
                            curParse = "";
                        }
                        
                        break;
                    case MathParseStateEnum.PreOp:
                        if (CheckMathOperatorNumA(mathStr[curPos], ref curParse, ref valueState))
                        {
                            curParse = "";
                        }
                        break;
                    case MathParseStateEnum.PostOp:
                        if (Char.IsDigit(mathStr[curPos]))
                        {
                            valueState.State = MathParseStateEnum.NumberB;
                            curParse += mathStr[curPos];
                        }
                        break;
                    case MathParseStateEnum.NumberB:
                        if (Char.IsDigit(mathStr[curPos]))
                        {
                            curParse += mathStr[curPos];
                        }
                        else if (mathStr[curPos] == DECIMAL_POINT)
                        {
                            valueState.State = MathParseStateEnum.NumBPostDec;
                            curParse += mathStr[curPos];
                        }
                        else if (CheckMathOperatorNumB(mathStr[curPos], ref curParse, ref valueState))
                        {
                            curParse = "";
                        }
                        /*
                        else if(mathStr[curPos] ==  ' ')
                        {
                            valueState.TermA = CalculateFormula(ref valueState);
                            valueState.TermB = null;
                            valueState.Op = MathOpEnum.None;
                        }
                        */
                        break;
                    case MathParseStateEnum.NumBPostDec:
                        if (Char.IsDigit(mathStr[curPos]))
                        {
                            curParse += mathStr[curPos];
                        }
                        else if (mathStr[curPos] == DECIMAL_POINT)
                        {
                            valueState.State = MathParseStateEnum.NumAPostDec;
                            curParse = "" + mathStr[curPos];
                        }
                        else if (CheckMathOperatorNumB(mathStr[curPos], ref curParse, ref valueState))
                        {
                            curParse = "";
                        }
                        /*
                        else if (mathStr[curPos] == ' ')
                        {
                            valueState.TermA = CalculateFormula(ref valueState);
                            valueState.TermB = null;
                            valueState.Op = MathOpEnum.None;
                        }
                        */
                        break;
                }

                Console.Write("char '{0}'", mathStr[curPos]);
                curPos++;
            }

            if(valueState.State == MathParseStateEnum.NumberB || valueState.State == MathParseStateEnum.NumBPostDec)
            {
                if (valueState.TermA.HasValue && valueState.Op != MathOpEnum.None)
                {
                    valueState.TermB = float.Parse(curParse);
                    Console.WriteLine("Add TermA ({0}) and TermB ({1})", valueState.TermA, valueState.TermB);
                    valueState.TermA = CalculateFormula(ref valueState);
                    valueState.TermB = null;
                    valueState.Op = MathOpEnum.None;
                }
            }
            else
            {
                Console.WriteLine("No Op TermA ({0}) and TermB ({1})", valueState.TermA, valueState.TermB);
            }

            return valueState.TermA.Value;
        }
    }
}
