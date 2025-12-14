using System;

namespace CardGame
{
    public static class GameEvents
    {
        public static event Action<PlayerSlot, ulong> OnPlayerJoined;
        public static event Action<string> OnPlayerQuit;
        public static event Action<GameStartMessage> OnGameStart;
        public static event Action<TurnStartMessage> OnTurnStart;
        
        public static void PlayerJoined(PlayerSlot playerId, ulong clientId)
        {
            OnPlayerJoined?.Invoke(playerId, clientId);
        }
        public static void PlayerQuit(string playerId)
        {
            OnPlayerQuit?.Invoke(playerId);
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