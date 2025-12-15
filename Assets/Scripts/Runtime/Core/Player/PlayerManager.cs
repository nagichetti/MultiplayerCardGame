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

        bool m_spawnedLocal, m_spawnedRemote;

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
            if (m_spawnedLocal) return;

            Debug.Log("Spawning Local");
            var player = Instantiate(m_localPlayer, m_localPlayerSpawnPoint);
            player.Bind(obj);
            m_spawnedLocal = true;
        }

        private void NetworkPlayer_OnSpawnClientPlayer(NetworkPlayer obj)
        {
            if(m_spawnedRemote) return;

            Debug.Log("Spawning Remote");
            var player = Instantiate(m_remotePlayer, m_remotePlayerSpawnPoint);
            player.Bind(obj);
            m_spawnedRemote = true;
        }
    }
}
