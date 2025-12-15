using System;
using Unity.Netcode;
using UnityEngine;

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
                NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
                NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback; ;
            }
        }

        private void Singleton_OnClientConnectedCallback(ulong obj)
        {
            ServerSession.OnClientConnected(obj);
        }


        //UnRegister from the serversession
        private void OnClientDisconnectCallback(ulong obj)
        {
            ServerSession.OnClientDisconnected(obj);
            NetworkManager.Singleton.Shutdown();
        }

        #region Communication
        //Sending msgs to server
        [ServerRpc(InvokePermission = RpcInvokePermission.Everyone)]
        public void SendToServerRpc(string json, ServerRpcParams rpcParams = default)
        {
            ServerMessageHandler.Process(json, rpcParams.Receive.SenderClientId);
        }
        [ClientRpc]
        public void ProcessFromClientRpc(string json, ulong senderClientId)
        {
            ServerMessageHandler.Process(json, senderClientId);
        }

        //Sending msgs to client
        [ClientRpc]
        public void SendToClientClientRpc(string json, ClientRpcParams rpcParams = default)
        {
            ClientMessageHandler.Process(json);
        }
       
        #endregion
    }
}

