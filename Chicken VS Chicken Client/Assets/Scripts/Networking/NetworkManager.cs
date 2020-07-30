using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedClassLibrary.Networking;
using System.Net;

namespace GameClient
{
    public class NetworkManager : MonoBehaviour
    {
        public static NetworkManager instance;

        NetworkManager()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                throw new System.Exception("Only on instance of NetworkManager can exist.");
            }

            remoteEndPoint = new IPEndPoint(IPAddress.Parse(remoteIp), remotePort);
        }

        private Connection connection;
        const string remoteIp = "127.0.0.1";
        const int remotePort = 25680;
        const int localPort = 25681;
        readonly IPEndPoint remoteEndPoint;

        public void ConnectToServer()
        {
            connection = new Connection(IPAddress.Parse(remoteIp), remotePort, new Connection.PacketHandler(HandlePacket));
            Connection.ListenForUDP(localPort, new Connection.UdpPacketHandler(HandleUdpPacket));
        }
        private void HandleUdpPacket(Packet _packet, IPEndPoint _endPoint)
        {
            if (IPEndPoint.Equals(_endPoint, remoteEndPoint))
            {
                HandlePacket(_packet);
            }
            else
            {
                Debug.LogWarning($"An unrecognised endpoint is sending UDP to port {localPort}.");
            }
        }
        public void HandlePacket(Packet _packet)
        {

        }
    }
}
