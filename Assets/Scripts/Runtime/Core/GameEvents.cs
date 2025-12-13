using System;

namespace CardGame
{
    public static class GameEvents
    {
        public static event Action<string, ulong> OnPlayerJoined;
        public static event Action<string> OnPlayerQuit;
        public static event Action<GameStartMessage> OnGameStart;

        public static void PlayerJoined(string playerId, ulong clientId)
        {
            OnPlayerJoined?.Invoke(playerId, clientId);
        }
        public static void PlayerQuit(string playerId)
        {
            OnPlayerQuit?.Invoke(playerId);
        }
        public static void GameStart(GameStartMessage payload)
        {
            OnGameStart?.Invoke(payload);
        }
    }
}
