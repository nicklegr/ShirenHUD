using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

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
            public int BaseBuyingPrice { get; set; }
            public int BaseSellingPrice { get; set; }
            public int ModifyBuyingPrice { get; set; }
            public int ModifySellingPrice { get; set; }
        }

        static readonly Dictionary<int, Data> Names = new Dictionary<int, Data>();

        public static void LoadItem()
        {
            foreach (string line in File.ReadLines(@"Item.csv"))
            {
                var tokens = line.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (tokens.Count() == 0)
                    continue;

                if (tokens[0] == "//")
                    continue;

                int key = Convert.ToInt32(tokens[0], 16);

                var data = new Data()
                {
                    Type = (ItemType)Enum.Parse(typeof(ItemType), tokens[1]),
                    Name = tokens[2],
                    BaseBuyingPrice = Convert.ToInt32(tokens[3]),
                    BaseSellingPrice = Convert.ToInt32(tokens[4]),
                    ModifyBuyingPrice = Convert.ToInt32(tokens[5]),
                    ModifySellingPrice = Convert.ToInt32(tokens[6]),
                };

                Names.Add(key, data);
            }
        }

        public static Item FromTable(Snes snes, int index)
        {
            Debug.Assert(index < 0x80);
            int code = snes.U8(0x7E8B8C + index);

            var item = new Item()
            {
                Valid = code != 0xFF,
                Code = code,
                TableData = code != 0xFF ? Item.Names[code] : null,
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
                            item.PotSizeLeft = snes.U8(0x7E8C8C + index);

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

        // 自然に落ちてる可能性がある全アイテムの種類
        public static IEnumerable<Item> AllPossible()
        {
            foreach (var data in Names)
            {
                var item = new Item()
                {
                    Valid = true,
                    Code = data.Key,
                    TableData = data.Value,
                    Type = data.Value.Type,
                    Name = data.Value.Name,
                };

                switch (item.Type)
                {
                    case ItemType.Invalid:
                        break;
                    case ItemType.Sword:
                        for (int i = -1; i <= 3; i++)
                        {
                            var newItem = (Item)item.MemberwiseClone();
                            newItem.Attack = i;
                            yield return newItem;
                        }
                        break;
                    case ItemType.Arrow:
                        // @todo 必要？
                        break;
                    case ItemType.Shield:
                        for (int i = -1; i <= 3; i++)
                        {
                            var newItem = (Item)item.MemberwiseClone();
                            newItem.Defense = i;
                            yield return newItem;
                        }
                        break;
                    case ItemType.Grass:
                        yield return item;
                        break;
                    case ItemType.Scroll:
                        yield return item;
                        break;
                    case ItemType.Wand:
                        for (int i = 5; i <= 8; i++) // 8があり得るのは封印の杖のみらしい
                        {
                            var newItem = (Item)item.MemberwiseClone();
                            newItem.WandLife = i;
                            yield return newItem;
                        }
                        break;
                    case ItemType.Bracelet:
                        yield return item;
                        break;
                    case ItemType.RiceBall:
                        yield return item;
                        break;
                    case ItemType.Pot:
                        for (int i = 3; i <= 6; i++)
                        {
                            var newItem = (Item)item.MemberwiseClone();
                            newItem.PotSizeLeft = i;
                            yield return newItem;
                        }
                        break;
                    case ItemType.Flower:
                        break;
                    case ItemType.Meat:
                        // @todo
                        break;
                    case ItemType.Event:
                        break;
                    case ItemType.Gitan:
                        break;
                    case ItemType.Others:
                        break;
                    default:
                        break;
                }

            }
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
                        name = string.Format("{0}[{1}]", Name, PotSizeLeft);
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
                flags += "買 " + BuyingPrice + "G";
                flags += " ";
                flags += "売 " + SellingPrice + "G";
                flags += " ";
                flags += InStore ? "商" : "";

                return string.Format("{0} ({1})", name, flags);
            }
        }

        public int BuyingPrice
        {
            get
            {
                int modify = 0;

                switch (Type)
                {
                    case ItemType.Invalid:
                        break;
                    case ItemType.Sword:
                        modify = Attack;
                        break;
                    case ItemType.Arrow:
                        modify = ArrowCount;
                        break;
                    case ItemType.Shield:
                        modify = Defense;
                        break;
                    case ItemType.Grass:
                        break;
                    case ItemType.Scroll:
                        break;
                    case ItemType.Wand:
                        modify = WandLife;
                        break;
                    case ItemType.Bracelet:
                        break;
                    case ItemType.RiceBall:
                        break;
                    case ItemType.Pot:
                        modify = PotSizeLeft + Contents.Count;
                        break;
                    case ItemType.Flower:
                        break;
                    case ItemType.Meat:
                        break;
                    case ItemType.Event:
                        break;
                    case ItemType.Gitan:
                        break;
                    case ItemType.Others:
                        break;
                    default:
                        break;
                }

                return TableData.BaseBuyingPrice + TableData.ModifyBuyingPrice * modify;
            }
        }

        public int SellingPrice
        {
            get
            {
                int modify = 0;

                switch (Type)
                {
                    case ItemType.Invalid:
                        break;
                    case ItemType.Sword:
                        modify = Attack;
                        break;
                    case ItemType.Arrow:
                        modify = ArrowCount;
                        break;
                    case ItemType.Shield:
                        modify = Defense;
                        break;
                    case ItemType.Grass:
                        break;
                    case ItemType.Scroll:
                        break;
                    case ItemType.Wand:
                        modify = WandLife;
                        break;
                    case ItemType.Bracelet:
                        break;
                    case ItemType.RiceBall:
                        break;
                    case ItemType.Pot:
                        modify = PotSizeLeft + Contents.Count;
                        break;
                    case ItemType.Flower:
                        break;
                    case ItemType.Meat:
                        break;
                    case ItemType.Event:
                        break;
                    case ItemType.Gitan:
                        break;
                    case ItemType.Others:
                        break;
                    default:
                        break;
                }

                return TableData.BaseSellingPrice + TableData.ModifySellingPrice * modify;
            }
        }

        public bool Valid { get; set; }
        public int Code { get; set; }
        Data TableData { get; set; }
        public ItemType Type { get; set; }
        public string Name { get; set; }
        public int Attack { get; set; } // 剣の補正値
        public int Defense { get; set; } // 盾の補正値
        public int ArrowCount { get; set; } // 矢の本数
        public int WandLife { get; set; } // 杖の残り回数
        public int PotSizeLeft { get; set; } // 壺の残りサイズ
        public int GitanAmount { get; set; } // ギタンの金額
        public bool InStore { get; set; }
        public List<Item> Contents { get; set; }
    }
}
