using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using SharedClassLibrary.Networking;
using SharedClassLibrary.Simulation;
using SharedClassLibrary.Simulation.NetworkObjects;

namespace GameServer
{
    class Client
    {
        public const byte maxPlayerCount = 8;
        /// <summary>
        /// The number of previous consecutive ticks of input state to send clients every tick.
        /// </summary>
        public const int extraInputsSend = 1;

        private static Dictionary<byte, Client> allClients = new Dictionary<byte, Client>();
        private delegate void PacketHandler(Client _fromClient, PacketReader _packet);
        private static Dictionary<byte, PacketHandler> packetHandlers = new Dictionary<byte, PacketHandler>()
        {
            { (byte) ClientPacketIds.welcomeRespond, ServerHandle.WelcomeRecieved},
            { (byte) ClientPacketIds.inputs, ServerHandle.InputsRecieved},
        };

        public static bool canAcceptNewClient {
            get
            {
                for (byte i = 0; i < maxPlayerCount; i++)
                {
                    if (!allClients.ContainsKey(i))
                    {
                        return true;
                    }
                }
                return false;
            } 
        }

        public static Client GetClient(byte _id)
        {
            return allClients[_id];
        }

        public readonly byte id;
        private readonly TCP tcp;
        private readonly UDP udp;
        private PlayerController playerController;

        public string playerName { get; set; }

        public bool IsUDPConnected
        {
            get
            {
                return (udp.endPoint != null);
            }
        }

        public Client(TcpClient _tcpSocket)
        {
            bool foundId = false;
            for (byte i = 0; i < maxPlayerCount; i++)
            {
                if (!allClients.ContainsKey(i))
                {
                    foundId = true;

                }
            }
            if (!foundId)
            {
                throw new Exception("Max client count exceeded");
            }

            tcp = new TCP(this);
            udp = new UDP(this);
            playerController = new PlayerController(id);
            tcp.Connect(_tcpSocket);
            allClients.Add(id, this);
        }
        public void TCPConnect(TcpClient _socket)
        {
            tcp.Connect(_socket);
            CreatePlayer();
        }
        public void UDPConnect(IPEndPoint _endPoint)
        {
            // TODO : Check this end point is equal to the tcp one.
            udp.Connect(_endPoint);
        }
        /// <summary>
        /// Checks if the given ip end point is allowed to send udp data to this client.
        /// </summary>
        public bool VerifyEndPoint(IPEndPoint _iPEndPoint)
        {
            return (_iPEndPoint.ToString() == udp.endPoint.ToString());
        }
        private void CreatePlayer()
        {
            new Player(playerController, new Vector2(0, 10));
            Console.WriteLine("Player object created.");
        }
        public InputState? GetInputState(int _tick)
        {
            return playerController.GetState(_tick);
        }

        public void HandleUdpPacket(PacketReader _packet)
        {
            udp.HandlePacket(_packet);
        }

        public void HandlePacket(PacketReader _packet)
        {
            byte _packetId = _packet.ReadByte();
            packetHandlers[_packetId](this, _packet);
        }

        public void SetInputState(int _tick, InputState _state)
        {
            playerController.SetState(_tick, _state);
        }

        public void SendWelcome()
        {
            Console.WriteLine($"Sending welcome packet to client id: {id}");
            PacketWriter packet = new WelcomePacketWriter(id, GameLogic.Instance.startTime, "Welcome to the server");

            tcp.Send(packet.GetGeneratedBytes());
        }

        private void ShareClientInputs()
        {
            Dictionary<byte, Dictionary<int, InputState>> inputStates = new Dictionary<byte, Dictionary<int, InputState>>();

            foreach (Client client in allClients.Values)
            {
                Dictionary<int, InputState> clientInputStates = new Dictionary<int, InputState>();
                for (int tick = GameLogic.Instance.GameTick; tick >= GameLogic.Instance.GameTick - extraInputsSend; tick--) 
                {
                    InputState? input = client.GetInputState(tick);
                    if (input != null)
                    {
                        clientInputStates.Add(tick, input.Value);
                    }
                }
                if (clientInputStates.Count > 0)
                {
                    inputStates.Add(client.id, clientInputStates);
                }
            }

            PacketWriter packetWriter = new ShareInputStatesPacketWriter(inputStates);
            tcp.Send(packetWriter.GetGeneratedBytes());
        }

        public static void ShareClientInputsToAllClients()
        {
            foreach (Client client in allClients.Values)
            {
                client.ShareClientInputs();
            }
        }
    }
}