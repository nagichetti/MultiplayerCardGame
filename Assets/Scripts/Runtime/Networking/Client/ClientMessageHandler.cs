using UnityEngine;

namespace CardGame
{
    public static class ClientMessageHandler
    {
        public static void Process(string json)
        {
            Debug.Log($"RAW JSON: {json}");
            var msg = JsonUtility.FromJson<NetworkMessage>(json);

            switch (msg.action)
            {
                case nameof(Actions.gameStart):
                    HandleGameStart(json);
                    break;
                case nameof(Actions.turnStart):
                    HandleTurnStart(json);
                    break;
                case nameof(Actions.turnEnd):
                    HandleTurnEnd(json);
                    break;
                case nameof(Actions.allPlayersReady):
                    HandlePlayersReady(json);
                    break;
                case nameof(Actions.revealCard):
                    HandleRevealCard(json);
                    break;
            }
            Debug.Log($"Action: {msg.action}");
        }

        private static void HandleRevealCard(string json)
        {
            var msg = JsonUtility.FromJson<RevealCardMessage>(json);
            GameEvents.RevealCard(msg);
        }

        private static void HandlePlayersReady(string json)
        {
            var msg = JsonUtility.FromJson<AllPlayersReadyMessage>(json);
            GameEvents.PlayersReady();
        }

        private static void HandleTurnStart(string json)
        {
            var msg = JsonUtility.FromJson<TurnStartMessage>(json);
            GameEvents.TurnStart(msg);
        }

        private static void HandleGameStart(string json)
        {
            var msg = JsonUtility.FromJson<GameStartMessage>(json);
            GameEvents.GameStart(msg);
        }
        private static void HandleTurnEnd(string json)
        {
            var msg = JsonUtility.FromJson<TurnEndMessage>(json);
            GameEvents.TurnEnd(msg);
        }
    }
}
