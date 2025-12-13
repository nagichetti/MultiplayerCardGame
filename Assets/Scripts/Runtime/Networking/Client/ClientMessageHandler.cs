using System;
using UnityEngine;

namespace CardGame
{
    public static class ClientMessageHandler
    {
        public static void Process(string json)
        {
            var msg = JsonUtility.FromJson<NetworkMessage>(json);

            switch (msg.action)
            {
                case "gameStart":
                    HandleGameStart(json);
                    break;
            }
        }

        private static void HandleGameStart(string json)
        {
            var msg = JsonUtility.FromJson<GameStartMessage>(json);
            Debug.Log("GameStarted");
            Debug.Log($"Action: {msg.action}, Player: {msg.playerIds[0]}");
        }
    }
}
