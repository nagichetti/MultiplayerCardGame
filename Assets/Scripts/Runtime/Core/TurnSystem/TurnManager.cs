using Unity.Netcode;
using UnityEngine;

namespace CardGame
{
    public class TurnManager : NetworkBehaviour
    {
        [SerializeField]
        private GameData GameData;

        public static TurnManager Instance;

        public PlayerSlot currentplayerTurn;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            GameEvents.OnGameStart += GameEvents_OnGameStart;
            GameEvents.OnTurnStart += GameEvents_OnTurnStart;
            GameEvents.OnTurnEnd += GameEvents_OnTurnEnd;
        }
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            GameEvents.OnGameStart -= GameEvents_OnGameStart;
            GameEvents.OnTurnStart -= GameEvents_OnTurnStart;
            GameEvents.OnTurnEnd -= GameEvents_OnTurnEnd;
        }

        private void GameEvents_OnGameStart(GameStartMessage obj)
        {
            SendStartTurnMsg(PickrandomPlayer());
        }
        private PlayerSlot PickrandomPlayer()
        {
            PlayerSlot[] playerSlots = new PlayerSlot[] { PlayerSlot.Player1, PlayerSlot.Player2 };
            int j = Random.Range(0, playerSlots.Length);
            return playerSlots[j];
        }
        private void GameEvents_OnTurnStart(TurnStartMessage obj)
        {
            currentplayerTurn = (PlayerSlot)System.Enum.Parse(typeof(PlayerSlot), obj.playerId);
            GameData.CurrentTurn++;
            GameData.RemainingCost += GameData.CurrentTurn;

            Debug.Log($"Unlocking {obj.playerId} Cards");
            if (obj.playerId == LocalPlayerContext.MySlot.ToString())
                CardManager.Instance.UpdateCards();
            else
                CardManager.Instance.LockInHandCards();
        }
        private void GameEvents_OnTurnEnd(TurnEndMessage obj)
        {
            PlayerSlot nextPlayer = currentplayerTurn == PlayerSlot.Player1 ? PlayerSlot.Player2 : PlayerSlot.Player1;

            currentplayerTurn = nextPlayer;

            CardManager.Instance.LockInHandCards();
            CardManager.Instance.LockBoardCards();

            SendStartTurnMsg(nextPlayer);
        }



        public void EndCurrentPlayerTurn()
        {
            if (currentplayerTurn == LocalPlayerContext.MySlot)
                SendEndTurnMsg(currentplayerTurn);
        }
        private void SendStartTurnMsg(PlayerSlot playerSlot)
        {
            if (!IsOwner) return;
            TurnStartMessage turnStartMsg = new TurnStartMessage
            {
                action = nameof(Actions.turnStart),
                playerId = playerSlot.ToString()
            };
            currentplayerTurn = playerSlot;
            JsonNetworkClient.SendToClients(turnStartMsg);
        }
        public void SendEndTurnMsg(PlayerSlot playerSlot)
        {
            TurnEndMessage turnEndMsg = new TurnEndMessage
            {
                action = nameof(Actions.turnEnd),
                playerId = playerSlot.ToString(),
                playedCards = CardManager.Instance.GetPlayedCards()
            };

            JsonNetworkClient.SendToClients(turnEndMsg);
        }
    }
}
