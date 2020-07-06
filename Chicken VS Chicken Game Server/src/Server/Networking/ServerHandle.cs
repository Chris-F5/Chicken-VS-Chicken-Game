using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    class ServerHandle
    {
        public static void PingRespond(Client _fromClient, Packet _packet)
        {
            byte _id = _packet.ReadByte();
            _fromClient.ping = ServerManager.CalcluatePing(_id);
        }
        public static void WelcomeRecieved(Client _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();

            Console.WriteLine($"{_fromClient.tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient.id}");
            if (_fromClient.id != _clientIdCheck)
            {
                Console.WriteLine($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }

            _fromClient.NetworkSynchroniserStartup();
        }
        public static void UDPTestRecieved(Client _fromClient, Packet _packet)
        {
            string _msg = _packet.ReadString();
            Console.WriteLine($"Recieved UDP test response: {_msg}");
        }
        public static void ButtonDown(Client _fromClient, Packet _packet)
        {
            byte _key = _packet.ReadByte();
            if (_key == (byte)KeyButton.right)
            {
                _fromClient.playerObject.rightKey = true;
            }
            else if (_key == (byte)KeyButton.left)
            {
                _fromClient.playerObject.leftKey = true;
            }
            else if (_key == (byte)KeyButton.up)
            {
                _fromClient.playerObject.upKey = true;
            }
            // TODO: Send key confirmation message
        }
        public static void ButtonUp(Client _fromClient, Packet _packet)
        {
            byte _key = _packet.ReadByte();
            if (_key == (byte)KeyButton.right)
            {
                _fromClient.playerObject.rightKey = false;
            }
            else if (_key == (byte)KeyButton.left)
            {
                _fromClient.playerObject.leftKey = false;
            }
            else if (_key == (byte)KeyButton.up)
            {
                _fromClient.playerObject.upKey = false;
            }
            // TODO: Send key confirmation message
        }
    }
}
