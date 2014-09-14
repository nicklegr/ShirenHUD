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
            mSnes.updateMemory();
            hpTextBox.Text = mSnes.U8(0x7E8604).ToString();

            string hint = "";

            int shirenX = mSnes.U8(0x7E85C8);
            int shirenY = mSnes.U8(0x7E85DC);
            hint += string.Format("シレン位置: {0}, {1}", shirenX, shirenY) + "\r\n";

            int itemAddr = 0x7E9EDF + (shirenY * 0x40) + shirenX;
            int itemIndex = mSnes.U8(itemAddr);
            hint += string.Format("足下アイテム: {0}", itemIndex) + "\r\n";

            // 商品を拾った場合は即更新、置いた場合は店主に話しかけた時に更新
            hint += string.Format("買値合計: {0}", mSnes.U16(0x7E8991)) + "\r\n";
            // 売値合計

            hint += "アイテムテーブル";
            for (int i = 0; i < 128; i++)
            {
                if (i % 3 == 0)
                    hint += "\r\n";

                int code = mSnes.U8(0x7E8B8C + i);
                string name = code != 0xFF ? Item.Names[code] : "";
                bool inStore = mSnes.U8(0x7E8E8C + i) != 0;
                hint += string.Format("{0:D3}: {1:X2} {2} {3} / ", i, code, name, inStore ? "(商)" : "");
            }

            hintBox.Text = hint;
        }

        Snes mSnes = new Snes();
    }
}
