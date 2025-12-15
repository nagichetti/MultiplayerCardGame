using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    public class CardManager : Singleton<CardManager>
    {
        [SerializeField]
        public GameData GameData;
        [SerializeField]
        private DeckData DeckData;
        [SerializeField]
        private Card cardPrefab;

        [SerializeField]
        private List<CardData> ShuffledCards;
        [SerializeField]
        private List<Card> InHandCards;
        [SerializeField]
        private List<Card> SelectedCards;
        [SerializeField]
        private List<Card> InBoardCards;

        private int currentCardIndex;
        public override void Awake()
        {
            Debug.Log("Awake");
            base.Awake();
            GameEvents.OnGameStart += GameEvents_OnGameStart;
            GameEvents.OnEndTurnPressed += GameEvents_OnEndTurnPressed;
        }

        private void GameEvents_OnEndTurnPressed()
        {
            foreach (var item in SelectedCards)
            {
                if(!InBoardCards.Contains(item))
                {
                    item.Fold();
                    CardEvents.AddCardToBoard(item);
                }
            }
            TurnManager.Instance.EndCurrentPlayerTurn();
        }

        private void OnDestroy()
        {
            GameEvents.OnGameStart -= GameEvents_OnGameStart;
            GameEvents.OnEndTurnPressed -= GameEvents_OnEndTurnPressed;
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
        public void AddCardToSelected(Card obj)
        {
            if (!SelectedCards.Contains(obj))
            {
                SelectedCards.Add(obj);
                UpdateCost(-obj.CardData.cost);
            }
        }

        public void RemoveCardFromSelected(Card obj)
        {
            if (SelectedCards.Contains(obj))
            {
                SelectedCards.Remove(obj);
                UpdateCost(obj.CardData.cost);
            }
        }
        public void AddCardToBorad(Card obj)
        {
            if (!InBoardCards.Contains(obj))
            {
                InBoardCards.Add(obj);
            }
        }

        public void RemoveCardFromBorad(Card obj)
        {
            if (InBoardCards.Contains(obj))
            {
                InBoardCards.Remove(obj);
            }
        }
        public int GetPlayedCards()
        {
            if(InBoardCards!=null)
                return InBoardCards.Count;
            return 0;
        }
        private void UpdateCost(int cost = 0)
        {
            GameData.RemainingCost += cost;
            UpdateCards();
        }
        public void UpdateCards()
        {
            foreach (var item in InHandCards)
            {
                item.UpdateCard(GameData.RemainingCost);
            }
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
            int cardCount = 0;
            for (int i = 0; i < DeckData.DeckSize; i++)
            {
                if (cardCount >= DeckData.Cards.Count)
                {
                    cardCount = 0;
                }
                ShuffledCards.Add(DeckData.Cards[cardCount]);
                cardCount++;
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

        public void LockBoardCards()
        {
            foreach (var card in InBoardCards)
            {
                card.Lock();
            }
        }
    }
}
