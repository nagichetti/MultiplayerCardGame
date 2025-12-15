using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(fileName = "Game Data", menuName = "Game/Game Data")]
    public class GameData : ScriptableObject
    {
        public PlayerSlot PlayerId;
        public int PlayersNeeded = 2;
        public List<string> PlayerIds = new();
        public int Totalturns = 6;
        private int currentTurn = 0;
        public int PlayersJoined;
        private int remainingCost;
        private int myScore;
        private int oppScore;

        public int CurrentTurn { get => currentTurn; set { currentTurn = value; GameDataUpdated(); } }
        public int RemainingCost
        {
            get => remainingCost; set { remainingCost = value; GameDataUpdated(); }
        }
        public int MyScore { get => myScore; set { myScore = value; GameDataUpdated(); } }
        public int OppScore { get => oppScore; set { oppScore = value; GameDataUpdated(); } }

        public void ResetData()
        {
            PlayerIds = new();
            PlayersJoined = 0;
            CurrentTurn = 0;
            RemainingCost = 0;
        }

        public event Action OnUpdateGameData;
        public void GameDataUpdated()
        {
            OnUpdateGameData?.Invoke();
        }
    }
}
