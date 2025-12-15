using System;

namespace CardGame
{
    public static class CardEvents
    {
        public static event Action<Card> OnSpawnCardToHand;
        public static event Action<Card> OnAddCardToHand;
        public static event Action<Card> OnRemoveCardFromHand;
        public static event Action<Card> OnAddCardToBorad;
        public static event Action<Card> OnRemoveCardFromBorad;
        public static event Action<string, CardData> OnRevealCard;
        public static event Action OnRevealEnd;

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
        public static void RevealCard(string playerId, CardData card)
        {
            OnRevealCard?.Invoke(playerId, card);
        }
        public static void RevealEnd()
        {
            OnRevealEnd?.Invoke();
        }
    }
}
