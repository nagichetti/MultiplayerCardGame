using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace CardGame
{
    public enum PlayerSlot
    {
        None,
        Player1,
        Player2
    }
    public static class ServerSession
    {
        public static bool ClientDisconnected;

        public static void OnClientDisconnected(ulong clientId)
        {
            if (!NetworkManager.Singleton.IsServer)
                return;

            Debug.Log("Client disconnected, waiting for reconnection");
            ClientDisconnected = true;
        }

        public static void OnClientConnected(ulong clientId)
        {
            if (!NetworkManager.Singleton.IsServer)
                return;

            if (ClientDisconnected)
            {
                Debug.Log("Client reconnected as Player2");
                ClientDisconnected = false;
            }
        }

        public static void StartGame()
        {
            if (!NetworkManager.Singleton.IsServer)
                return;

            Debug.Log("SERVER starting game");

            var msg = new GameStartMessage
            {
                action = nameof(Actions.gameStart),
                playerIds = new[] { "Player1", "Player2" },
                totalTurns = GameManager.Instance.GameData.Totalturns
            };

            string json = JsonUtility.ToJson(msg);

            NetworkMessageRouter.Instance.SendToClientClientRpc(json);
        }

        public static void Broadcast(string msg)
        {
            NetworkMessageRouter.Instance.SendToClientClientRpc(msg);
        }
    }
}