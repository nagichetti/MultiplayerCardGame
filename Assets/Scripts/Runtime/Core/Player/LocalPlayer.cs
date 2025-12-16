using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public class LocalPlayer : Player
    {
        [SerializeField]
        private PlayerSlot slot;
        [SerializeField]
        private GameData GameData;

        [Header("Text")]
        [SerializeField]
        private TextMeshProUGUI m_remainingCostText;
        [SerializeField]
        private TextMeshProUGUI m_playerName;
        [SerializeField]
        private TextMeshProUGUI m_timer;
        [SerializeField]
        private TextMeshProUGUI m_score;

        [SerializeField]
        Button m_button;
        private void OnEnable()
        {
            slot = LocalPlayerContext.MySlot;
            GameData.OnUpdateGameData += GameData_OnUpdateGameData;
            m_button.onClick.AddListener(EndTurn);
        }

        private void EndTurn()
        {
            GameEvents.EndTurnPressed();
        }
        private void Update()
        {
            m_timer.text = "Time: " + GameData.remainingTime.ToString("0");
        }
        private void OnDisable()
        {
            GameData.OnUpdateGameData -= GameData_OnUpdateGameData;
            m_button.onClick.RemoveListener(EndTurn);
        }
        private void GameData_OnUpdateGameData()
        {
            m_playerName.text = LocalPlayerContext.MySlot.ToString();
            m_remainingCostText.text = GameData.RemainingCost.ToString();
            if(GameData.PlayerScores.TryGetValue(LocalPlayerContext.MySlot, out var score))
                m_score.text = score.ToString();
        }
    }
}
