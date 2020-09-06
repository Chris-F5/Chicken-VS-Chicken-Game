using System.Collections.Generic;
using SharedClassLibrary.Networking;
using SharedClassLibrary.Simulation;

namespace GameClient
{
    class InputPacketWriter : PacketWriter
    {
        Dictionary<int, InputState> inputStates;
        public InputPacketWriter(Dictionary<int, InputState> _inputStates)
        {
            inputStates = _inputStates;
        }

        protected override void GenerateBufferContent()
        {
            WriteByte((byte)ClientPacketIds.inputs);
            foreach (KeyValuePair<int, InputState> inputState in inputStates)
            {
                int tick = inputState.Key;
                WriteInt(tick);
                WriteInputState(inputState.Value);
                InsertInt(buffer.Count);
            }
        }
    }
}
