using System;
using Unity.Netcode;
using UnityEngine;

namespace CardGame
{
    public static class ClientMessageHandler
    {
        public static void Process(string json)
        {
            Debug.Log($"RAW JSON: {json}");
            var msg = JsonUtility.FromJson<NetworkMessage>(json);

            switch (msg.action)
            {
                case nameof(Actions.playerAssigned):
                    HandlePlayerAssigned(json);
                    break;
                case nameof(Actions.gameStart):
                    HandleGameStart(json);
                    break;
                case nameof(Actions.turnStart):
                    HandleTurnStart(json);
                    break;
            }
            Debug.Log($"Action: {msg.action}");
        }

        private static void HandlePlayerAssigned(string json)
        {
            var msg = JsonUtility.FromJson<PlayerAssignMessage>(json);

            PlayerSlot slot = Enum.Parse<PlayerSlot>(msg.playerSlot);
            LocalPlayerContext.Set(slot, NetworkManager.Singleton.LocalClientId);
        }
        private static void HandleTurnStart(string json)
        {
            var msg = JsonUtility.FromJson<TurnStartMessage>(json);
            GameEvents.TurnStart(msg);
        }

        private static void HandleGameStart(string json)
        {
            var msg = JsonUtility.FromJson<GameStartMessage>(json);
            GameEvents.GameStart(msg);
        }
    }
}
