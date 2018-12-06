/*
 * message code define
 * if you are changing this file, mind to rebuild both Frontend and Backend later
 */

namespace Common
{
    public enum Command
    {
        NONE,
        SBEGIN = 0,
        S_PLAYER_REGISTER,
        S_PLAYER_ENTER,
        S_ITEM_SPAWN,
        S_SPAWN,
        S_PLAYER_RESPAWN,
        S_PLAYER_ACTION,
        S_ENTITY_DESTROY,
        S_PLAYER_MOVE,
        S_SPRITE_MOVE,
        S_JUMP,
        S_ATTACK,
        S_EQUIP_WEAPON,
        S_HIT,
        S_TAKE_ITEM,
        S_EXIT,
        S_SPRITE_DIE,
        S_PLAYER_DIE,
        S_TIP_INFO,
        S_OTHER_PLAYER_ENTER,
        S_OTHER_PLAYER_EXIT,
        S_ONLINE_PLAYERS,
        S_CHAT_MESSAGE,
        S_GET_ITEM,
        S_PLAYER_INVENTORY,
        S_PLAYER_EQUIP_ITEM,
        S_PLAYER_UNEQUIP_ITEM,
        S_PLAYER_DROP_ITEM,
        S_PLAYER_CHANGE_ITEM,
        SEND,

        CBEGIN,
        C_LOGIN,
        C_REGISTER,
        C_PLAYER_ENTER,
        C_PLAYER_ATTACK,
        C_PLAYER_JUMP,
        C_PLAYER_MOVE,
        C_PLAYER_TAKE,
        C_POSITION_REVISE,
        C_ENEMY_CLOSING,
        C_DAMAGE,
        C_CHAT_MESSAGE,
        C_CREATE_ITEM,
        C_PLAYER_EQUIP_ITEM,
        C_PLAYER_UNEQUIP_ITEM,
        C_PLAYER_DROP_ITEM,
        C_PLAYER_CHANGE_ITEM,
        CEND,

        DEBUGGING, // THE FOLLOWING MESSEGES ARE FOR DEBUGGING
        C_FIND_PATH,
        S_FIND_PATH,

        CMD_END, // DO NOT GREATER THAN UINT_MAX !!!
    }
}
