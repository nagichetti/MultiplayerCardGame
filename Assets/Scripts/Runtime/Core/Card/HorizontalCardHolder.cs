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

        private CardVisual selectedCard;

        private void Awake()
        {
            CardEvents.OnSpawnCardToHand += SpawnCard;
            CardEvents.OnAddCardToHand += AddCard;
            CardEvents.OnRemoveCardFromHand += RemoveCard;
        }

        private void OnDestroy()
        {
            CardEvents.OnSpawnCardToHand -= SpawnCard;
            CardEvents.OnAddCardToHand -= AddCard;
            CardEvents.OnRemoveCardFromHand -= RemoveCard;
        }

        public void SpawnCard(Card spawncard)
        {
            var cardSlot = Instantiate(m_cardSlot, transform);
            Card card = Instantiate(spawncard, cardSlot.transform);
            card.CardVisual.BeginDragEvent.AddListener(BeginDrag);
            card.CardVisual.EndDragEvent.AddListener(DragEnded);
            card.Init();
            CardEvents.AddCardToHand(card);

        }
        private void AddCard(Card card)
        {
            if (!cards.Contains(card))
                cards.Add(card);
        }
        private void RemoveCard(Card card)
        {
            if (cards.Contains(card))
                cards.Remove(card);
        }
        private void BeginDrag(CardVisual card)
        {
            if (selectedCard == null)
                selectedCard = card;
        }

        private void DragEnded(CardVisual card)
        {
            if (selectedCard == null) return;

            selectedCard.transform.localPosition = new Vector3(0, 0, selectedCard.transform.localPosition.z);
            selectedCard = null;
        }
    }
}
