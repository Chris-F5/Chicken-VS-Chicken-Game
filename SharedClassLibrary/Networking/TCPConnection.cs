using System;
using System.Net;
using System.Net.Sockets;
using SharedClassLibrary.Logging;

namespace SharedClassLibrary.Networking
{
    internal class TCPConnection
    {
        public static int dataBufferSize = 4096;
        public static bool IsListening { get { return listener != null; } }

        private static TcpListener listener;
        private static Connection.NewConnectionHandler newConnectionHandler;

        private readonly Connection connection;
        private readonly IPEndPoint remoteIpEndPoint;

        private TcpClient socket;
        private NetworkStream stream;

        private byte[] recieveBuffer;
        private Packet recievedData;

        public TCPConnection(Connection _connection, IPEndPoint _remoteIpEndPoint)
        {
            connection = _connection;
            remoteIpEndPoint = _remoteIpEndPoint;
        }

        public void SendPacket(Packet _packet)
        {
            if (_packet == null)
            {
                throw new ArgumentNullException("_packet can't be null.");
            }
            _packet.WriteLength();
            stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
        }

        public void Connect()
        {
            Logger.LogDebug("TCPConnection is connecting...");

            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            recieveBuffer = new byte[dataBufferSize];
            socket.BeginConnect(remoteIpEndPoint.Address, remoteIpEndPoint.Port, ConnectCallback, null);
        }
        private void ConnectCallback(IAsyncResult _result)
        {
            Logger.LogDebug("Connect callback 1");
            socket.EndConnect(_result);
            Logger.LogDebug("Connect callback 2");
            if (!socket.Connected)
            {
                Logger.LogDebug("Connect callback canceled");
                return;
            }

            stream = socket.GetStream();

            recievedData = new Packet();

            stream.BeginRead(recieveBuffer, 0, dataBufferSize, RecieveCallback, null);
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
            Logger.LogDebug("Recieved TCP data.");
            Logger.LogDebug("0");
            // This line is waiting for a pending asynchronous read to complete.
            int _byteLength = stream.EndRead(_result);
            Logger.LogDebug("1");
            if (_byteLength <= 0)
            {
                // TODO: disconect client
                Logger.LogWarning("Byte length was smaller than or equal to zero.");
                Logger.LogDebug("3");
                return;
            }
            else
            {
                Logger.LogDebug("2");
                byte[] _data = new byte[_byteLength];
                Array.Copy(recieveBuffer, _data, _byteLength);

                recievedData.Reset(HandleData(_data));

                stream.BeginRead(recieveBuffer, 0, dataBufferSize, RecieveCallback, null);
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
                    Logger.LogWarning("Packet length was smaller than or equal to zero.");
                    return true;
                }
            }

            Logger.LogDebug($"Packet length: {_packetLength} / {recievedData.UnreadLength()}");

            while (_packetLength > 0 && _packetLength <= recievedData.UnreadLength())
            {
                byte[] _packetBytes = recievedData.ReadBytes(_packetLength);

                Logger.LogDebug("Recieved TCP packet.");

                using (Packet _packet = new Packet(_packetBytes))
                {
                    connection.HandlePacket(_packet);
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
            if (!IsListening)
            {
                listener = new TcpListener(IPAddress.Any, _port);
                listener.Start();
                listener.BeginAcceptTcpClient(new AsyncCallback(NewConnectionCallback), null);
                Logger.LogDebug($"Started listening for TCP on port {_port}.");
            }
        }
        public static void NewConnectionCallback(IAsyncResult _result)
        {
            TcpClient _client = listener.EndAcceptTcpClient(_result);
            listener.BeginAcceptTcpClient(new AsyncCallback(NewConnectionCallback), null);

            Logger.LogDebug($"Incoming TCP connection from {_client.Client.RemoteEndPoint}.");

            newConnectionHandler(_client);
        }
    }
}
