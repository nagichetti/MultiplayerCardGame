using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(fileName = "Deck Data", menuName ="Game/Deck Data")]
    public class DeckData : ScriptableObject
    {
        public int DeckSize;
        public int StartingHand;
        public List<CardData> Cards;
    }
}
