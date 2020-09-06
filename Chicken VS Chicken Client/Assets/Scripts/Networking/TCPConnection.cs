using System;
using System.Collections.Generic;
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
        private static List<byte> recievedData;

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

            recievedData = new List<byte>();

            stream.BeginRead(recieveBuffer, 0, dataBufferSize, RecieveCallback, null);
        }
        private static void RecieveCallback(IAsyncResult _result)
        {
            // This line waits for a pending asynchronous read to complete.
            int _byteLength = stream.EndRead(_result);
            if (_byteLength <= 0)
            {
                // TODO: disconect client
                Debug.LogWarning("TCP byte length was smaller than or equal to zero.");
                return;
            }
            else
            {
                byte[] _data = new byte[_byteLength];
                Array.Copy(recieveBuffer, _data, _byteLength);

                HandleData(_data);

                stream.BeginRead(recieveBuffer, 0, dataBufferSize, RecieveCallback, null);
            }
        }
        private static void HandleData(byte[] _data)
        {
            int packetLength = 0;

            recievedData.AddRange(_data);

            if (packetLength == 0 && recievedData.Count >= 4)
            {
                byte[] lengthBytes = recievedData.GetRange(0, 4).ToArray();

                packetLength = BitConverter.ToInt32(lengthBytes, 0);
                if (packetLength <= 0)
                {
                    Debug.LogWarning("Recieved TCP packet with claimed length of 0 or less.");
                    recievedData.Clear();
                    return;
                }
            }
            Debug.Log(packetLength);
            while (packetLength > 0 && packetLength <= recievedData.Count)
            {
                byte[] packetBytes = recievedData.GetRange(0, packetLength).ToArray();
                recievedData.RemoveRange(0, packetLength);

                Debug.Log("Handeling TCP 1");
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    Debug.Log("Handeling TCP 2");
                    PacketReader packet = new PacketReader(packetBytes);
                    NetworkManager.instance.HandlePacket(packet);
                });

                packetLength = 0;
                if (recievedData.Count >= 4)
                {
                    byte[] lengthBytes = recievedData.GetRange(0, 4).ToArray();

                    packetLength = BitConverter.ToInt32(lengthBytes, 0);
                    if (packetLength <= 0)
                    {
                        Debug.LogWarning("Recieved TCP packet with claimed length of 0 or less.");
                        recievedData.Clear();
                        return;
                    }
                }
            }
            if (packetLength <= 1)
            {
                recievedData.Clear();
            }
        }
        public static void SendData(byte[] _data)
        {
            stream.BeginWrite(_data, 0, _data.Length, null, null);
        }
    }
}
