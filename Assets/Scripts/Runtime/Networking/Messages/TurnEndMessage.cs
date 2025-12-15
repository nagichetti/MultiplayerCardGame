using UnityEngine;

namespace CardGame
{
    public class TurnEndMessage : NetworkMessage
    {
        public string playerId;
        public int playedCards;
    }
}
