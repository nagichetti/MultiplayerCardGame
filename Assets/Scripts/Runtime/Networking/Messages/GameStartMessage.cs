using UnityEngine;

namespace CardGame
{
    [System.Serializable]
    public class GameStartMessage : NetworkMessage
    {
        public string[] playerIds;
        public int totalTurns;
    }
}
