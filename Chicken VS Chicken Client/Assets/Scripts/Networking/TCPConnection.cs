using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using SharedClassLibrary.Networking;

namespace GameClient
{
    static class TCPConnection
    {
        public static int dataBufferSize = 4096;

        private static IPEndPoint remoteIpEndPoint;
        private static TcpClient socket;
        private static NetworkStream stream;
        private static byte[] recieveBuffer;
        private static Packet recievedData;

        public static void Connect(IPEndPoint _remoteIpEndPoint)
        {
            remoteIpEndPoint = _remoteIpEndPoint;

            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            recieveBuffer = new byte[dataBufferSize];
            socket.BeginConnect(remoteIpEndPoint.Address, remoteIpEndPoint.Port, ConnectCallback, null);
        }
        private static void ConnectCallback(IAsyncResult _result)
        {
            socket.EndConnect(_result);
            if (!socket.Connected)
            {
                return;
            }

            stream = socket.GetStream();

            recievedData = new Packet();

            stream.BeginRead(recieveBuffer, 0, dataBufferSize, RecieveCallback, null);
        }
        private static void RecieveCallback(IAsyncResult _result)
        {
            Debug.Log("Recieved TCP data.");
            // This line is waiting for a pending asynchronous read to complete.
            int _byteLength = stream.EndRead(_result);
            if (_byteLength <= 0)
            {
                // TODO: disconect client
                Debug.LogWarning("Byte length was smaller than or equal to zero.");
                return;
            }
            else
            {
                byte[] _data = new byte[_byteLength];
                Array.Copy(recieveBuffer, _data, _byteLength);

                recievedData.Reset(HandleData(_data));

                stream.BeginRead(recieveBuffer, 0, dataBufferSize, RecieveCallback, null);
            }
        }
        private static bool HandleData(byte[] _data)
        {
            int _packetLength = 0;

            recievedData.SetBytes(_data);

            if (recievedData.UnreadLength() >= 4)
            {
                _packetLength = recievedData.ReadInt();
                if (_packetLength <= 0)
                {
                    Debug.LogWarning("Packet length was smaller than or equal to zero.");
                    return true;
                }
            }

            while (_packetLength > 0 && _packetLength <= recievedData.UnreadLength())
            {
                byte[] _packetBytes = recievedData.ReadBytes(_packetLength);

                Debug.Log("Recieved TCP packet.");

                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        NetworkManager.instance.HandlePacket(_packet);
                    }
                });

                _packetLength = 0;
                if (recievedData.UnreadLength() >= 4)
                {
                    _packetLength = recievedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true;
                    }
                }
            }
            if (_packetLength <= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void SendPacket(Packet _packet)
        {
            if (_packet == null)
            {
                throw new ArgumentNullException("_packet can't be null.");
            }
            _packet.WriteLength();
            stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
        }
    }
}
