﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OverLayerCSharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        protected override void DefWndProc(ref Message m)
        {
            /*
            switch(m.Msg)
            {
                case 
            };
            */
            base.DefWndProc(ref m);
        }
    }
}
