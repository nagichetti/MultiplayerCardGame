using System.Collections;
using System.Linq;
using TMPro;
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
        [SerializeField]
        private TextMeshProUGUI finalMsg;

        private bool GameStarted;

        private NetworkPlayer networkPlayer;

        public override void Awake()
        {
            base.Awake();
            GameEvents.OnPlayerJoined += GameEvents_OnPlayerJoined;
            GameEvents.OnGameStart += GameEvents_OnGameStart;
            GameEvents.OnGameEnd += GameEvents_OnGameEnd;
        }

        private void GameEvents_OnGameEnd(GameEndMessage obj)
        {
            if (obj.wonPlayerId == PlayerSlot.None.ToString())
                finalMsg.text = "Draw";
            else
                finalMsg.text = LocalPlayerContext.MySlot.ToString() == obj.wonPlayerId ? "Victory" : "Defeat";

            finalMsg.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            GameData.ResetData();
            GameEvents.OnPlayerJoined -= GameEvents_OnPlayerJoined;
            GameEvents.OnGameStart -= GameEvents_OnGameStart;
            GameEvents.OnGameEnd -= GameEvents_OnGameEnd;
        }

        private void GameEvents_OnGameStart(GameStartMessage obj)
        {
            if (obj == null) return;

            m_game.SetActive(true);
            m_mainMenu.SetActive(false);

            if (GameData.PlayerIds.Count == 0)
                GameData.PlayerIds = obj.playerIds.ToList();

            GameData.Totalturns = obj.totalTurns;
            GameData.CurrentTurn++;
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
        public void EndMatch()
        {
            PlayerSlot winner = GameData.PlayerScores[PlayerSlot.Player1] > GameData.PlayerScores[PlayerSlot.Player2] ? PlayerSlot.Player1 : PlayerSlot.Player2;

            if (GameData.PlayerScores[PlayerSlot.Player1] == GameData.PlayerScores[PlayerSlot.Player2])
                winner = PlayerSlot.None;

            var msg = new GameEndMessage
            {
                action = nameof(Actions.gameEnd),
                wonPlayerId = winner.ToString(),
            };

            JsonNetworkClient.SendToClients(msg);
        }
    }
}
