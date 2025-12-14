using System;
using Unity.Netcode;
using UnityEngine;

namespace CardGame
{
    public static class ServerMessageHandler
    {
        public static void Process(string json, ulong senderClientId)
        {
            //if (!NetworkManager.Singleton.IsHost) return;
            Debug.Log($"RAW JSON ON SERVER: {json}");
            var msg = JsonUtility.FromJson<NetworkMessage>(json);

            switch (msg.action)
            {
                case nameof(Actions.playerAssigned):
                    ValidatePlayerAssign(json);
                    break;
                case nameof(Actions.gameStart):
                    ValidateGameStart(json);
                    break;
                case nameof(Actions.turnStart):
                    ValidateTurnStart(json);
                    break;
                default:
                    Debug.LogError($"Unknown Message: {msg.action}");
                    break;
            }

            Debug.Log($"Validating {msg.action}");
        }

        private static void ValidatePlayerAssign(string json)
        {
            ServerSession.Broadcast(json);
        }

        private static void ValidateTurnStart(string json)
        {
            ServerSession.Broadcast(json);
        }

        private static void ValidateGameStart(string json)
        {
            ServerSession.Broadcast(json);
        }
    }
}
