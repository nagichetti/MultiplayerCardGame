using System;
using UnityEngine;

namespace CardGame
{
    public static class CardEvents
    {
        public static event Action<Card> OnSpawnCardToHand;
        public static event Action<Card> OnAddCardToHand;
        public static event Action<Card> OnRemoveCardFromHand;
        public static event Action<Card> OnAddCardToBorad;
        public static event Action<Card> OnRemoveCardFromBorad;

        public static void SpawnCardToHand(Card card)
        {
            OnSpawnCardToHand?.Invoke(card);
        }
        public static void AddCardToHand(Card card)
        {
            OnAddCardToHand?.Invoke(card);
            CardManager.Instance.AddCardToInHand(card);
        }
        public static void RemoveCardFromHand(Card card)
        {
            OnRemoveCardFromHand?.Invoke(card);
            CardManager.Instance.RemoveCardFromInHand(card);
        }
        public static void AddCardToBoard(Card card)
        {
            OnAddCardToBorad?.Invoke(card);
            CardManager.Instance.AddCardToBorad(card);
        }
        public static void RemoveCardFromBoard(Card card)
        {
            OnRemoveCardFromBorad?.Invoke(card);
            CardManager.Instance.RemoveCardFromBorad(card);
        }
    }
}
