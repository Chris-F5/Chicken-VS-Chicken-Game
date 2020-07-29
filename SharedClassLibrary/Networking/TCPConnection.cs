using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SharedClassLibrary.Networking
{
    internal class TCPConnection
    {
        public static int dataBufferSize = 4096;
        public static bool IsListening { get { return listener != null; } }

        private static TcpListener listener;
        private static Connection.NewConnectionHandler newConnectionHandler;

        private TcpClient socket;
        private NetworkStream stream;

        private byte[] recieveBuffer;
        private Packet recievedData;
        private Connection.PacketHandler packetHandler;

        public TCPConnection(Connection.PacketHandler _packetHandler)
        {
            packetHandler = _packetHandler;
        }

        public void Connect(IPAddress _remoteIp, int _remotePort)
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            recieveBuffer = new byte[dataBufferSize];
            socket.BeginConnect(_remoteIp, _remotePort, RecieveCallback, null);
        }
        public void AcceptConnection(TcpClient _socket)
        {
            socket = _socket;
            socket.ReceiveBufferSize = dataBufferSize;
            socket.SendBufferSize = dataBufferSize;
            stream = socket.GetStream();

            recievedData = new Packet();
            recieveBuffer = new byte[dataBufferSize];

            stream.BeginRead(recieveBuffer, 0, dataBufferSize, RecieveCallback, null);
        }

        private void RecieveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    // TODO: disconect client
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
            catch (Exception _ex)
            {
                Console.WriteLine($"Error recieving TCP data: {_ex}");
                //TODO: disconect client
            }
        }

        public void Send(Packet _packet, bool _writeLength = true)
        {
            try
            {
                if (socket != null)
                {
                    if (_writeLength)
                    {
                        _packet.WriteLength();
                    }
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error sending packet to player via TCP: {_ex}");
                //TODO: disconect client
            }
        }


        private bool HandleData(byte[] _data)
        {
            int _packetLength = 0;

            recievedData.SetBytes(_data);

            if (recievedData.UnreadLength() >= 4)
            {
                _packetLength = recievedData.ReadInt();
                if (_packetLength <= 0)
                {
                    return true;
                }
            }

            while (_packetLength > 0 && _packetLength <= recievedData.UnreadLength())
            {
                byte[] _packetBytes = recievedData.ReadBytes(_packetLength);
                using (Packet _packet = new Packet(_packetBytes))
                {
                    packetHandler(_packet);
                }

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

        public static void StartListening(int _port, Connection.NewConnectionHandler _newConnectionHandler)
        {
            newConnectionHandler = _newConnectionHandler; 
            listener = new TcpListener(IPAddress.Any, _port);
            listener.Start();
            listener.BeginAcceptTcpClient(new AsyncCallback(ConnectCallback), null);
            Console.WriteLine($"Started listening for TCP on port {_port}.");
        }
        public static void ConnectCallback(IAsyncResult _result)
        {
            TcpClient _client = listener.EndAcceptTcpClient(_result);
            listener.BeginAcceptTcpClient(new AsyncCallback(ConnectCallback), null);

            Console.WriteLine($"Incoming TCP connection from {_client.Client.RemoteEndPoint}.");

            newConnectionHandler(_client);
        }
    }
}
