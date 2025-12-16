using System;

namespace CardGame
{
    public static class GameEvents
    {
        public static event Action OnPlayerJoined;
        public static event Action OnEndTurnPressed;
        public static event Action OnPlayersReady;
        public static event Action<GameStartMessage> OnGameStart;
        public static event Action<GameEndMessage> OnGameEnd;
        public static event Action<TurnStartMessage> OnTurnStart;
        public static event Action<TurnEndMessage> OnTurnEnd;
        public static event Action<RevealCardMessage> OnRevealCard;
        public static event Action<ScoreUpdateMessage> OnScoreUpdate;
        
        public static void PlayerJoined()
        {
            OnPlayerJoined?.Invoke();
        }
        public static void PlayersReady()
        {
            OnPlayersReady?.Invoke();
        }
        public static void EndTurnPressed()
        {
            OnEndTurnPressed?.Invoke();
        }
        public static void GameStart(GameStartMessage msg)
        {
            OnGameStart?.Invoke(msg);
        }
        public static void GameEnd(GameEndMessage msg)
        {
            OnGameEnd?.Invoke(msg);
        }
        public static void TurnStart(TurnStartMessage msg)
        {
            OnTurnStart?.Invoke(msg);
        }
        public static void TurnEnd(TurnEndMessage msg)
        {
            OnTurnEnd?.Invoke(msg);
        }
        public static void RevealCard(RevealCardMessage msg)
        {
            OnRevealCard?.Invoke(msg);
        }
        public static void UpdateScore(ScoreUpdateMessage msg)
        {
            OnScoreUpdate?.Invoke(msg);
        }
    }
}