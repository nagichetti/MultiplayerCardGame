using UnityEngine;

namespace CardGame
{
    public class RemoteCard : MonoBehaviour
    {
        [SerializeField]
        private Card m_card;

        private void OnEnable()
        {
            m_card.FaceDown();
        }
    }
}
