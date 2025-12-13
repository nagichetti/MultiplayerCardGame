using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class HorizontalCardHolder : MonoBehaviour
    {
        [SerializeField]
        private List<Card> cards = new();
        [SerializeField]
        private GameObject m_cardSlot;
        [SerializeField]
        private Card m_card;

        private Card selectedCard;

        private void Start()
        {
            for (int i = 0; i < 6; i++)
            {
                AddCard(m_card);
            }
        }

        public void AddCard(Card spawncard)
        {
            var cardSlot = Instantiate(m_cardSlot, transform);
            Card card = Instantiate(spawncard, cardSlot.transform);
            card.BeginDragEvent.AddListener(BeginDrag);
            card.EndDragEvent.AddListener(DragEnded);
            cards.Add(card);
        }

        private void BeginDrag(Card card)
        {
            if (selectedCard == null)
                selectedCard = card;
        }

        private void DragEnded(Card card)
        {
            if (selectedCard == null) return;

            selectedCard.transform.localPosition = new Vector3(0, 0, selectedCard.transform.localPosition.z);
            selectedCard = null;
        }
    }
}
