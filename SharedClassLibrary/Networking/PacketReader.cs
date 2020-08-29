using System;
using System.Text;
using SharedClassLibrary.Simulation;

namespace SharedClassLibrary.Networking
{
    public class PacketReader
    {
        private byte[] buffer;
        private int readPos;

        public int unreadLength 
        {
            get
            {
                return buffer.Length - readPos;
            }
        }

        public PacketReader(byte[] _data)
        {
            readPos = 0;
            buffer = _data;
        }

        // TODO: Fix this mess
        public byte ReadByte(bool _moveReadPos = true)
        {
            if (buffer.Length > readPos)
            {
                byte value = buffer[readPos];
                if (_moveReadPos)
                {
                    readPos += 1;
                }
                return value;
            }
            else
            {
                throw new Exception("Could not read value of type 'byte'!");
            }
        }
        
        public InputState ReadInputState()
        {
            InputState state = new InputState();
            if (ReadBool())
            {
                state.upKey = true;
            }
            if (ReadBool())
            {
                state.leftKey = true;
            }
            if (ReadBool())
            {
                state.rightKey = true;
            }
            if (ReadBool())
            {
                state.downKey = true;
            }
            return state;
        }

        public short ReadShort(bool _moveReadPos = true)
        {
            if (buffer.Length > readPos)
            {
                short value = BitConverter.ToInt16(buffer, readPos);
                if (_moveReadPos)
                {
                    readPos += 2;
                }
                return value;
            }
            else
            {
                throw new Exception("Could not read value of type 'short'!");
            }
        }

        public int ReadInt(bool _moveReadPos = true)
        {
            if (buffer.Length > readPos)
            {
                int value = BitConverter.ToInt32(buffer, readPos);
                if (_moveReadPos)
                {
                    readPos += 4;
                }
                return value;
            }
            else
            {
                throw new Exception("Could not read value of type 'int'!");
            }
        }

        public long ReadLong(bool _moveReadPos = true)
        {
            if (buffer.Length > readPos)
            {
                long value = BitConverter.ToInt64(buffer, readPos);
                if (_moveReadPos)
                {
                    readPos += 8;
                }
                return value;
            }
            else
            {
                throw new Exception("Could not read value of type 'long'!");
            }
        }

        public float ReadFloat(bool _moveReadPos = true)
        {
            if (buffer.Length > readPos)
            {
                float value = BitConverter.ToSingle(buffer, readPos);
                if (_moveReadPos)
                {
                    readPos += 4;
                }
                return value;
            }
            else
            {
                throw new Exception("Could not read value of type 'float'!");
            }
        }

        public bool ReadBool(bool _moveReadPos = true)
        {
            if (buffer.Length > readPos)
            {
                bool value = BitConverter.ToBoolean(buffer, readPos);
                if (_moveReadPos)
                {
                    readPos += 1;
                }
                return value;
            }
            else
            {
                throw new Exception("Could not read value of type 'bool'!");
            }
        }

        public string ReadString(bool _moveReadPos = true)
        {
            try
            {
                // Get the length of the string
                int length = ReadInt();
                // TODO: Handle errors that are sometimes thrown on the following line. (idk null charachters or something)
                string value = Encoding.ASCII.GetString(buffer, readPos, length);
                if (_moveReadPos && value.Length > 0)
                {
                    readPos += length;
                }
                return value;
            }
            catch
            {
                throw new Exception("Could not read value of type 'string'!");
            }
        }
    }
}
