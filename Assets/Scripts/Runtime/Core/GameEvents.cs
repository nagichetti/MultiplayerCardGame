using System;

namespace CardGame
{
    public static class GameEvents
    {
        public static event Action OnPlayerJoined;
        public static event Action OnEndTurnPressed;
        public static event Action<GameStartMessage> OnGameStart;
        public static event Action<TurnStartMessage> OnTurnStart;
        public static event Action<TurnEndMessage> OnTurnEnd;
        
        public static void PlayerJoined()
        {
            OnPlayerJoined?.Invoke();
        }
        public static void EndTurnPressed()
        {
            OnEndTurnPressed?.Invoke();
        }
        public static void GameStart(GameStartMessage msg)
        {
            OnGameStart?.Invoke(msg);
        }
        public static void TurnStart(TurnStartMessage msg)
        {
            OnTurnStart?.Invoke(msg);
        }
        public static void TurnEnd(TurnEndMessage msg)
        {
            OnTurnEnd?.Invoke(msg);
        }
    }
}