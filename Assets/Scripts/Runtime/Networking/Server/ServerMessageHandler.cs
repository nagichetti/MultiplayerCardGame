using System;
using Unity.Netcode;
using UnityEngine;

namespace CardGame
{
    public static class ServerMessageHandler
    {
        public static void Process(string json, ulong senderClientId)
        {
            if (!NetworkManager.Singleton.IsHost) return;
            var msg = JsonUtility.FromJson<NetworkMessage>(json);

            switch (msg.action)
            {
                case "gameStart":
                    ValidateGameStart(json);
                    break;
                default:
                    Debug.LogError($"Unknown Message: {msg.action}");
                    break;
            }
        }

        private static void ValidateGameStart(string json)
        {
            Debug.Log("Board casting game start");
            ServerSession.Broadcast(json);
        }
    }
}
