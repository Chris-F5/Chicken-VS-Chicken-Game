using System.Collections.Generic;
using SharedClassLibrary.Networking;
using SharedClassLibrary.Simulation;

namespace GameServer
{
    class ShareInputStatesPacketWriter : PacketWriter
    {
        Dictionary<byte, Dictionary<int, InputState>> inputStates;
        public ShareInputStatesPacketWriter(Dictionary<byte, Dictionary<int, InputState>> _inputStates)
        {
            inputStates = _inputStates;
        }

        protected override void GenerateBufferContent()
        {
            WriteByte((byte)ServerPacketIds.shareClientInputs);
            foreach (KeyValuePair<byte ,Dictionary<int, InputState>> clientInputState in inputStates)
            {
                WriteByte(clientInputState.Key);
                foreach (KeyValuePair<int, InputState> inputState in clientInputState.Value)
                {
                    int tick = inputState.Key;
                    WriteInt(tick);
                    WriteInputState(inputState.Value);
                }
                // End of this player info
                WriteInt(int.MaxValue);
            }
            InsertInt(buffer.Count);
        }
    }
}
