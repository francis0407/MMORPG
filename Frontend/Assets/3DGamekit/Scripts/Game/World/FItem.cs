using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
namespace FrontEnd.Item
{
    public class FItem
    {
        public int item_id;
        public ItemType item_type;
        public string name;
        public string icon_name;

        public int health_value;
        public int speed_value;
        public int damage_value;
        public int intelligence_value;
        public int defence_value;

        public int silver_value;
        private static Dictionary<ItemType, string[]> namePool = new Dictionary<ItemType, string[]>
        {
            {ItemType.Helmet, new string[]{"Helmet_1", "Helmet_2", "Helmet_3", "Helmet_4"} },
            {ItemType.Armour, new string[]{"Armor_1", "Armor_2", "Armor_3"} },
            {ItemType.Legging, new string[]{"Legging_1", "Legging_2", "Legging_3"} },
            {ItemType.Shoes, new string[]{"Shoes_1", "Shoes_2", "Shoes_3", "Shoes_4", "Shoes_5"} },
            {ItemType.Leftweapon, new string[]{"Flail", "Shield"} },
            {ItemType.Rightweapon, new string[]{"Sword_1", "Sword_2", "Ax_1", "Ax_2", "Ax_3"} }
        };

        public static List<ItemConf> itemConfs;

        //private static Dictionary<ItemType>
        public DItem ToDItem()
        {
            DItem dItem = new DItem();
            dItem.item_id = item_id;
            dItem.item_type = item_type;
            dItem.name = name;
            dItem.icon_name = icon_name;
            dItem.health_value = health_value;
            dItem.speed_value = speed_value;
            dItem.damage_value = damage_value;
            dItem.intelligence_value = intelligence_value;
            dItem.defence_value = defence_value;
            dItem.silver_value = silver_value;
            return dItem;
        }

        static public FItem FromDItem(DItem item)
        {
            FItem fItem = new FItem();
            fItem.item_id = item.item_id;
            fItem.item_type = item.item_type;
            fItem.name = item.name;
            fItem.icon_name = item.icon_name;

            fItem.health_value = item.health_value;
            fItem.speed_value = item.speed_value;
            fItem.damage_value = item.damage_value;
            fItem.intelligence_value = item.intelligence_value;
            fItem.defence_value = item.defence_value;
            fItem.silver_value = item.silver_value;

            return fItem;
        }
        static public FItem CreateRandomItem(int luck)
        {
            Random rand = new Random();
            if (itemConfs == null)
            {
                // ???
                FItem newItem = new FItem();
                newItem.item_type = (ItemType)rand.Next(1, 7);

                var names = namePool[newItem.item_type];
                newItem.name = names[rand.Next(0, names.Length - 1)];
                newItem.icon_name = newItem.name;

                newItem.health_value = luck;
                newItem.speed_value = luck;
                newItem.damage_value = luck;
                newItem.intelligence_value = luck;
                newItem.defence_value = luck;
                newItem.silver_value = 1 + luck * 5;
                return newItem;
            }
            else
            {
                var itemConf = itemConfs[rand.Next(0, itemConfs.Count - 1)];
                return CreateSpecificItem(itemConf.name, itemConf.icon, itemConf.type, luck);  
            }
        }

        static public FItem CreateSpecificItem(string name, string icon, ItemType type, int luck)
        {
            FItem newItem = new FItem();
            Random rand = new Random();

            newItem.name = name;
            newItem.icon_name = icon;
            newItem.item_type = type;
            if (newItem.name == "HealthElixir")
            {
                newItem.health_value = 0;
                newItem.speed_value = 0;
                newItem.damage_value = 0;
                newItem.intelligence_value = 0;
                newItem.defence_value = 0;
                newItem.silver_value = 10;
            }
            else
            {
                newItem.health_value = System.Math.Max(1, luck + (rand.Next(0, 11) - 5));
                newItem.speed_value = System.Math.Max(1, luck + (rand.Next(0, 11) - 5));
                newItem.damage_value = System.Math.Max(1, luck + (rand.Next(0, 11) - 5));
                newItem.intelligence_value = System.Math.Max(1, luck + (rand.Next(0, 11) - 5));
                newItem.defence_value = System.Math.Max(1, luck + (rand.Next(0, 11) - 5));
                newItem.silver_value = newItem.health_value + newItem.speed_value + newItem.damage_value + newItem.intelligence_value + newItem.defence_value;
            }
            return newItem;
        }
    }

    
}
