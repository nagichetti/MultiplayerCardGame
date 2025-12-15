using System;
using UnityEditor.PackageManager;

namespace CardGame
{
    public static class GameEvents
    {
        public static event Action OnPlayerJoined;
        public static event Action<GameStartMessage> OnGameStart;
        public static event Action<TurnStartMessage> OnTurnStart;
        
        public static void PlayerJoined()
        {
            OnPlayerJoined?.Invoke();
        }
        public static void GameStart(GameStartMessage msg)
        {
            OnGameStart?.Invoke(msg);
        }
        public static void TurnStart(TurnStartMessage msg)
        {
            OnTurnStart?.Invoke(msg);
        }
    }
}