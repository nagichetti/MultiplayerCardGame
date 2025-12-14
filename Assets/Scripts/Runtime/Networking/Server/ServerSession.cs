using System.Collections.Generic;
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
        public static Dictionary<ulong, string> clientToPlayerId = new();

        public static Dictionary<ulong, PlayerSlot> clientSlots = new();
        private static Dictionary<PlayerSlot, ulong> slotOwners = new()
    {
        { PlayerSlot.Player1, ulong.MaxValue },
        { PlayerSlot.Player2, ulong.MaxValue }
    };
        //When Player joins add it to the dictionary
        public static void RegisterPlayer(ulong clientId)
        {
            PlayerSlot assignedSlot = GetFreeSlot();

            if (assignedSlot == PlayerSlot.None)
            {
                Debug.LogWarning("Match full");
                return;
            }


            clientSlots[clientId] = assignedSlot;
            slotOwners[assignedSlot] = clientId;

            SendPlayerAssigned(assignedSlot, clientId);

            Debug.Log($"Client {clientId} assigned {assignedSlot}");

            GameEvents.PlayerJoined(assignedSlot, clientId);
        }



        //When Player joins remove it from the dictionary
        public static void UnregisterPlayer(ulong clientId)
        {
            if (clientToPlayerId.ContainsKey(clientId))
            {
                GameEvents.PlayerQuit(clientToPlayerId[clientId]);
                clientToPlayerId.Remove(clientId);
            }
        }

        private static PlayerSlot GetFreeSlot()
        {
            if (slotOwners[PlayerSlot.Player1] == ulong.MaxValue)
                return PlayerSlot.Player1;

            if (slotOwners[PlayerSlot.Player2] == ulong.MaxValue)
                return PlayerSlot.Player2;

            return PlayerSlot.None;
        }

        //Broadcasting from server to client
        public static void Broadcast(string json)
        {
            NetworkMessageRouter.Instance.SendToClientClientRpc(json);
        }

        private static void SendPlayerAssigned(PlayerSlot slot, ulong targetClientId)
        {
            var msg = new PlayerAssignMessage
            {
                action = Actions.playerAssigned.ToString(),
                playerSlot = slot.ToString()
            };

            JsonNetworkClient.SendToClient(msg, targetClientId);
        }

    }

}