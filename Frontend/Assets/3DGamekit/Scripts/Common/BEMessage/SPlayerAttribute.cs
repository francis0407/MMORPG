using System;

namespace Common
{
    [Serializable]
    public class SPlayerAttribute : Message
    {
        public SPlayerAttribute() : base(Command.S_PLAYER_ATTRIBUTE) { }
        public int health;
        public int damage;
        public int intelligence;
        public int defence;
        public int speed;

        public int hp;

        public int gold;
        public int silver;


    }
}
