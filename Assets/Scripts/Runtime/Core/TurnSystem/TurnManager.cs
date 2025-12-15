using UnityEngine;

namespace CardGame
{
    public class TurnManager : Singleton<TurnManager>
    {
        [SerializeField]
        private GameData GameData;

        private NetworkPlayer player;
        private void Start()
        {
            GameEvents.OnGameStart += GameEvents_OnGameStart;
            GameEvents.OnTurnStart += GameEvents_OnTurnStart;
            NetworkPlayer.OnSpawnLocalPlayer += NetworkPlayer_OnSpawnLocalPlayer;
        }

        private void NetworkPlayer_OnSpawnLocalPlayer(NetworkPlayer obj)
        {
            player = obj;
        }

        private void OnDestroy()
        {
            GameEvents.OnGameStart -= GameEvents_OnGameStart;
            GameEvents.OnTurnStart -= GameEvents_OnTurnStart;
        }
        private void GameEvents_OnGameStart(GameStartMessage obj)
        {
            TurnStartMessage turnStartMsg = new TurnStartMessage
            {
                action = nameof(Actions.turnStart),
                playerId = LocalPlayerContext.MySlot.ToString()
            };

            var msg = JsonUtility.ToJson(turnStartMsg);
            player.SendToServerServerRpc(msg);
        }
        private void GameEvents_OnTurnStart(TurnStartMessage obj)
        {
            CardManager.Instance.UnlockInHandCards();
        }
    }
}
