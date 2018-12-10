using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
namespace FrontEnd.Item
{
    public enum ItemStatus
    {
        Using,
        Storing,
        Selling,
        Drop
    }
    [Serializable]
    public class DItem
    {
        public int item_id;
        public ItemType item_type;
        public string name;
        public string icon_name;
        public ItemStatus status;
        public int health_value;
        public int speed_value;
        public int damage_value;
        public int intelligence_value;
        public int defence_value;
        public int silver_value;
    }
}
