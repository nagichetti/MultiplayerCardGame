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
        public int PlayersJoined;

        public void ResetData()
        {
            PlayerIds = new();
            PlayersJoined = 0;
        }
    }
}
