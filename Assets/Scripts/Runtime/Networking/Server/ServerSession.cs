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

    /// <summary>
    /// Server-only session state.
    /// Tracks slot assignment, connection lifecycle, and reconnection.
    /// </summary>
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
        public static void Broadcast(object msg)
        {
            string json = JsonUtility.ToJson(msg);
            NetworkMessageRouter.Instance.SendToClientClientRpc(json);
        }
    }
}