using TMPro;
using UnityEngine;

namespace CardGame
{
    public class RemotePlayer : Player
    {
        [SerializeField]
        private GameData GameData;
        [SerializeField]
        private TextMeshProUGUI m_remotePlayerName;
        [SerializeField]
        private TextMeshProUGUI m_scoreText;

        private void Awake()
        {
            GameData.OnUpdateGameData += GameData_OnUpdateGameData;
        }
        private void OnDestroy()
        {
            GameData.OnUpdateGameData -= GameData_OnUpdateGameData;
        }
        private void GameData_OnUpdateGameData()
        {
            var slot = LocalPlayerContext.MySlot == PlayerSlot.Player1 ? PlayerSlot.Player2 : PlayerSlot.Player1;

            m_remotePlayerName.text = slot.ToString();

            if (GameData.PlayerScores.TryGetValue(slot, out var score))
                m_scoreText.text = "Score: " + (GameData.PlayerScores[slot].ToString());
        }
    }
}
