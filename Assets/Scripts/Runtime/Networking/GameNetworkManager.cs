using Unity.Netcode;
using UnityEngine;
namespace CardGame
{
    public class GameNetworkManager : Singleton<GameNetworkManager>
    {
        [SerializeField]
        private LanBroadcaster lanBroadcaster;
        [SerializeField]
        private LanListener lanListener;

        public void StartServer()
        {
            NetworkManager.Singleton.StartHost();
            Instantiate(lanBroadcaster.gameObject);
        }
        public void StartCilent()
        {
            Instantiate(lanListener.gameObject);
        }
    }
}
