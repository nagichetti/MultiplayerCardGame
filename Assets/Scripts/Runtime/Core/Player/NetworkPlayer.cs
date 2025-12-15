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
            GameEvents.PlayerJoined();
        }

    }
}
