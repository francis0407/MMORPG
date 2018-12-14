using Common;

namespace Backend.Network
{
    public partial class Incoming
    {
        IRegister register;
        public Incoming(IRegister register)
        {
            this.register = register;
            RegisterAll();
        }

        private void RegisterAll()
        {
            register.Register(Command.C_LOGIN, OnRecvLogin);
            register.Register(Command.C_REGISTER, OnRecvRegister);
            register.Register(Command.C_PLAYER_ENTER, OnRecvPlayerEnter);
            register.Register(Command.C_PLAYER_MOVE, OnRecvPlayerMove);
            register.Register(Command.C_PLAYER_JUMP, OnRecvPlayerJump);
            register.Register(Command.C_PLAYER_ATTACK, OnRecvPlayerAttack);
            register.Register(Command.C_PLAYER_TAKE, OnRecvPlayerTake);
            register.Register(Command.C_POSITION_REVISE, OnRecvPositionRevise);
            register.Register(Command.C_ENEMY_CLOSING, OnRecvEnemyClosing);
            register.Register(Command.C_DAMAGE, OnRecvDamage);
            register.Register(Command.C_CHAT_MESSAGE, OnRecvChatMessage);
            register.Register(Command.C_CREATE_ITEM, OnRecvPlayerTakeItem);
            register.Register(Command.C_PLAYER_EQUIP_ITEM, OnRecvPlayerEquipItem);
            register.Register(Command.C_PLAYER_UNEQUIP_ITEM, OnRecvPlayerUnEquipItem);
            register.Register(Command.C_PLAYER_DROP_ITEM, OnRecvPlayerDropItem);
            register.Register(Command.C_PLAYER_CHANGE_ITEM, OnRecvPlayerChangeItem);
            register.Register(Command.C_PLAYER_USE_ITEM, OnRecvPlayerUseItem);
            register.Register(Command.C_GET_TMALL_ITEMS, OnRecvGetTmallItems);
            register.Register(Command.C_BUY_TMALL_ITEMS, OnRecvBuyTmallItems);
            register.Register(Command.C_BUY_MARKET_ITEM, OnRecvBuyMarketItem);
            register.Register(Command.C_SELL_MARKET_ITEM, OnRecvSellMarketItem);
            register.Register(Command.C_GET_MARKET_ITEMS, OnRecvGetMarketItems);
            register.Register(Command.C_HEART_BEAT, OnRecvHeartBeat);
            register.Register(Command.C_PLAYER_EXIT, OnRecvPlayerExit);
            // DEBUG ..
            register.Register(Command.C_FIND_PATH, OnRecvFindPath);

        }


        static void ClientTipInfo(IChannel channel, string info)
        {
            STipInfo tipInfo = new STipInfo();
            tipInfo.info = info;
            channel.Send(tipInfo);
        }





    }
}
