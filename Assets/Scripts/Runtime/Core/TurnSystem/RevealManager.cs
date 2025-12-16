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

        private HashSet<CardData> destroyedCards = new();
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

            var card = revealOrder[obj.orderIndex].Item2;

            if (destroyedCards.Contains(card))
            {
                scoreUpdated = true; // allow reveal sequence to continue
                return;
            }

            CardEvents.RevealCard(obj.playerId, card);
            UpdateScore(card, obj.playerId);
        }

        private void UpdateScore(CardData data, string playerId)
        {

            switch (data.abilityData.type.ToString())
            {
                case nameof(Ability.GainPoints):
                    GainPoints(playerId, data);
                    break;
                case nameof(Ability.StealPoints):
                    StartCoroutine(StealPoints(playerId, data));
                    break;
                case nameof(Ability.DoublePower):
                    DoublePower(playerId, data);
                    break;
                case nameof(Ability.DrawExtraCard):
                    DrawExtraCard(playerId, data);
                    break;
                case nameof(Ability.DiscardOppRandomCard):
                    DestroyOppCard(playerId, data);
                    break;
                case nameof(Ability.DestroyOppPlayedCards):
                    DestroyAllOppCards(playerId, data);
                    break;
                default:
                    SendScore(playerId, data.power);
                    break;
            }
        }
        #region Abilities
        private void GainPoints(string playerId, CardData data)
        {
            SendScore(playerId, data.power + data.abilityData.value);
        }
        IEnumerator StealPoints(string playerId, CardData data)
        {
            var player = (PlayerSlot)System.Enum.Parse(typeof(PlayerSlot), playerId);

            var oppPlayer = player == PlayerSlot.Player1 ? PlayerSlot.Player2 : PlayerSlot.Player1;

            if (GameData.PlayerScores.TryGetValue(oppPlayer, out int score))
            {
                if (score > 0)
                {
                    SendScore(playerId, data.power + data.abilityData.value);
                    yield return new WaitForSeconds(0.5f);
                    SendScore(oppPlayer.ToString(), -data.abilityData.value);
                    //yield return new WaitForSeconds(3);
                }
            }

        }
        private void DoublePower(string playerId, CardData data)
        {
            SendScore(playerId, data.power + data.abilityData.value);
        }
        private void DrawExtraCard(string playerId, CardData data)
        {
            if (playerId != LocalPlayerContext.MySlot.ToString()) return;
            SendScore(playerId, data.power + data.abilityData.value);
            CardManager.Instance.GiveNextCards(data.abilityData.value);
        }
        private void DestroyOppCard(string playerId, CardData data)
        {
            var player = (PlayerSlot)System.Enum.Parse(typeof(PlayerSlot), playerId);

            var oppPlayer = player == PlayerSlot.Player1 ? PlayerSlot.Player2 : PlayerSlot.Player1;

            if (LocalPlayerContext.MySlot == oppPlayer)
            {
                CardManager.Instance.TakeOutRandomCard();
                SendScore(playerId, data.power);
            }
        }
        private void DestroyAllOppCards(string playerId, CardData data)
        {
            var player = (PlayerSlot)System.Enum.Parse(typeof(PlayerSlot), playerId);
            var oppPlayer = player == PlayerSlot.Player1 ? PlayerSlot.Player2 : PlayerSlot.Player1;

            Debug.Log($"Destroying all cards played by {oppPlayer}");

            // Mark opponent cards as destroyed (DO NOT REMOVE)
            if (playedCards.TryGetValue(oppPlayer.ToString(), out var oppCards))
            {
                foreach (var card in oppCards)
                {
                    destroyedCards.Add(card);
                }
            }

            // Clear visuals ONLY on owning client
            if (LocalPlayerContext.MySlot == oppPlayer)
            {
                CardManager.Instance.ClearPlayedCards();
            }

            // Score still applies
            SendScore(playerId, data.power);
        }
        #endregion
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
            //revealOrder.Clear();

            bool bothPlayedDestroy = DidBothPlayersPlayDestroy();

            if (bothPlayedDestroy)
            {
                //Debug.Log("Both players played DestroyOppPlayedCards");

            }
            var first = PlayerSlot.Player1.ToString();
            var second = PlayerSlot.Player2.ToString();
            playedCards.TryGetValue(first, out var firstCards);
            playedCards.TryGetValue(second, out var secondCards);

            firstCards ??= new List<CardData>();
            secondCards ??= new List<CardData>();

            AddDestroyCardsFirst(first, firstCards);
            AddDestroyCardsFirst(second, secondCards);

            firstCards.RemoveAll(IsDestroyCard);
            secondCards.RemoveAll(IsDestroyCard);



            int max = Mathf.Max(firstCards.Count, secondCards.Count);

            for (int i = 0; i < max; i++)
            {
                if (i < firstCards.Count)
                    revealOrder.Add((first, firstCards[i]));

                if (i < secondCards.Count)
                    revealOrder.Add((second, secondCards[i]));
            }

        }

        private void AddDestroyCardsFirst(string playerId, List<CardData> cards)
        {
            foreach (var card in cards)
            {
                if (IsDestroyCard(card))
                {
                    revealOrder.Add((playerId, card));
                }
            }
        }

        private bool IsDestroyCard(CardData card)
        {
            return card.abilityData != null &&
                   card.abilityData.type == Ability.DestroyOppPlayedCards;
        }

        private bool DidBothPlayersPlayDestroy()
        {
            bool p1HasDestroy = false;
            bool p2HasDestroy = false;

            if (playedCards.TryGetValue(PlayerSlot.Player1.ToString(), out var p1Cards))
            {
                p1HasDestroy = p1Cards.Exists(IsDestroyCard);
            }

            if (playedCards.TryGetValue(PlayerSlot.Player2.ToString(), out var p2Cards))
            {
                p2HasDestroy = p2Cards.Exists(IsDestroyCard);
            }

            return p1HasDestroy && p2HasDestroy;
        }

        public void EndRevealPhase()
        {
            IsRevealPhase = false;
            revealOrder.Clear();
            destroyedCards.Clear();
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
