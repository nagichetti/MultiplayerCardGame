using System;
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

        public const Int32 Port = 4646;
        private bool broadCastSpawned, lanListenerSpawned;
        public void StartServer()
        {
            if (!broadCastSpawned)
            {
                Instantiate(lanBroadcaster.gameObject);
                broadCastSpawned = true;
            }
            NetworkManager.Singleton.StartHost();
        }
        public void StartCilent()
        {
            if (!lanListenerSpawned)
            {
                Instantiate(lanListener.gameObject);
                lanListenerSpawned = true;
            }
        }
    }
}
