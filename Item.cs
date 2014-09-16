using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ShirenHUD
{
    class Item
    {
        public enum ItemType
        {
            Invalid,
            Sword,
            Arrow,
            Shield,
            Grass,
            Scroll,
            Wand,
            Bracelet,
            RiceBall,
            Pot,
            Flower,
            Meat,
            Event,
            Gitan,
            Others,
        }

        class Data
        {
            public ItemType Type { get; set; }
            public string Name { get; set; }
        }

        static readonly Dictionary<int, Data> Names = new Dictionary<int, Data>()
        {
            // 武器
            { 0x00, new Data { Type = ItemType.Sword, Name = "こん棒" } },
            { 0x01, new Data { Type = ItemType.Sword, Name = "長巻" } },
            { 0x02, new Data { Type = ItemType.Sword, Name = "ブフーの包丁" } },
            { 0x03, new Data { Type = ItemType.Sword, Name = "カタナ" } },
            { 0x04, new Data { Type = ItemType.Sword, Name = "ドラゴンキラー" } },
            { 0x05, new Data { Type = ItemType.Sword, Name = "どうたぬき" } },
            { 0x06, new Data { Type = ItemType.Sword, Name = "剛剣マンジカブラ" } },
            { 0x07, new Data { Type = ItemType.Sword, Name = "成仏の鎌" } },
            { 0x08, new Data { Type = ItemType.Sword, Name = "つるはし" } },
            { 0x09, new Data { Type = ItemType.Sword, Name = "必中の剣" } },
            { 0x0A, new Data { Type = ItemType.Sword, Name = "ミノタウロスの斧" } },
            { 0x0B, new Data { Type = ItemType.Sword, Name = "妖刀かまいたち" } },
            { 0x0C, new Data { Type = ItemType.Sword, Name = "１ツ目殺し" } },
            { 0x0D, new Data { Type = ItemType.Sword, Name = "ドレインバスター" } },
            { 0x0E, new Data { Type = ItemType.Sword, Name = "火迅風魔刀" } },
            { 0x0F, new Data { Type = ItemType.Sword, Name = "秘剣カブラステギ" } },

            // 矢
            { 0x10, new Data { Type = ItemType.Arrow, Name = "木の矢" } },
            { 0x11, new Data { Type = ItemType.Arrow, Name = "鉄の矢" } },
            { 0x12, new Data { Type = ItemType.Arrow, Name = "銀の矢" } },
            { 0x13, new Data { Type = ItemType.Arrow, Name = "新規アイテム" } },
            { 0x14, new Data { Type = ItemType.Arrow, Name = "新規アイテム" } },
            { 0x15, new Data { Type = ItemType.Arrow, Name = "新規アイテム" } },

            // 盾
            { 0x16, new Data { Type = ItemType.Shield, Name = "皮甲の盾" } },
            { 0x17, new Data { Type = ItemType.Shield, Name = "青銅甲の盾" } },
            { 0x18, new Data { Type = ItemType.Shield, Name = "無どくの盾" } },
            { 0x19, new Data { Type = ItemType.Shield, Name = "木甲の盾" } },
            { 0x1A, new Data { Type = ItemType.Shield, Name = "鉄甲の盾" } },
            { 0x1B, new Data { Type = ItemType.Shield, Name = "ドラゴンシールド" } },
            { 0x1C, new Data { Type = ItemType.Shield, Name = "風魔の盾" } },
            { 0x1D, new Data { Type = ItemType.Shield, Name = "バトルカウンター" } },
            { 0x1E, new Data { Type = ItemType.Shield, Name = "重装の盾" } },
            { 0x1F, new Data { Type = ItemType.Shield, Name = "やまびこの盾" } },
            { 0x20, new Data { Type = ItemType.Shield, Name = "見切りの盾" } },
            { 0x21, new Data { Type = ItemType.Shield, Name = "見かけだおしの盾" } },
            { 0x22, new Data { Type = ItemType.Shield, Name = "使い捨ての盾" } },
            { 0x23, new Data { Type = ItemType.Shield, Name = "地雷ナバリの盾" } },
            { 0x24, new Data { Type = ItemType.Shield, Name = "トドの盾" } },
            { 0x25, new Data { Type = ItemType.Shield, Name = "ラセン風魔の盾" } },
            { 0x26, new Data { Type = ItemType.Shield, Name = "新規アイテム" } },
            { 0x27, new Data { Type = ItemType.Shield, Name = "新規アイテム" } },

            // 草
            { 0x28, new Data { Type = ItemType.Grass, Name = "薬草" } },
            { 0x29, new Data { Type = ItemType.Grass, Name = "弟切草" } },
            { 0x2A, new Data { Type = ItemType.Grass, Name = "しあわせ草" } },
            { 0x2B, new Data { Type = ItemType.Grass, Name = "めぐすり草" } },
            { 0x2C, new Data { Type = ItemType.Grass, Name = "ドラゴン草" } },
            { 0x2D, new Data { Type = ItemType.Grass, Name = "無敵草" } },
            { 0x2E, new Data { Type = ItemType.Grass, Name = "天使の種" } },
            { 0x2F, new Data { Type = ItemType.Grass, Name = "復活の草" } },
            { 0x30, new Data { Type = ItemType.Grass, Name = "消え去り草" } },
            { 0x31, new Data { Type = ItemType.Grass, Name = "くねくね草" } },
            { 0x32, new Data { Type = ItemType.Grass, Name = "不幸の種" } },
            { 0x33, new Data { Type = ItemType.Grass, Name = "超不幸の種" } },
            { 0x34, new Data { Type = ItemType.Grass, Name = "キグニ族の種" } },
            { 0x35, new Data { Type = ItemType.Grass, Name = "物忘れの草" } },
            { 0x36, new Data { Type = ItemType.Grass, Name = "－－－" } }, // 置いた所にモンスターが集まるとされる草(効果なし)
            { 0x37, new Data { Type = ItemType.Grass, Name = "命の草" } },
            { 0x38, new Data { Type = ItemType.Grass, Name = "胃拡張の種" } },
            { 0x39, new Data { Type = ItemType.Grass, Name = "胃縮小の種" } },
            { 0x3A, new Data { Type = ItemType.Grass, Name = "話の種" } },
            { 0x3B, new Data { Type = ItemType.Grass, Name = "ちからの草" } },
            { 0x3C, new Data { Type = ItemType.Grass, Name = "どく消し草" } },
            { 0x3D, new Data { Type = ItemType.Grass, Name = "どく草" } },
            { 0x3E, new Data { Type = ItemType.Grass, Name = "混乱草" } },
            { 0x3F, new Data { Type = ItemType.Grass, Name = "睡眠草" } },
            { 0x40, new Data { Type = ItemType.Grass, Name = "雑草" } },
            { 0x41, new Data { Type = ItemType.Grass, Name = "新規アイテム" } },
            { 0x42, new Data { Type = ItemType.Grass, Name = "新規アイテム" } },
            { 0x43, new Data { Type = ItemType.Grass, Name = "新規アイテム" } },
            { 0x44, new Data { Type = ItemType.Grass, Name = "新規アイテム" } },
            { 0x45, new Data { Type = ItemType.Grass, Name = "新規アイテム" } },
            { 0x46, new Data { Type = ItemType.Grass, Name = "新規アイテム" } },
            { 0x47, new Data { Type = ItemType.Grass, Name = "新規アイテム" } },
            { 0x48, new Data { Type = ItemType.Grass, Name = "新規アイテム" } },
            { 0x49, new Data { Type = ItemType.Grass, Name = "新規アイテム" } },
            { 0x4A, new Data { Type = ItemType.Grass, Name = "新規アイテム" } },
            { 0x4B, new Data { Type = ItemType.Grass, Name = "新規アイテム" } },
            { 0x4C, new Data { Type = ItemType.Grass, Name = "新規アイテム" } },
            { 0x4D, new Data { Type = ItemType.Grass, Name = "新規アイテム" } },
            { 0x4E, new Data { Type = ItemType.Grass, Name = "新規アイテム" } },
            { 0x4F, new Data { Type = ItemType.Grass, Name = "新規アイテム" } },
            { 0x50, new Data { Type = ItemType.Grass, Name = "新規アイテム" } },
            { 0x51, new Data { Type = ItemType.Grass, Name = "新規アイテム" } },
            { 0x52, new Data { Type = ItemType.Grass, Name = "新規アイテム" } },
            { 0x53, new Data { Type = ItemType.Grass, Name = "新規アイテム" } },
            { 0x54, new Data { Type = ItemType.Grass, Name = "新規アイテム" } },
            { 0x55, new Data { Type = ItemType.Grass, Name = "新規アイテム" } },

            // 巻物
            { 0x56, new Data { Type = ItemType.Scroll, Name = "おはらいの巻物" } },
            { 0x57, new Data { Type = ItemType.Scroll, Name = "識別の巻物" } },
            { 0x58, new Data { Type = ItemType.Scroll, Name = "あかりの巻物" } },
            { 0x59, new Data { Type = ItemType.Scroll, Name = "壺増大の巻物" } },
            { 0x5A, new Data { Type = ItemType.Scroll, Name = "真空斬りの巻物" } },
            { 0x5B, new Data { Type = ItemType.Scroll, Name = "くちなしの巻物" } },
            { 0x5C, new Data { Type = ItemType.Scroll, Name = "－－の巻物" } }, // 時の砂の巻物
            { 0x5D, new Data { Type = ItemType.Scroll, Name = "ワナの巻物" } },
            { 0x5E, new Data { Type = ItemType.Scroll, Name = "困った時の巻物" } },
            { 0x5F, new Data { Type = ItemType.Scroll, Name = "敵倍速の巻物" } },
            { 0x60, new Data { Type = ItemType.Scroll, Name = "バクスイの巻物" } },
            { 0x61, new Data { Type = ItemType.Scroll, Name = "パワーアップの巻物" } },
            { 0x62, new Data { Type = ItemType.Scroll, Name = "－－の巻物" } }, // 全滅の巻物(倉庫なしクリアしていれば効果あり)
            { 0x63, new Data { Type = ItemType.Scroll, Name = "自爆の巻物" } },
            { 0x64, new Data { Type = ItemType.Scroll, Name = "大部屋の巻物" } },
            { 0x65, new Data { Type = ItemType.Scroll, Name = "モンスターハウスの巻物" } },
            { 0x66, new Data { Type = ItemType.Scroll, Name = "混乱の巻物" } },
            { 0x67, new Data { Type = ItemType.Scroll, Name = "ジェノサイドの巻物" } },
            { 0x68, new Data { Type = ItemType.Scroll, Name = "白紙の巻物" } },
            { 0x69, new Data { Type = ItemType.Scroll, Name = "迷子の巻物" } },
            { 0x6A, new Data { Type = ItemType.Scroll, Name = "天の恵みの巻物" } },
            { 0x6B, new Data { Type = ItemType.Scroll, Name = "地の恵みの巻物" } },
            { 0x6C, new Data { Type = ItemType.Scroll, Name = "メッキの巻物" } },
            { 0x6D, new Data { Type = ItemType.Scroll, Name = "吸い出しの巻物" } },
            { 0x6E, new Data { Type = ItemType.Scroll, Name = "拾えずの巻物" } },
            { 0x6F, new Data { Type = ItemType.Scroll, Name = "－－の巻物" } }, // 時限爆弾の巻物(効果なし)
            { 0x70, new Data { Type = ItemType.Scroll, Name = "新規アイテム" } }, // 聖域の巻物
            { 0x71, new Data { Type = ItemType.Scroll, Name = "新規アイテム" } },
            { 0x72, new Data { Type = ItemType.Scroll, Name = "新規アイテム" } },
            { 0x73, new Data { Type = ItemType.Scroll, Name = "新規アイテム" } },
            { 0x74, new Data { Type = ItemType.Scroll, Name = "新規アイテム" } },
            { 0x75, new Data { Type = ItemType.Scroll, Name = "新規アイテム" } },
            { 0x76, new Data { Type = ItemType.Scroll, Name = "新規アイテム" } },
            { 0x77, new Data { Type = ItemType.Scroll, Name = "新規アイテム" } },
            { 0x78, new Data { Type = ItemType.Scroll, Name = "新規アイテム" } },
            { 0x79, new Data { Type = ItemType.Scroll, Name = "新規アイテム" } },
            { 0x7A, new Data { Type = ItemType.Scroll, Name = "新規アイテム" } },
            { 0x7B, new Data { Type = ItemType.Scroll, Name = "新規アイテム" } }, // パルプンテの巻物で、透明で読むことが出来ませんが、めぐすり草を飲むと見えるようになり、グラフィックが通常の巻物とは違います。

            // 杖
            { 0x7C, new Data { Type = ItemType.Wand, Name = "封印の杖" } },
            { 0x7D, new Data { Type = ItemType.Wand, Name = "ふきとばしの杖" } },
            { 0x7E, new Data { Type = ItemType.Wand, Name = "しあわせの杖" } },
            { 0x7F, new Data { Type = ItemType.Wand, Name = "不幸の杖" } },
            { 0x80, new Data { Type = ItemType.Wand, Name = "身がわりの杖" } },
            { 0x81, new Data { Type = ItemType.Wand, Name = "場所替えの杖" } },
            { 0x82, new Data { Type = ItemType.Wand, Name = "ブフーの杖" } },
            { 0x83, new Data { Type = ItemType.Wand, Name = "ガイコツまどうの杖" } },
            { 0x84, new Data { Type = ItemType.Wand, Name = "かなしばりの杖" } },
            { 0x85, new Data { Type = ItemType.Wand, Name = "一時しのぎの杖" } },
            { 0x86, new Data { Type = ItemType.Wand, Name = "痛み分けの杖" } },
            { 0x87, new Data { Type = ItemType.Wand, Name = "新規アイテム" } },
            { 0x88, new Data { Type = ItemType.Wand, Name = "新規アイテム" } },
            { 0x89, new Data { Type = ItemType.Wand, Name = "新規アイテム" } },
            { 0x8A, new Data { Type = ItemType.Wand, Name = "新規アイテム" } },
            { 0x8B, new Data { Type = ItemType.Wand, Name = "新規アイテム" } },
            { 0x8C, new Data { Type = ItemType.Wand, Name = "新規アイテム" } },
            { 0x8D, new Data { Type = ItemType.Wand, Name = "新規アイテム" } },
            { 0x8E, new Data { Type = ItemType.Wand, Name = "新規アイテム" } },
            { 0x8F, new Data { Type = ItemType.Wand, Name = "新規アイテム" } },
            { 0x90, new Data { Type = ItemType.Wand, Name = "新規アイテム" } },
            { 0x91, new Data { Type = ItemType.Wand, Name = "新規アイテム" } },
            { 0x92, new Data { Type = ItemType.Wand, Name = "新規アイテム" } },

            // 腕輪
            { 0x93, new Data { Type = ItemType.Bracelet, Name = "通過の腕輪" } },
            { 0x94, new Data { Type = ItemType.Bracelet, Name = "値切の腕輪" } },
            { 0x95, new Data { Type = ItemType.Bracelet, Name = "ワナ師の腕輪" } },
            { 0x96, new Data { Type = ItemType.Bracelet, Name = "レベル固定の腕輪" } },
            { 0x97, new Data { Type = ItemType.Bracelet, Name = "回復の腕輪" } },
            { 0x98, new Data { Type = ItemType.Bracelet, Name = "錆よけの腕輪" } },
            { 0x99, new Data { Type = ItemType.Bracelet, Name = "会心の腕輪" } },
            { 0x9A, new Data { Type = ItemType.Bracelet, Name = "痛恨の腕輪" } },
            { 0x9B, new Data { Type = ItemType.Bracelet, Name = "呪いよけの腕輪" } },
            { 0x9C, new Data { Type = ItemType.Bracelet, Name = "遠投の腕輪" } },
            { 0x9D, new Data { Type = ItemType.Bracelet, Name = "しあわせの腕輪" } },
            { 0x9E, new Data { Type = ItemType.Bracelet, Name = "垂れ流しの腕輪" } },
            { 0x9F, new Data { Type = ItemType.Bracelet, Name = "透視の腕輪" } },
            { 0xA0, new Data { Type = ItemType.Bracelet, Name = "混乱よけの腕輪" } },
            { 0xA1, new Data { Type = ItemType.Bracelet, Name = "識別の腕輪" } },
            { 0xA2, new Data { Type = ItemType.Bracelet, Name = "新規アイテム" } },
            { 0xA3, new Data { Type = ItemType.Bracelet, Name = "新規アイテム" } },
            { 0xA4, new Data { Type = ItemType.Bracelet, Name = "新規アイテム" } },
            { 0xA5, new Data { Type = ItemType.Bracelet, Name = "新規アイテム" } },
            { 0xA6, new Data { Type = ItemType.Bracelet, Name = "新規アイテム" } },
            { 0xA7, new Data { Type = ItemType.Bracelet, Name = "新規アイテム" } },
            { 0xA8, new Data { Type = ItemType.Bracelet, Name = "新規アイテム" } },
            { 0xA9, new Data { Type = ItemType.Bracelet, Name = "新規アイテム" } },
            { 0xAA, new Data { Type = ItemType.Bracelet, Name = "新規アイテム" } },
            { 0xAB, new Data { Type = ItemType.Bracelet, Name = "新規アイテム" } },
            { 0xAC, new Data { Type = ItemType.Bracelet, Name = "新規アイテム" } },
            { 0xAD, new Data { Type = ItemType.Bracelet, Name = "新規アイテム" } },

            // おにぎり
            { 0xAE, new Data { Type = ItemType.RiceBall, Name = "おにぎり" } },
            { 0xAF, new Data { Type = ItemType.RiceBall, Name = "大きいおにぎり" } },
            { 0xB0, new Data { Type = ItemType.RiceBall, Name = "くさったおにぎり" } },
            { 0xB1, new Data { Type = ItemType.RiceBall, Name = "巨大なおにぎり" } },
            { 0xB2, new Data { Type = ItemType.RiceBall, Name = "特製おにぎり" } },
            { 0xB3, new Data { Type = ItemType.RiceBall, Name = "新規アイテム" } },

            // 壺
            { 0xB4, new Data { Type = ItemType.Pot, Name = "保存の壺" } },
            { 0xB5, new Data { Type = ItemType.Pot, Name = "やりすごしの壺" } },
            { 0xB6, new Data { Type = ItemType.Pot, Name = "分裂の壺" } },
            { 0xB7, new Data { Type = ItemType.Pot, Name = "強化の壺" } },
            { 0xB8, new Data { Type = ItemType.Pot, Name = "識別の壺" } },
            { 0xB9, new Data { Type = ItemType.Pot, Name = "背中の壺" } },
            { 0xBA, new Data { Type = ItemType.Pot, Name = "倉庫の壺" } },
            { 0xBB, new Data { Type = ItemType.Pot, Name = "弱化の壺" } },
            { 0xBC, new Data { Type = ItemType.Pot, Name = "－－－" } }, // 手封じの壺(効果なし)
            { 0xBD, new Data { Type = ItemType.Pot, Name = "底抜けの壺" } },
            { 0xBE, new Data { Type = ItemType.Pot, Name = "魔物のるつぼ" } },
            { 0xBF, new Data { Type = ItemType.Pot, Name = "変化の壺" } },
            { 0xC0, new Data { Type = ItemType.Pot, Name = "合成の壺" } },
            { 0xC1, new Data { Type = ItemType.Pot, Name = "トドの壺" } },
            { 0xC2, new Data { Type = ItemType.Pot, Name = "ガイバラの壺" } },
            { 0xC3, new Data { Type = ItemType.Pot, Name = "アホくさい壺" } },
            { 0xC4, new Data { Type = ItemType.Pot, Name = "割れない壺" } },
            { 0xC5, new Data { Type = ItemType.Pot, Name = "うっぷんばらしの壺" } },

            // 花
            { 0xC6, new Data { Type = ItemType.Flower, Name = "新規アイテム" } },
            { 0xC7, new Data { Type = ItemType.Flower, Name = "新規アイテム" } },
            { 0xC8, new Data { Type = ItemType.Flower, Name = "新規アイテム" } },
            { 0xC9, new Data { Type = ItemType.Flower, Name = "新規アイテム" } },
            { 0xCA, new Data { Type = ItemType.Flower, Name = "新規アイテム" } },
            { 0xCB, new Data { Type = ItemType.Flower, Name = "新規アイテム" } },
            { 0xCC, new Data { Type = ItemType.Flower, Name = "新規アイテム" } },
            { 0xCD, new Data { Type = ItemType.Flower, Name = "新規アイテム" } },
            { 0xCE, new Data { Type = ItemType.Flower, Name = "新規アイテム" } },
            { 0xCF, new Data { Type = ItemType.Flower, Name = "新規アイテム" } },
            { 0xD0, new Data { Type = ItemType.Flower, Name = "新規アイテム" } },
            { 0xD1, new Data { Type = ItemType.Flower, Name = "新規アイテム" } },
            { 0xD2, new Data { Type = ItemType.Flower, Name = "新規アイテム" } },
            { 0xD3, new Data { Type = ItemType.Flower, Name = "新規アイテム" } },
            { 0xD4, new Data { Type = ItemType.Flower, Name = "新規アイテム" } },
            { 0xD5, new Data { Type = ItemType.Flower, Name = "新規アイテム" } },
            { 0xD6, new Data { Type = ItemType.Flower, Name = "新規アイテム" } },
            { 0xD7, new Data { Type = ItemType.Flower, Name = "新規アイテム" } },
            { 0xD8, new Data { Type = ItemType.Flower, Name = "新規アイテム" } },
            { 0xD9, new Data { Type = ItemType.Flower, Name = "新規アイテム" } },
            { 0xDA, new Data { Type = ItemType.Flower, Name = "新規アイテム" } },
            { 0xDB, new Data { Type = ItemType.Flower, Name = "新規アイテム" } },
            { 0xDC, new Data { Type = ItemType.Flower, Name = "新規アイテム" } },
            { 0xDD, new Data { Type = ItemType.Flower, Name = "新規アイテム" } },
            { 0xDE, new Data { Type = ItemType.Flower, Name = "新規アイテム" } },
            { 0xDF, new Data { Type = ItemType.Flower, Name = "新規アイテム" } },

            // 肉
            { 0xE0, new Data { Type = ItemType.Meat, Name = "モンスターの肉" } },

            // 目的物
            // グラフィックはコンドルの羽根で、拾うとテーブルマウンテンをクリアしたことになります。
            { 0xE1, new Data { Type = ItemType.Event, Name = "黄金の羽根" } },
            { 0xE2, new Data { Type = ItemType.Event, Name = "しあわせの箱" } },
            { 0xE3, new Data { Type = ItemType.Event, Name = "奇妙な箱" } },
            { 0xE4, new Data { Type = ItemType.Event, Name = "新規アイテム目的物" } },

            // その他
            { 0xE5, new Data { Type = ItemType.Gitan, Name = "ギタン" } },
            { 0xE6, new Data { Type = ItemType.Others, Name = "　　　　　　　　　　" } }, // 草で、未識別の状態では飲めますが、識別すると透明で飲めなくなり、めぐすり草を飲んでも見えるようにはなりません。 
            { 0xE7, new Data { Type = ItemType.Others, Name = "ンドゥバ" } },
        };

        public static Item FromTable(Snes snes, int index)
        {
            Debug.Assert(index < 0x80);
            int code = snes.U8(0x7E8B8C + index);

            var item = new Item()
            {
                Valid = code != 0xFF,
                Code = code,
                Type = code != 0xFF ? Item.Names[code].Type : ItemType.Invalid,
                Name = code != 0xFF ? Item.Names[code].Name : "",
                InStore = snes.U8(0x7E8E8C + index) != 0,
            };

            if (item.Valid)
            {
                switch (item.Type)
                {
                    case ItemType.Invalid:
                        break;
                    case ItemType.Sword:
                        item.Attack = snes.U8(0x7E8C8C + index);
                        break;
                    case ItemType.Arrow:
                        item.ArrowCount = snes.U8(0x7E8C8C + index);
                        break;
                    case ItemType.Shield:
                        item.Defense = snes.U8(0x7E8C8C + index);
                        break;
                    case ItemType.Grass:
                        break;
                    case ItemType.Scroll:
                        break;
                    case ItemType.Wand:
                        item.WandLife = snes.U8(0x7E8C8C + index);
                        break;
                    case ItemType.Bracelet:
                        break;
                    case ItemType.RiceBall:
                        break;
                    case ItemType.Pot:
                        {
                            item.PotSize = snes.U8(0x7E8C8C + index);

                            // 壺の中のアイテムリスト
                            var nextIndex = index;
                            while (true)
                            {
                                nextIndex = snes.U8(0x7E8E0C + nextIndex);
                                if (nextIndex == 0xFF)
                                    break;

                                item.Contents.Add(Item.FromTable(snes, nextIndex));
                            }
                        }
                        break;
                    case ItemType.Flower:
                        break;
                    case ItemType.Meat:
                        break;
                    case ItemType.Event:
                        break;
                    case ItemType.Gitan:
                        item.GitanAmount = snes.U8(0x7E8D0C + index) << 8 | snes.U8(0x7E8C8C + index);
                        break;
                    case ItemType.Others:
                        break;
                    default:
                        break;
                }
            }

            return item;
        }

        public static Item FromShiren(Snes snes, int index)
        {
            Debug.Assert(index < 20);

            var tableIndex = snes.U8(0x7E894F + index);
            if (tableIndex == 0xFF)
                return Item.Invalid();

            return Item.FromTable(snes, tableIndex);
        }

        // 足下のアイテム
        public static Item FromGround(Snes snes)
        {
            int shirenX = snes.U8(0x7E85C8);
            int shirenY = snes.U8(0x7E85DC);

            int itemAddr = 0x7E9EDF + (shirenY * 0x40) + shirenX;
            int itemIndex = snes.U8(itemAddr);

            if (itemIndex < 0x80)
                return Item.FromTable(snes, itemIndex);
            else
                return Item.Invalid();
        }

        public static Item Invalid()
        {
            return new Item() { Valid = false };
        }

        public Item()
        {
            Contents = new List<Item>();
        }

        public string DisplayName
        {
            get
            {
                if (!Valid)
                    return "";

                string name = Name;
                switch (Type)
                {
                    case ItemType.Invalid:
                        break;
                    case ItemType.Sword:
                        if(Attack != 0)
                            name += Attack.ToString("+#;-#");
                        break;
                    case ItemType.Arrow:
                        name = string.Format("{0}本の{1}", ArrowCount, Name);
                        break;
                    case ItemType.Shield:
                        if(Defense != 0)
                            name += Defense.ToString("+#;-#");
                        break;
                    case ItemType.Grass:
                        break;
                    case ItemType.Scroll:
                        break;
                    case ItemType.Wand:
                        name = string.Format("{0}[{1}]", Name, WandLife);
                        break;
                    case ItemType.Bracelet:
                        break;
                    case ItemType.RiceBall:
                        break;
                    case ItemType.Pot:
                        name = string.Format("{0}[{1}]", Name, PotSize);
                        break;
                    case ItemType.Flower:
                        break;
                    case ItemType.Meat:
                        break;
                    case ItemType.Event:
                        break;
                    case ItemType.Gitan:
                        name = string.Format("{0}{1}", GitanAmount, Name);
                        break;
                    case ItemType.Others:
                        break;
                    default:
                        break;
                }

                string flags = "";
                flags += InStore ? "商" : "";

                return string.Format("{0} ({1})", name, flags);
            }
        }

        public bool Valid { get; set; }
        public int Code { get; set; }
        public ItemType Type { get; set; }
        public string Name { get; set; }
        public int Attack { get; set; } // 剣の補正値
        public int Defense { get; set; } // 盾の補正値
        public int ArrowCount { get; set; } // 矢の本数
        public int WandLife { get; set; } // 杖の残り回数
        public int PotSize { get; set; } // 壺のサイズ
        public int GitanAmount { get; set; } // ギタンの金額
        public bool InStore { get; set; }
        public List<Item> Contents { get; set; }
    }
}
