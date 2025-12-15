using UnityEngine;

namespace CardGame
{
    [System.Serializable]
    public class CardData
    {
        public int id;
        public string name;
        public int cost;
        public int power;
        public AbilityData abilityData;
        public Sprite Sprite;
    }
    [System.Serializable]
    public class AbilityData
    {
        public string type;
        public int value;
    }
}
