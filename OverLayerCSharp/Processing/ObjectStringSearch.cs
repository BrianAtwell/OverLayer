/**
 * <summary>
 * This file handles state based searching an array of strings from the MathParser class.
 * License MIT 2020
 * </summary>
 * <author>Brian Atwell</author>
 */

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

        /**
         * <summary>Initialize the class</summary>
         * <param name="arrayObject">This is the class array that contains the string field.</param>
         * <param name="fieldName">This is the name of the field that will compare foreach object of the array.</param>
         */
        public ObjectStringSearch(Object[] arrayObject, string fieldName)
        {
            _arrayObject = arrayObject;
            _fieldName = fieldName;

            if(_arrayObject == null)
            {
                throw new ArgumentNullException("arrayObject");
            }
        }

        /**
         * <summary>Gets the string by fieldName of the _arrayObject</summary>
         * <param name="pos">position of the element of the _arrayObject that you want the string.</param>
         */
        private string GetObjectString(int pos)
        {
            Type myType = _arrayObject[pos].GetType();

            FieldInfo fieldInfo = myType.GetField(_fieldName);

            if(fieldInfo.FieldType is string)
            {
                throw new ArgumentException("The fieldName needs to be a field ");
            }

            return (string) fieldInfo.GetValue(_arrayObject[pos]);
        }

        /**
         * <summary>Resets the search state</summary>
         */
        public void Reset()
        {
            _arrayStartPosition = 0;
            _arrayEndPosition = 0;
            _stringPosition = 0;
        }

        /**
         * <summary>Initialize the search state</summary>
         */
        public void Start()
        {
            _arrayEndPosition = _arrayObject.Length - 1;
        }

        /**
         * <summary>Search the array with the curChar</summary>
         * <param name="curChar">The current character of the search string.</param>
         */
        public int SearchChar(char curChar)
        {
            bool isStartPos = true;

            for(int i=_arrayStartPosition; i <=_arrayEndPosition; i++)
            {
                string curString = GetObjectString(i);

                if (isStartPos)
                {
                    Console.WriteLine("Start String {0} char {1} char {2}", curString, curString[_stringPosition], curChar);
                    if (_stringPosition < curString.Length)
                    {
                        if (curString[_stringPosition] != curChar)
                        {
                            _arrayStartPosition++;
                        }
                        else
                        {

                            isStartPos = false;
                        }
                    }
                
                }
                else
                {
                    Console.WriteLine("End String {0} char {1} char {2}", curString, curString[_stringPosition], curChar);
                    if (_stringPosition >= curString.Length || curString[_stringPosition] != curChar)
                    {
                        _arrayEndPosition = i - 1;
                        break;
                    }
                }
            }

            _stringPosition++;

            return _arrayEndPosition - _arrayStartPosition;
        }

        /**
         * <summary>End the search, use current length to find the string. Return position.</summary>
         */
        public int End()
        {
            if (_arrayEndPosition == _arrayStartPosition && _arrayEndPosition < _arrayObject.Length
                && _arrayEndPosition >= 0
                && _stringPosition == (GetObjectString(_arrayEndPosition).Length - 1))
            {
                string curString = GetObjectString(_arrayEndPosition);
                if (_stringPosition == curString.Length)
                {
                    return _arrayStartPosition;
                }
            }
            else if(_arrayEndPosition < _arrayObject.Length && _arrayEndPosition >= 0)
            {
                for (int i = _arrayStartPosition; i <= _arrayEndPosition; i++)
                {
                    if(_stringPosition == GetObjectString(i).Length)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
    }
}
