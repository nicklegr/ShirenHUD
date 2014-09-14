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
            hint += string.Format("買値合計: {0}", mSnes.U32(0x7E8991)) + "\r\n";

            // 売値合計
            // 他の変数と共用らしく、店主に話しかけてる間だけ正しい値
            hint += string.Format("売値合計: {0}", mSnes.U32(0x7E1FDB)) + "\r\n";

            // 店内フラグ
            // 検証甘め。中断時やセーブデータ選択画面では1になることがある。冒険中は今のところ合ってる
            hint += string.Format("店内フラグ: {0} {1}", mSnes.U8(0x7E0CA4), mSnes.U8(0x7E0CB4)) + "\r\n";

            hint += "所持アイテム:\r\n";
            for (int i = 0; i < 20; i++)
            {
                if (i == 10)
                    hint += "\r\n";

                var item = Item.FromShiren(mSnes, i);
                hint += string.Format("{0:D2}: {1}\r\n", i, item.DisplayName);
            }

            var groundItem = Item.FromGround(mSnes);
            if (groundItem.Valid && groundItem.Contents.Count != 0)
            {
                hint += "足下の壺の中身:\r\n";
                foreach (var item in groundItem.Contents)
                {
                    hint += string.Format("  {0}\r\n", item.DisplayName);
                }
            }

            hint += "アイテムテーブル:";
            for (int i = 0; i < 128; i++)
            {
                if (i % 3 == 0)
                    hint += "\r\n";

                var item = Item.FromTable(mSnes, i);
                hint += string.Format("{0:D3}: {1} / ", i, item.DisplayName);
            }

            hintBox.Text = hint;
        }

        Snes mSnes = new Snes();
    }
}
