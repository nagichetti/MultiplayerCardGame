using UnityEngine;

namespace CardGame
{
    public class ScoreUpdateMessage : NetworkMessage
    {
        public string playerId;
        public int score;
    }
}
