using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class BoardCardHolder : MonoBehaviour
    {
        [SerializeField]
        private List<Card> m_placedCards;
        [SerializeField]
        private GameObject m_cardSlot;

        private void Start()
        {
            m_placedCards = new();
        }
        private void Awake()
        {
            CardEvents.OnAddCardToBorad += CardEvents_OnAddCardToBorad;
            CardEvents.OnRemoveCardFromBorad += CardEvents_OnRemoveCardFromBorad;
        }

        private void OnDestroy()
        {
            CardEvents.OnAddCardToBorad -= CardEvents_OnAddCardToBorad;
            CardEvents.OnRemoveCardFromBorad -= CardEvents_OnRemoveCardFromBorad;
        }
        private void CardEvents_OnAddCardToBorad(Card card)
        {
            if (!m_placedCards.Contains(card))
                m_placedCards.Add(card);

            CardEvents.RemoveCardFromHand(card);
        }

        private void CardEvents_OnRemoveCardFromBorad(Card card)
        {
            if (m_placedCards.Contains(card))
                m_placedCards.Remove(card);
           
            CardEvents.AddCardToHand(card);
        }
    }
}
