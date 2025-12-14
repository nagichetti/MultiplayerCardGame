using Unity.Netcode;
using UnityEngine;

namespace CardGame
{
    public static class JsonNetworkClient
    {
        public static void SendToClients(object msg)
        {
            string json = JsonUtility.ToJson(msg);
            NetworkMessageRouter.Instance.SendToServerRpc(json);
        }

        public static void SendToClient(object msg, ulong targetClientId)
        {
            string json = JsonUtility.ToJson(msg);
            ClientRpcParams rpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { targetClientId }
                }
            };
            NetworkMessageRouter.Instance.SendToClientClientRpc(json, rpcParams);
        }
    }
}
