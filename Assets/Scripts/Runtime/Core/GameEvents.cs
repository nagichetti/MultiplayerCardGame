using System;
using UnityEditor.PackageManager;

namespace CardGame
{
    public static class GameEvents
    {
        public static event Action<PlayerSlot, ulong> OnPlayerJoined;
        public static event Action<PlayerSlot, ulong> OnPlayerQuit;
        public static event Action<GameStartMessage> OnGameStart;
        public static event Action<TurnStartMessage> OnTurnStart;
        
        public static void PlayerJoined(PlayerSlot playerId, ulong clientId)
        {
            OnPlayerJoined?.Invoke(playerId, clientId);
        }
        public static void PlayerQuit(PlayerSlot playerId, ulong clientId)
        {
            OnPlayerQuit?.Invoke(playerId, clientId);
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