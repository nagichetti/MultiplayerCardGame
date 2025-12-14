using System;
using Unity.Netcode;

namespace CardGame
{
    public class NetworkMessageRouter : NetworkBehaviour
    {
        public static NetworkMessageRouter Instance;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        //OnNetwork spawn if its server listen to client joining events
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
                NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
            }

        }
        //Register from the serversession
        private void OnClientConnectedCallback(ulong obj)
        {
            ServerSession.RegisterPlayer(obj);
        }
        //UnRegister from the serversession
        private void OnClientDisconnectCallback(ulong obj)
        {
            ServerSession.UnregisterPlayer(obj);
        }

        #region Communication
        //Sending msgs to server
        [ServerRpc(InvokePermission = RpcInvokePermission.Everyone)]
        public void SendToServerRpc(string json)
        {
            ServerMessageHandler.Process(json, OwnerClientId);
        }
        [ClientRpc]
        public void ProcessFromClientRpc(string json, ulong senderClientId)
        {
            ServerMessageHandler.Process(json, senderClientId);
        }

        //Sending msgs to client
        [ClientRpc]
        public void SendToClientClientRpc(string json)
        {
            ClientMessageHandler.Process(json);
        }
        [ClientRpc]
        public void SendToClientClientRpc(string json, ClientRpcParams rpcParams = default)
        {
            ClientMessageHandler.Process(json);
        }
       
        #endregion
    }
}

