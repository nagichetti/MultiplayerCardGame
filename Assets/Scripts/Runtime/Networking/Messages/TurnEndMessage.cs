using System;
using System.Collections.Generic;

namespace CardGame
{
    [Serializable]
    public class TurnEndMessage : NetworkMessage
    {
        public string playerId;
        public List<CardData> playedCards;
    }
}
