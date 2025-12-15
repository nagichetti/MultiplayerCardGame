using System;
using UnityEngine;

namespace CardGame
{
    [Serializable]
    public class RevealCardMessage : NetworkMessage
    {
        public string playerId;
        public int orderIndex;
    }
}
