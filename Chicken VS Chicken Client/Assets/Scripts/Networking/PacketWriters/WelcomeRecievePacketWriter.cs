using System.Collections.Generic;
using SharedClassLibrary.Networking;
using SharedClassLibrary.Simulation;

namespace GameClient
{
    class WelcomeRecievePacketWriter : PacketWriter
    {
        int clientId;
        string playerName;
        public WelcomeRecievePacketWriter(int _clientId, string _playerName)
        {
            clientId = _clientId;
            playerName = _playerName;
        }

        protected override void GenerateBufferContent()
        {
            WriteByte((byte)ClientPacketIds.welcomeRespond);
            WriteInt(clientId);
            WriteString(playerName);
            InsertInt(buffer.Count);
        }
    }
}
