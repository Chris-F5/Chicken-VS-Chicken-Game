using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using SharedClassLibrary.Networking;

namespace GameClient
{
    static class UDPConnection
    {
        private static IPEndPoint remoteIpEndPoint;
        private static UdpClient socket;

        public static void Connect(int _localPort, IPEndPoint _remoteIpEndPoint)
        {
            if (_remoteIpEndPoint == null)
            {
                throw new ArgumentNullException("_remoteIpEndPoint cant be null.");
            }

            remoteIpEndPoint = _remoteIpEndPoint;

            Debug.Log($"UDP connecting to {remoteIpEndPoint}");

            socket = new UdpClient(_localPort);
            socket.Connect(remoteIpEndPoint);
            socket.BeginReceive(RecieveCallback, null);

            using (Packet _packet = new Packet())
            {
                SendPacket(_packet);
            }
        }
        private static void RecieveCallback(IAsyncResult _result)
        {
            Debug.Log("Revieved UDP");

            // TODO: Check if the source of the end recieve can be trusted.
            byte[] _data = socket.EndReceive(_result, ref remoteIpEndPoint);
            socket.BeginReceive(RecieveCallback, null);

            if (_data.Length < 4)
            {
                // Something went wrong while transporting the packet. For now, it will be ignored.
                return;
            }
            else
            {
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_data))
                    {
                        int _packetLength = _packet.ReadInt();
                        if (_packetLength != _packet.UnreadLength())
                        {
                            Debug.LogWarning("Suggested udp packet length does not equal actual length. Ignoring packet.");
                            return;
                        }
                        NetworkManager.instance.HandlePacket(_packet);
                    }
                });
            }
        }
        public static void SendPacket(Packet _packet)
        {
            if (_packet == null)
            {
                throw new ArgumentNullException("_packet can't be null.");
            }
            if (remoteIpEndPoint == null)
            {
                throw new Exception("Cant send packet if not connected.");
            }

            _packet.WriteLength();
            _packet.InsertByte(NetworkManager.instance.myId);
            if (socket != null)
            {
                socket.BeginSend(_packet.ToArray(), _packet.Length(), null, null);
            }
        }
    }
}
