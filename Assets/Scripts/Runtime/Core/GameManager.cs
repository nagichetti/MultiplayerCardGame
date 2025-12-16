using System.Collections;
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

        public bool spam;
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
            GameData.CurrentTurn ++;
            GameData.RemainingCost = GameData.CurrentTurn;
            GameData.PlayerScores.Add(PlayerSlot.Player1, 0);
            GameData.PlayerScores.Add(PlayerSlot.Player2, 0);
        }

        private void GameEvents_OnPlayerJoined()
        {
            if (!NetworkManager.Singleton.IsServer)
                return;

            GameData.PlayersJoined++;

            if (GameData.PlayersJoined == GameData.PlayersNeeded && !GameStarted)
            {
                GameStarted = true;
                StartCoroutine(StartGameWithDelay());
                //StartMatch();
            }
        }
        IEnumerator StartGameWithDelay()
        {
            yield return new WaitForSeconds(2f);
                ServerSession.StartGame();

        }
        private void OnValidate()
        {
            if (spam)
            {
                StartMatch();
                spam = false;
            }
        }
        private void StartMatch()
        {
            var msg = new GameStartMessage
            {
                action = nameof(Actions.gameStart),
                playerIds = GameData.PlayerIds.ToArray(),
                totalTurns = GameData.Totalturns
            };
            Debug.Log("Sending" + msg.action);

            JsonNetworkClient.SendToClients(msg);
        }
    }
}
