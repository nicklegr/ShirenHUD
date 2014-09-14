using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ShirenHUD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            var snes = new Snes();
            hpTextBox.Text = snes.U8(0x7E8604).ToString();
        }
    }
}
