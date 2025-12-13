using Unity.Netcode;
using UnityEngine;

namespace CardGame
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField]
        private GameData GameData;

        private void Awake()
        {
            GameEvents.OnPlayerJoined += GameEvents_OnPlayerJoined;
            GameEvents.OnPlayerQuit += GameEvents_OnPlayerQuit;
        }
        private void OnDestroy()
        {
            GameData.ResetData();
            GameEvents.OnPlayerJoined -= GameEvents_OnPlayerJoined;
            GameEvents.OnPlayerQuit -= GameEvents_OnPlayerQuit;
        }

        private void GameEvents_OnPlayerJoined(string obj)
        {
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
