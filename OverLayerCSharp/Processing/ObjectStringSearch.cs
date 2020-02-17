using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OverLayerCSharp.Processing
{
    public class ObjectStringSearch
    {
        private Object[] _arrayObject = null;
        private string _fieldName = "";
        private int _arrayStartPosition;
        private int _arrayEndPosition;
        private int _stringPosition;

        public ObjectStringSearch(Object[] arrayObject, string fieldName)
        {
            _arrayObject = arrayObject;
            _fieldName = fieldName;

            if(_arrayObject == null)
            {
                throw new ArgumentNullException("arrayObject");
            }
        }

        private string GetObjectString(int curPos)
        {
            Type myType = _arrayObject[curPos].GetType();

            FieldInfo fieldInfo = myType.GetField(_fieldName);

            if(fieldInfo.GetType() != typeof(String))
            {
                throw new ArgumentException("The fieldName needs to be a field ");
            }

            return (string) fieldInfo.GetValue(_arrayObject);
        }

        public void Reset()
        {
            _arrayStartPosition = 0;
            _arrayEndPosition = 0;
            _stringPosition = 0;
        }

        public void Start()
        {
            _arrayEndPosition = _arrayObject.Length - 1;
        }

        public int SearchChar(char curChar)
        {
            bool isStartPos = true;

            for(int i=_arrayStartPosition; i <=_arrayEndPosition; i++)
            {
                string curString = GetObjectString(i);

                if (isStartPos)
                {
                    if (_stringPosition < curString.Length && curString[_stringPosition] == curChar)
                    {
                        _arrayStartPosition++;
                    }
                }
                else
                {
                    if (_stringPosition >= curString.Length || curString[_stringPosition] != curChar)
                    {
                        _arrayEndPosition = i - 1;
                        break;
                    }
                }
            }

            return _arrayEndPosition - _arrayStartPosition;
        }

        public int End()
        {
            if (_arrayEndPosition== _arrayStartPosition && _arrayEndPosition < _arrayObject.Length)
            {
                string curString = GetObjectString(_arrayEndPosition);
                if(_stringPosition == curString.Length)
                {
                    return _arrayStartPosition;
                }
            }
            return -1;
        }
    }
}
