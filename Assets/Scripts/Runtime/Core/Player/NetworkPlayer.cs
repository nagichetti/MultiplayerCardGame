using System;
using Unity.Netcode;

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
        }
    }
}
