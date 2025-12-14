using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(fileName = "Player Data", menuName = "Game/Player Data")]
    public class PlayerData : ScriptableObject
    {
        public int remainingTurns = 0;
    }
}
