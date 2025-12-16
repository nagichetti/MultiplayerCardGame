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
            CardEvents.OnRevealCard += CardEvents_OnRevealCard;
            CardEvents.OnRevealEnd += RemoveCards;
            CardEvents.OnClearPlayedCards += CardEvents_OnClearPlayedCards;
        }

        private void OnDestroy()
        {
            CardEvents.OnAddCardToBorad -= CardEvents_OnAddCardToBorad;
            CardEvents.OnRemoveCardFromBorad -= CardEvents_OnRemoveCardFromBorad;
            CardEvents.OnRevealCard -= CardEvents_OnRevealCard;
            CardEvents.OnRevealEnd -= RemoveCards;
            CardEvents.OnClearPlayedCards -= CardEvents_OnClearPlayedCards;
        }
        private void CardEvents_OnClearPlayedCards(PlayerSlot oppPlayer)
        {
            if (LocalPlayerContext.MySlot == oppPlayer)
                RemoveCards();
        }
        private void CardEvents_OnRevealCard(string playerId, CardData obj)
        {
            if (LocalPlayerContext.MySlot.ToString() != playerId) return;
            foreach (var item in m_placedCards)
            {
                if (item.CardData.id == obj.id)
                    item.FaceUp();
            }
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
        private void RemoveCards()
        {
            if (m_placedCards.Count > 0)
                m_placedCards.ForEach(x => Destroy(x.transform.parent.gameObject));
            m_placedCards.Clear();
        }
    }
}
