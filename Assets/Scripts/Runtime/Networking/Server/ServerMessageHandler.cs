using System;
using UnityEngine;

namespace CardGame
{
    public static class ServerMessageHandler
    {
        public static void Process(string json, ulong senderClientId)
        {
            Debug.Log($"RAW JSON ON SERVER: {json}");

            var msg = JsonUtility.FromJson<NetworkMessage>(json);

            switch (msg.action)
            {
                case nameof(Actions.gameStart):
                    ValidateGameStart(json);
                    break;

                case nameof(Actions.turnStart):
                    ValidateTurnStart(json);
                    break;
                case nameof(Actions.turnEnd):
                    ValidateTurnEnd(json);
                    break;
                default:
                    Debug.LogError($"Unknown or invalid server message: {msg.action}");
                    break;
            }

            Debug.Log($"Validating {msg.action}");
        }

        private static void ValidateTurnEnd(string json)
        {
            ServerSession.Broadcast(json);
        }

        private static void ValidateGameStart(string json)
        {
            ServerSession.Broadcast(json);
        }

        private static void ValidateTurnStart(string json)
        {
            ServerSession.Broadcast(json);
        }
    }
}
