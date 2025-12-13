using System;

namespace CardGame
{
    public static class GameEvents
    {
        public static event Action<string> OnPlayerJoined;
        public static event Action<string> OnPlayerQuit;
        public static event Action<string> OnGameStart;

        public static void PlayerJoined(string playerId)
        {
            OnPlayerJoined?.Invoke(playerId);
        }
        public static void PlayerQuit(string playerId)
        {
            OnPlayerQuit?.Invoke(playerId);
        }
        public static void GameStart(string payload)
        {
            OnGameStart?.Invoke(payload);
        }
    }
}
