using UnityEngine;

namespace CardGame
{
    public class Player : MonoBehaviour
    {
        private NetworkPlayer m_networkPlayer;
        public void Bind(NetworkPlayer networkPlayer)
        {
            m_networkPlayer = networkPlayer;
        }
    }
}
