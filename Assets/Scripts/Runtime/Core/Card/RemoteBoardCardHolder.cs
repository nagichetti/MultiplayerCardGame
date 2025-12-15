using NUnit.Framework;
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
        }
        private void OnDestroy()
        {
            GameEvents.OnTurnEnd -= GameEvents_OnTurnEnd;
        }
        private void GameEvents_OnTurnEnd(TurnEndMessage obj)
        {
            if (LocalPlayerContext.MySlot.ToString() == obj.playerId) return;
            for (int i = 0; i < obj.playedCards; i++)
            {
                SpawnCardToBoard();
            }
        }

        private void SpawnCardToBoard()
        {
            RemoteCard placedCard = Instantiate(m_card, transform);
            m_remotePlayedCards.Add(placedCard);
        }
    }
}
