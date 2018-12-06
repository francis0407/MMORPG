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

        private static Dictionary<ItemType, string[]> namePool = new Dictionary<ItemType, string[]>
        {
            {ItemType.Helmet, new string[]{"Helmet_1", "Helmet_2", "Helmet_3", "Helmet_4"} },
            {ItemType.Armour, new string[]{"Armor_1", "Armor_2", "Armor_3"} },
            {ItemType.Legging, new string[]{"Legging_1", "Legging_2", "Legging_3"} },
            {ItemType.Shoes, new string[]{"Shoes_1", "Shoes_2", "Shoes_3", "Shoes_4", "Shoes_5"} },
            {ItemType.Leftweapon, new string[]{"Flail", "Shield"} },
            {ItemType.Rightweapon, new string[]{"Sword_1", "Sword_2", "Ax_1", "Ax_2", "Ax_3"} }
        };

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

            return fItem;
        }
        static public FItem CreateRandomItem(int luck)
        {
            Random rand = new Random();
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
            
            return newItem;
        }
    }

    
}
