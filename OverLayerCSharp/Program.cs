/**
 * <summary>
 * Main Program that initializes, registers, creates, and messages for OverLay window.
 * License MIT 2020
 * </summary>
 * <author>Brian Atwell</author>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OverLayerCSharp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            OverLayWindow Window = new OverLayWindow();
            Window.RegisterWindow();
            Window.CreateWindow();
            OverLayWindow.MessageLoop();
        }
    }
}
