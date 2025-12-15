using UnityEngine;

namespace CardGame
{
    public class PlayerDisconnectedMessage : NetworkMessage
    {
        public string playerSlot;
    }
}
