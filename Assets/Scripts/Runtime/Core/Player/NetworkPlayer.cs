using System;
using Unity.Netcode;
using UnityEngine;

namespace CardGame
{
    public class NetworkPlayer : NetworkBehaviour
    {
        public static event Action<NetworkPlayer> OnSpawnLocalPlayer;
        public static event Action<NetworkPlayer> OnSpawnRemotePlayer;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsOwner)
                OnSpawnLocalPlayer?.Invoke(this);
            else
                OnSpawnRemotePlayer?.Invoke(this);

            if (IsOwner)
            {
                // Local player
                if (IsHost)
                {
                    LocalPlayerContext.SetSlot(PlayerSlot.Player1);
                }
                else
                {
                    LocalPlayerContext.SetSlot(PlayerSlot.Player2);
                }
            }
        }

        [ServerRpc(InvokePermission = RpcInvokePermission.Everyone)]
        public void SendToServerServerRpc(string json, ServerRpcParams rpcParams = default)
        {
            NetworkMessageRouter.Instance.ProcessFromClientRpc(
                json,
                rpcParams.Receive.SenderClientId
            );
        }
    }
}
