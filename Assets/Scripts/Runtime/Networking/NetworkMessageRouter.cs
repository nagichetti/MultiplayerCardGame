using CardGame;
using System;
using Unity.Netcode;
using UnityEngine;

public class NetworkMessageRouter : NetworkBehaviour
{
    public static NetworkMessageRouter Instance;
    internal bool IsReady;

    public override void OnNetworkSpawn()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        }

        if (IsClient)
        {
            IsReady = true;
            Debug.Log("Router ready on client");
        }

        if (IsServer)
        {
            Debug.Log("Router ready on server");
        }
    }

    private void OnClientConnectedCallback(ulong clientId)
    {
        ServerSession.OnClientConnected(clientId);
    }

    private void OnClientDisconnectCallback(ulong clientId)
    {
        ServerSession.OnClientDisconnected(clientId);
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void SendToServerRpc(string json)
    {
        ServerMessageHandler.Process(json, NetworkManager.Singleton.LocalClientId);
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
}
