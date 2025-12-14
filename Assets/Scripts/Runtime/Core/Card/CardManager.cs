using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class CardManager : Singleton<CardManager>
    {
        [SerializeField]
        private DeckData DeckData;
        [SerializeField]
        private Card cardPrefab;

        [SerializeField]
        private List<CardData> ShuffledCards;
        [SerializeField]
        private List<Card> InHandCards;
        [SerializeField]
        private List<Card> InBoardCards;

        private int currentCardIndex;
        public override void Awake()
        {
            Debug.Log("Awake");
            base.Awake();
            GameEvents.OnGameStart += GameEvents_OnGameStart;
        }

        private void OnDestroy()
        {
            GameEvents.OnGameStart -= GameEvents_OnGameStart;
        }
        public void AddCardToInHand(Card obj)
        {
            if (!InHandCards.Contains(obj))
                InHandCards.Add(obj);
        }
        public void RemoveCardFromInHand(Card obj)
        {
            if (InHandCards.Contains(obj))
                InHandCards.Remove(obj);
        }
        public void AddCardToBorad(Card obj)
        {
            if(!InBoardCards.Contains(obj))
                InBoardCards.Add(obj);
        }

        public void RemoveCardFromBorad(Card obj)
        {
            if (InBoardCards.Contains(obj))
                InBoardCards.Remove(obj);
        }

        private void GameEvents_OnGameStart(GameStartMessage obj)
        {
            Debug.Log("Giving out the cards");
            CreateShuffledDeck();
            for (int i = 0; i < DeckData.StartingHand; i++)
            {
                cardPrefab.CardData = ShuffledCards[i];
                CardEvents.SpawnCardToHand(cardPrefab);
                currentCardIndex = i;
            }
        }
        public void GiveNextCards(int cardCount)
        {
            for (int i = 0; i < cardCount; i++)
            {
                cardPrefab.CardData = ShuffledCards[currentCardIndex];
                CardEvents.SpawnCardToHand(cardPrefab);
                currentCardIndex++;
            }
        }
        public void CreateShuffledDeck()
        {
            ShuffledCards = new List<CardData>();

            foreach (var card in DeckData.Cards)
            {
                ShuffledCards.Add(card);
            }

            Shuffle(ShuffledCards);
        }

        void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        public void LockInHandCards()
        {
            Debug.Log("Locking hands");
            foreach (var card in InHandCards)
            {
                card.Lock();
            }
        }
        public void UnlockInHandCards()
        {
            Debug.Log("UnLocking hands");
            foreach (var card in InHandCards)
            {
                card.UnLock();
            }
        }
    }
}
