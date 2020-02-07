/**
 * <summary>
 * DebugHandler will hold debug variables that will control debugging.
 * License MIT 2020
 * </summary>
 * <author>Brian Atwell</author>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverLayerCSharp.DebugState
{
    public class DebugHandler
    {
        private static bool _isDebug = false;

        public static bool IsDebugging
        {
            get
            {
                return _isDebug;
            }
            set
            {
                _isDebug = value;
                if(value==false)
                {
                    _enableTransparency = true;
                }
            }
        }

        // Cannot enable transparency if IsDebug is false
        // The User will not be able to close the program or use their computer
        private static bool _enableTransparency = true;

        public static bool EnableTransparency
        {
            get
            {
                return _enableTransparency;
            }
            set
            {
                _enableTransparency = value;
                if (value == false)
                {
                    _isDebug = true;
                }
            }
        }

        static DebugHandler()
        {
#if DEBUG
            IsDebugging = true;

#endif
        }
    }
}
