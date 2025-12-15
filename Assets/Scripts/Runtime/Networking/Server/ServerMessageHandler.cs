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
                    ValidateGameStart(json, senderClientId);
                    break;

                case nameof(Actions.turnStart):
                    ValidateTurnStart(json, senderClientId);
                    break;

                default:
                    Debug.LogError($"Unknown or invalid server message: {msg.action}");
                    break;
            }

            Debug.Log($"Validating {msg.action}");
        }


        private static void ValidateGameStart(string json, ulong senderClientId)
        {
            ServerSession.Broadcast(json);
        }

        private static void ValidateTurnStart(string json, ulong senderClientId)
        {
            ServerSession.Broadcast(json);
        }
    }
}
