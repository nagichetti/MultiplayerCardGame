using CardGame;
using System;
using Unity.Netcode;
using UnityEngine;

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

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
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
    public void SendToServerRpc(string json, RpcParams rpcParams = default)
    {
        ServerMessageHandler.Process(json, rpcParams.Receive.SenderClientId);
    }

    [ClientRpc]
    public void SendToClientClientRpc(string json)
    {
        Debug.Log($"CLIENT RPC RECEIVED | IsHost={IsHost} | {json}");
        ClientMessageHandler.Process(json);
    }
}
