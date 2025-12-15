using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace CardGame
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField]
        public GameData GameData;
        [SerializeField]
        private NetworkPlayer m_networkPrefab;
        [SerializeField]
        private GameObject m_mainMenu;
        [SerializeField]
        private GameObject m_game;

        private bool GameStarted;

        private NetworkPlayer networkPlayer;
        public override void Awake()
        {
            base.Awake();
            GameEvents.OnPlayerJoined += GameEvents_OnPlayerJoined;
            GameEvents.OnGameStart += GameEvents_OnGameStart;
        }

        private void OnDestroy()
        {
            GameData.ResetData();
            GameEvents.OnPlayerJoined -= GameEvents_OnPlayerJoined;
            GameEvents.OnGameStart -= GameEvents_OnGameStart;
        }

        private void GameEvents_OnGameStart(GameStartMessage obj)
        {
            if (obj == null) return;

            m_game.SetActive(true);
            m_mainMenu.SetActive(false);

            if (GameData.PlayerIds.Count == 0)
                GameData.PlayerIds = obj.playerIds.ToList();

            GameData.Totalturns = obj.totalTurns;
        }

        private void GameEvents_OnPlayerJoined()
        {
            if (!NetworkManager.Singleton.IsServer)
                return;

            GameData.PlayersJoined++;

            if (GameData.PlayersJoined == GameData.PlayersNeeded && !GameStarted)
            {
                GameStarted = true;
                ServerSession.StartGame();
            }
        }
    }
}
