using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace CardGame
{
    public static class ServerSession
    {
        public static Dictionary<ulong, string> clientToPlayerId = new();
        public static int m_playerCount = 0;

        //When Player joins add it to the dictionary
        public static void RegisterPlayer(ulong clientId)
        {
            m_playerCount++;
            string playerId = $"Player{m_playerCount}";
            clientToPlayerId[clientId] = playerId;

            Debug.Log($"Client {clientId} assigned {playerId}");

            GameEvents.PlayerJoined(playerId, clientId);
        }

        //When Player joins remove it from the dictionary
        public static void UnregisterPlayer(ulong clientId)
        {
            if (clientToPlayerId.ContainsKey(clientId))
            {
                GameEvents.PlayerQuit(clientToPlayerId[clientId]);
                clientToPlayerId.Remove(clientId);
                m_playerCount--;
            }
        }
        //Broadcasting from server to client
        public static void Broadcast(object msg)
        {
            string json = JsonUtility.ToJson(msg);
            NetworkMessageRouter.Instance.SendToClientClientRpc(json);
        }
    }

}