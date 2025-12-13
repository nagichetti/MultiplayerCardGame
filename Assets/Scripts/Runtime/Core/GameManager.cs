using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace CardGame
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField]
        private GameData GameData;
        [SerializeField]
        private NetworkPlayer m_networkPrefab;
        [SerializeField]
        private GameObject m_mainMenu;
        [SerializeField]
        private GameObject m_game;
        private void Awake()
        {
            GameEvents.OnPlayerJoined += GameEvents_OnPlayerJoined;
            GameEvents.OnPlayerQuit += GameEvents_OnPlayerQuit;
            GameEvents.OnGameStart += GameEvents_OnGameStart;
        }

        private void OnDestroy()
        {
            GameData.ResetData();
            GameEvents.OnPlayerJoined -= GameEvents_OnPlayerJoined;
            GameEvents.OnPlayerQuit -= GameEvents_OnPlayerQuit;
            GameEvents.OnGameStart -= GameEvents_OnGameStart;
        }

        private void GameEvents_OnGameStart(GameStartMessage obj)
        {
            m_game.SetActive(true);
            m_mainMenu.SetActive(false);

            if (GameData.PlayerIds.Count == 0)
                GameData.PlayerIds = obj.playerIds.ToList();

            GameData.Totalturns = obj.totalTurns;
        }

        private void GameEvents_OnPlayerJoined(string obj, ulong clienId)
        {
            NetworkPlayer player = Instantiate(m_networkPrefab);
            player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clienId);

            GameData.PlayersJoined++;
            GameData.PlayerIds.Add(obj);

            if (GameData.PlayersJoined == GameData.PlayersNeeded)
                StartMatch();
        }
        private void GameEvents_OnPlayerQuit(string obj)
        {
            GameData.PlayersJoined--;

            if (GameData.PlayerIds.Contains(obj))
                GameData.PlayerIds.Remove(obj);
        }

        private void StartMatch()
        {
            var msg = new GameStartMessage
            {
                action = "gameStart",
                playerIds = GameData.PlayerIds.ToArray(),
                totalTurns = GameData.Totalturns
            };

            ServerSession.Broadcast(msg);
        }
    }
}
