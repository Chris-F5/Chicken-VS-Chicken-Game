using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    class ServerHandle
    {
        public static void WelcomeRecieved(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();

            Console.WriteLine($"{ServerManager.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}");
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }

            ServerSend.SendAllObjectsAsNew(_fromClient);
        }
        public static void UDPTestRecieved(int _fromClient, Packet _packet)
        {
            string _msg = _packet.ReadString();
            Console.WriteLine($"Recieved UDP test response: {_msg}");
        }
        public static void ButtonDown(int _fromClient, Packet _packet)
        {
            byte _key = _packet.ReadByte();
            Console.WriteLine(_key);
            if (_key == (byte)KeyButton.right)
            {
                ServerManager.clients[_fromClient].playerObject.rightKey = true;
            }
            else if (_key == (byte)KeyButton.right)
            {
                ServerManager.clients[_fromClient].playerObject.leftKey = true;
            }
            // TODO: Send key confirmation message
        }
        public static void ButtonUp(int _fromClient, Packet _packet)
        {
            Console.WriteLine("UP");
            byte _key = _packet.ReadByte();
            if (_key == (byte)KeyButton.right)
            {
                ServerManager.clients[_fromClient].playerObject.rightKey = false;
            }
            else if (_key == (byte)KeyButton.right)
            {
                ServerManager.clients[_fromClient].playerObject.leftKey = false;
            }
            // TODO: Send key confirmation message
        }
    }
}
