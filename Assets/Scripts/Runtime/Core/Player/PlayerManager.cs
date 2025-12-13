using UnityEngine;

namespace CardGame
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField]
        private Player m_localPlayer;
        [SerializeField]
        private Player m_remotePlayer;
        [SerializeField]
        private Transform m_localPlayerSpawnPoint;
        [SerializeField]
        private Transform m_remotePlayerSpawnPoint;

        private void Awake()
        {
            NetworkPlayer.OnSpawnLocalPlayer += NetworkPlayer_OnSpawnHostPlayer;
            NetworkPlayer.OnSpawnRemotePlayer += NetworkPlayer_OnSpawnClientPlayer;
        }
        private void OnDestroy()
        {
            NetworkPlayer.OnSpawnLocalPlayer -= NetworkPlayer_OnSpawnHostPlayer;
            NetworkPlayer.OnSpawnRemotePlayer -= NetworkPlayer_OnSpawnClientPlayer;
        }
        private void NetworkPlayer_OnSpawnHostPlayer(NetworkPlayer obj)
        {
            Debug.Log("Spawning Local");
            var player = Instantiate(m_localPlayer, m_localPlayerSpawnPoint);
            player.Bind(obj);
        }

        private void NetworkPlayer_OnSpawnClientPlayer(NetworkPlayer obj)
        {
            Debug.Log("Spawning Remote");
            var player = Instantiate(m_remotePlayer, m_remotePlayerSpawnPoint);
            player.Bind(obj);
        }
    }
}
