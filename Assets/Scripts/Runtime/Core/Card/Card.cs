using TMPro;
using UnityEngine;

namespace CardGame
{
    public class Card : MonoBehaviour
    {

        [SerializeField]
        private CardData m_cardData;
        [SerializeField]
        private CardVisual m_cardVisual;

        [Header("UI Text")]
        [SerializeField]
        private TextMeshProUGUI costText;
        [SerializeField]
        private TextMeshProUGUI powerText;
        [SerializeField]
        private TextMeshProUGUI ability;

        private bool isPlaced;
        public CardVisual CardVisual => m_cardVisual;
        public CardData CardData { get => m_cardData; set => m_cardData = value; }
        public bool IsPlaced
        {
            get => isPlaced; set
            {
                isPlaced = value;
                if (isPlaced)
                    CardEvents.AddCardToBoard(this);
                else
                    CardEvents.RemoveCardFromBoard(this);
            }
        }
        private void Awake()
        {
            Lock();
        }
        public void Init()
        {
            costText.text = "Cost: " + CardData.cost.ToString();
            powerText.text = "Power: " + CardData.power.ToString();
            ability.text = "Ability: " + CardData.abilityData.type;
        }

        public void Lock()
        {
            m_cardVisual.CanDrag = false;
        }
        public void UnLock()
        {
            m_cardVisual.CanDrag = true;
        }
    }
}
