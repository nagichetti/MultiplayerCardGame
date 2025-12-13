using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace CardGame
{
    public class LanBroadcaster : MonoBehaviour
    {
        private UdpClient udp;
        private IPEndPoint endPoint;
        private float timer;

        private void Start()
        {
            udp = new UdpClient();
            udp.EnableBroadcast = true;
            endPoint = new IPEndPoint(IPAddress.Broadcast, 7777);
        }
        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= 1f)
            {
                BroadCast();
                timer = 0f;
            }
        }

        private void BroadCast()
        {
            string msg = "CARD_GAME_HOST";
            byte[] data = Encoding.UTF8.GetBytes(msg);
            udp.Send(data, data.Length, endPoint);
        }
    }
}
