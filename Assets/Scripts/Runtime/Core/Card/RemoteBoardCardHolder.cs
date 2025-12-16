using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class RemoteBoardCardHolder : MonoBehaviour
    {
        [SerializeField]
        private List<RemoteCard> m_remotePlayedCards = new();

        [SerializeField]
        private RemoteCard m_card;
        private void Awake()
        {
            GameEvents.OnTurnEnd += GameEvents_OnTurnEnd;
            CardEvents.OnRevealCard += CardEvents_OnRevealCard;
            CardEvents.OnRevealEnd += RemoveCards;
            CardEvents.OnClearPlayedCards += CardEvents_OnClearPlayedCards;
        }
        private void OnDestroy()
        {
            GameEvents.OnTurnEnd -= GameEvents_OnTurnEnd;
            CardEvents.OnRevealCard -= CardEvents_OnRevealCard;
            CardEvents.OnRevealEnd -= RemoveCards;
            CardEvents.OnClearPlayedCards -= CardEvents_OnClearPlayedCards;
        }
        private void CardEvents_OnClearPlayedCards(PlayerSlot obj)
        {
            if (LocalPlayerContext.MySlot == obj) return;
            RemoveCards();
        }

        private void CardEvents_OnRevealCard(string playerId, CardData obj)
        {
            if (LocalPlayerContext.MySlot.ToString() == playerId) return;
            foreach (var item in m_remotePlayedCards)
            {
                if (item.Card.CardData.id == obj.id)
                {
                    item.Card.FaceUp();
                }
            }
        }
        private void GameEvents_OnTurnEnd(TurnEndMessage obj)
        {
            if (LocalPlayerContext.MySlot.ToString() == obj.playerId) return;
            for (int i = 0; i < obj.playedCards.Count; i++)
            {
                SpawnCardToBoard(obj.playedCards[i]);
            }
        }
        private void RemoveCards()
        {
            if (m_remotePlayedCards.Count > 0)
                m_remotePlayedCards.ForEach(x => Destroy(x.gameObject));
            m_remotePlayedCards.Clear();
        }
        private void SpawnCardToBoard(CardData cardData)
        {
            RemoteCard placedCard = Instantiate(m_card, transform);
            placedCard.Card.CardData = cardData;
            placedCard.Card.Init();
            placedCard.Card.FaceDown();
            m_remotePlayedCards.Add(placedCard);
        }
    }
}
