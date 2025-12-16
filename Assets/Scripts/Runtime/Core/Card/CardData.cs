using UnityEngine;

namespace CardGame
{
    [System.Serializable]
    public class CardData
    {
        public string name;
        public int id;
        public int cost;
        public int power;
        public AbilityData abilityData;

        public CardData Clone()
        {
            return new CardData
            {
                id = this.id,
                name = this.name,
                cost = this.cost,
                power = this.power,
                abilityData = this.abilityData,
            };
        }
    }
    [System.Serializable]
    public class AbilityData
    {
        public Ability type;
        public int value;
    }
}
