using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace CardGame
{
    public class RevealManager : NetworkBehaviour
    {
        public static RevealManager Instance;
        [SerializeField]
        private GameData GameData;
        public PlayerSlot revealPhaseFirst, revealPhaseSecond;
        public bool IsRevealPhase;

        private Dictionary<string, List<CardData>> playedCards = new();

        public List<(string, CardData)> revealOrder = new();

        bool scoreUpdated = true;
        string lastUpdatedPlayer;
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            GameEvents.OnPlayersReady += GameEvents_OnPlayersReady;
            GameEvents.OnTurnEnd += GameEvents_OnTurnEnd;
            GameEvents.OnRevealCard += GameEvents_OnRevealCard;
            GameEvents.OnScoreUpdate += GameEvents_OnScoreUpdate;
        }
        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            GameEvents.OnPlayersReady -= GameEvents_OnPlayersReady;
            GameEvents.OnTurnEnd -= GameEvents_OnTurnEnd;
            GameEvents.OnRevealCard -= GameEvents_OnRevealCard;
            GameEvents.OnScoreUpdate -= GameEvents_OnScoreUpdate;
        }

        private void GameEvents_OnScoreUpdate(ScoreUpdateMessage obj)
        {
            Debug.Log("Adding score" + obj.playerId);
            if (lastUpdatedPlayer == obj.playerId) return;
            PlayerSlot player = (PlayerSlot)System.Enum.Parse(typeof(PlayerSlot), obj.playerId);
            GameData.PlayerScores[player] += obj.score;
            GameData.GameDataUpdated();
            scoreUpdated = true;
            lastUpdatedPlayer = obj.playerId;
        }

        private void GameEvents_OnRevealCard(RevealCardMessage obj)
        {
            lastUpdatedPlayer = string.Empty;

            CardEvents.RevealCard(obj.playerId, revealOrder[obj.orderIndex].Item2);
            UpdateScore(revealOrder[obj.orderIndex].Item2, obj.playerId);
        }

        private void UpdateScore(CardData data, string playerId)
        {
            switch (data.abilityData.type)
            {
                case Ability.GainPoints:
                    SendScore(playerId, data.power + data.abilityData.value);
                    break;
                default:
                    SendScore(playerId, data.power);
                    break;
            }
        }

        private void GameEvents_OnTurnEnd(TurnEndMessage obj)
        {
            playedCards[obj.playerId] = obj.playedCards;
        }

        private void GameEvents_OnPlayersReady()
        {
            StartRevealPhase();
        }

        public void StartRevealPhase()
        {
            if (IsRevealPhase) return;
            IsRevealPhase = true;
            CardManager.Instance.LockBoardCards();
            CardManager.Instance.LockInHandCards();
            revealPhaseFirst = DetermineInitiative();

            PlayerSlot revealPhaseSecond = revealPhaseFirst == PlayerSlot.Player1 ? PlayerSlot.Player2 : PlayerSlot.Player1;

            ArrangeCards();
            foreach (var item in CardManager.Instance.GetBoardCards())
            {
                item.FaceDown();
            }
            StartCoroutine(RevealSequence());
        }

        private void ArrangeCards()
        {
            var first = PlayerSlot.Player1.ToString();
            var second = PlayerSlot.Player2.ToString();
            playedCards.TryGetValue(first, out var firstCards);
            playedCards.TryGetValue(second, out var secondCards);

            firstCards ??= new List<CardData>();
            secondCards ??= new List<CardData>();

            int max = Mathf.Max(firstCards.Count, secondCards.Count);

            for (int i = 0; i < max; i++)
            {
                if (i < firstCards.Count)
                    revealOrder.Add((first, firstCards[i]));

                if (i < secondCards.Count)
                    revealOrder.Add((second, secondCards[i]));
            }
        }


        public void EndRevealPhase()
        {
            IsRevealPhase = false;
            revealOrder.Clear();
            CardManager.Instance.ClearPlayedCards();
            CardEvents.RevealEnd();
            TurnManager.Instance.StartNextTurn(DetermineInitiative());
        }

        public PlayerSlot DetermineInitiative()
        {
            if (GameData.PlayerScores[PlayerSlot.Player1] > GameData.PlayerScores[PlayerSlot.Player2]) return PlayerSlot.Player1;
            if (GameData.PlayerScores[PlayerSlot.Player2] > GameData.PlayerScores[PlayerSlot.Player1]) return PlayerSlot.Player2;
            return Random.value > 0.5f ? PlayerSlot.Player1 : PlayerSlot.Player2;
        }

        IEnumerator RevealSequence()
        {
            int count = 0;
            Debug.Log($"Reveal Order Count {revealOrder.Count}");
            while (revealOrder.Count > count)
            {
                yield return new WaitUntil(() => scoreUpdated == true);
                scoreUpdated = false;
                yield return new WaitForSeconds(2f);
                SendRevealCard(count, revealOrder[count].Item1);
                count++;
            }
            yield return new WaitForSeconds(2);
            EndRevealPhase();
        }
        public void SendRevealCard(int index, string playerId)
        {
            if (!IsHost) return;
            RevealCardMessage msg = new RevealCardMessage
            {
                action = nameof(Actions.revealCard),
                playerId = playerId,
                orderIndex = index
            };

            JsonNetworkClient.SendToClients(msg);
        }
        public void SendScore(string playerId, int score)
        {
            ScoreUpdateMessage msg = new ScoreUpdateMessage
            {
                action = nameof(Actions.scoreUpdate),
                playerId = playerId,
                score = score
            };

            JsonNetworkClient.SendToClients(msg);
        }
    }
}
