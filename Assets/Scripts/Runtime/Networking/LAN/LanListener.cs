using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace CardGame
{
    public class LanListener : MonoBehaviour
    {
        private UdpClient udp;
        private bool isConnected;
        private string ipAddress;

        private void Start()
        {
            udp = new UdpClient(GameNetworkManager.Port);
            udp.BeginReceive(OnReceive, null);
            StartCoroutine(WaitForIP());
        }

        private void OnReceive(IAsyncResult result)
        {
            if (isConnected) return;

            IPEndPoint ep = new IPEndPoint(IPAddress.Any, GameNetworkManager.Port);
            byte[] data = udp.EndReceive(result, ref ep);
            string msg = Encoding.UTF8.GetString(data);

            if(msg == "CARD_GAME_HOST")
            {
                isConnected = true;
                ipAddress = ep.Address.ToString();
            }
        }

        IEnumerator WaitForIP()
        {
            yield return new WaitUntil(()=> !string.IsNullOrEmpty(ipAddress));
            Connect(ipAddress);
        }

        private void Connect(string address)
        {
            LocalPlayerContext.Init();
            Debug.Log($"Trying to connect: {address}");
            UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.ConnectionData.Address = address;
            transport.ConnectionData.Port = GameNetworkManager.Port;

            NetworkManager.Singleton.StartClient();
        }


    }
}
