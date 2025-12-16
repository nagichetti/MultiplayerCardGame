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
        [SerializeField]
        private Sprite m_faceUpImage;
        [SerializeField]
        private Sprite m_faceDownImage;

        [Header("UI Text")]
        [SerializeField]
        GameObject m_uiParent;
        [SerializeField]
        private TextMeshProUGUI costText;
        [SerializeField]
        private TextMeshProUGUI powerText;
        [SerializeField]
        private TextMeshProUGUI ability;
        [SerializeField]
        private TextMeshProUGUI cardName;

        private bool isPlaced;
        public CardVisual CardVisual => m_cardVisual;
        public CardData CardData { get => m_cardData; set => m_cardData = value; }
        public bool IsPlaced
        {
            get => isPlaced; set
            {
                isPlaced = value;
                HandlePlacement();
            }
        }
        private void Awake()
        {
            Lock();
        }
        private void OnDestroy()
        {
        }

        private void HandlePlacement()
        {
            if (isPlaced)
            {
                CardEvents.AddCardToBoard(this);
            }
            else
            {
                CardEvents.RemoveCardFromBoard(this);
            }
        }


        public void Init()
        {
            costText.text = CardData.cost.ToString();
            powerText.text = CardData.power.ToString();
            ability.text = CardData.abilityData.type.ToString();
            cardName.text = CardData.name;
            FaceUp();
        }
        public void Fold()
        {
            m_cardVisual.IsSelected = false;
            m_cardVisual.CanSelect = false;
            AttachTo();
        }
        void AttachTo()
        {
            var parent = GameObject.FindGameObjectWithTag("Board").GetComponent<RectTransform>();
            transform.parent.SetParent(parent, false);
            transform.eulerAngles = Vector3.zero;
        }
        public void Lock()
        {
            if (m_cardVisual.IsSelected) return;
            m_cardVisual.CanSelect = false;
        }
        public void UnLock()
        {
            m_cardVisual.CanSelect = true;
        }

        public void UpdateCard(int remainingCost)
        {
            if (CardData.cost > remainingCost)
                Lock();
            else
                UnLock();
        }

        public void FaceDown()
        {
            m_cardVisual.SetVisual(m_faceDownImage);
            m_uiParent.SetActive(false);
        }
        public void FaceUp()
        {
            m_cardVisual.SetVisual(m_faceUpImage);
            m_uiParent.SetActive(true);
        }
    }
}
