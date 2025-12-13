using UnityEngine;

namespace CardGame
{
    public static class JsonNetworkClient
    {
        public static void Send(object msg)
        {
            string json = JsonUtility.ToJson(msg);
            NetworkMessageRouter.Instance.SendToServersServerRpc(json);
        }
    }
}
