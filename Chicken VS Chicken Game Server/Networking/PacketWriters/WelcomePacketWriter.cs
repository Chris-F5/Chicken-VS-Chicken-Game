using System;
using SharedClassLibrary.Networking;

namespace GameServer
{
    public class WelcomePacketWriter : PacketWriter
    {
        byte clientId;
        long gameLogicStartTime;
        string message;

        public WelcomePacketWriter(byte _clientId, DateTime _gameLogicStartTime, string _message)
        {
            clientId = _clientId;
            message = _message;
            gameLogicStartTime = _gameLogicStartTime.Ticks;
        }

        protected override void GenerateBufferContent()
        {
            WriteByte((byte)ServerPacketIds.welcome);
            WriteByte(clientId);
            WriteLong(gameLogicStartTime);
            WriteString(message);
            InsertInt(buffer.Count);
        }
    }
}
