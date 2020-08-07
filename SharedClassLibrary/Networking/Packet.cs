using System;
using System.Collections.Generic;
using System.Text;

namespace SharedClassLibrary.Networking
{
    /// <summary>Sent from server to client.</summary>
    public enum ServerPacketIds
    {
        ping,
        welcome,
        synchronise,
    }

    /// <summary>Sent from client to server.</summary>
    public enum ClientPacketIds
    {
        pingRespond,
        welcomeReceived,
        udpTestRecieve,
        buttonDown,
        buttonUp
    }

    public class Packet : IDisposable
    {
        private List<byte> buffer;

        private byte[] readableBuffer;
        private int readPos;
        public bool lengthWritten { get; private set; }

        /// <summary>Creates a new empty packet (without an ID).</summary>
        public Packet()
        {
            lengthWritten = false;
            buffer = new List<byte>();
            readPos = 0;
        }

        /// <summary>Creates a new packet with a given ID. Used for sending.</summary>
        /// <param name="_id">The packet ID.</param>
        public Packet(byte _id)
        {
            buffer = new List<byte>();
            readPos = 0;

            WriteByte((byte)_id);
        }

        /// <summary>Creates a packet from which data can be read. Used for receiving.</summary>
        /// <param name="_data">The bytes to add to the packet.</param>
        public Packet(byte[] _data)
        {
            buffer = new List<byte>();
            readPos = 0;

            SetBytes(_data);
        }

        #region Functions
        /// <summary>Sets the packet's content and prepares it to be read.</summary>
        public void SetBytes(byte[] _data)
        {
            WriteBytes(_data);
            readableBuffer = buffer.ToArray();
        }

        /// <summary>Inserts the length of the packet's content at the start of the buffer.</summary>
        public void WriteLength()
        {
            if (!lengthWritten)
            {
                lengthWritten = true;
                buffer.InsertRange(0, BitConverter.GetBytes(buffer.Count));
            }
            else
            {
                throw new Exception("Length can not be written to buffer twice.");
            }
        }

        /// <summary>Inserts the given int at the start of the buffer.</summary>
        public void InsertInt(int _value)
        {
            if (!lengthWritten)
            {
                buffer.InsertRange(0, BitConverter.GetBytes(_value));
            }
        }
        public void InsertByte(byte _value)
        {
            buffer.Insert(0, _value);
        }

        /// <summary>Gets the packet's content in array form.</summary>
        public byte[] ToArray()
        {
            readableBuffer = buffer.ToArray();
            return readableBuffer;
        }

        /// <summary>Gets the length of the packet's content.</summary>
        public int Length()
        {
            return buffer.Count;
        }

        /// <summary>Gets the length of the unread data contained in the packet.</summary>
        public int UnreadLength()
        {
            return Length() - readPos;
        }

        // TODO: Clean up this discusting function.
        /// <summary>Resets the packet instance to allow it to be reused.</summary>
        /// <param name="_shouldReset">Whether or not to reset the packet.</param>
        public void Reset(bool _shouldReset = true)
        {
            if (_shouldReset)
            {
                buffer.Clear();
                readableBuffer = null;
                readPos = 0;
            }
            else
            {
                // "Unread" the last read int
                readPos -= 4;
            }
        }
        #endregion

        #region Write Data

        /// <summary>Adds a byte to the packet.</summary>
        public void WriteByte(byte _value)
        {
            if (!lengthWritten)
            {
                buffer.Add(_value);
            }
        }
        /// <summary>Adds an array of bytes to the packet.</summary>
        public void WriteBytes(byte[] _value)
        {
            if (!lengthWritten)
            {
                buffer.AddRange(_value);
            }
        }
        /// <summary>Adds a short to the packet.</summary>
        public void WriteShort(short _value)
        {
            if (!lengthWritten)
            {
                buffer.AddRange(BitConverter.GetBytes(_value));
            }
        }
        /// <summary>Adds an int to the packet.</summary>
        public void WriteInt(int _value)
        {
            if (!lengthWritten)
            {
                buffer.AddRange(BitConverter.GetBytes(_value));
            }
        }
        /// <summary>Adds a long to the packet.</summary>
        public void WriteLong(long _value)
        {
            if (!lengthWritten)
            {
                buffer.AddRange(BitConverter.GetBytes(_value));
            }
        }
        /// <summary>Adds a float to the packet.</summary>
        public void WriteFloat(float _value)
        {
            if (!lengthWritten)
            {
                buffer.AddRange(BitConverter.GetBytes(_value));
            }
        }
        /// <summary>Adds a bool to the packet.</summary>
        public void WriteBool(bool _value)
        {
            if (!lengthWritten)
            {
                buffer.AddRange(BitConverter.GetBytes(_value));
            }
        }
        /// <summary>Adds a string to the packet.</summary>
        public void WriteString(string _value)
        {
            if (!lengthWritten)
            {
                WriteInt(_value.Length); // Add the length of the string to the packet
                buffer.AddRange(Encoding.ASCII.GetBytes(_value)); // Add the string itself
            }
        }
        #endregion

        #region Read Data
        /// <summary>Reads a byte from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public byte ReadByte(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                byte _value = readableBuffer[readPos];
                if (_moveReadPos)
                {
                    readPos += 1;
                }
                return _value;
            }
            else
            {
                throw new Exception("Could not read value of type 'byte'!");
            }
        }

        /// <summary>Reads an array of bytes from the packet.</summary>
        /// <param name="_length">The length of the byte array.</param>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public byte[] ReadBytes(int _length, bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                byte[] _value = buffer.GetRange(readPos, _length).ToArray();
                if (_moveReadPos)
                {
                    readPos += _length;
                }
                return _value;
            }
            else
            {
                throw new Exception("Could not read value of type 'byte[]'!");
            }
        }

        /// <summary>Reads a short from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public short ReadShort(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                short _value = BitConverter.ToInt16(readableBuffer, readPos);
                if (_moveReadPos)
                {
                    readPos += 2;
                }
                return _value;
            }
            else
            {
                throw new Exception("Could not read value of type 'short'!");
            }
        }

        /// <summary>Reads an int from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public int ReadInt(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                int _value = BitConverter.ToInt32(readableBuffer, readPos);
                if (_moveReadPos)
                {
                    readPos += 4;
                }
                return _value;
            }
            else
            {
                throw new Exception("Could not read value of type 'int'!");
            }
        }

        /// <summary>Reads a long from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public long ReadLong(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                long _value = BitConverter.ToInt64(readableBuffer, readPos);
                if (_moveReadPos)
                {
                    readPos += 8;
                }
                return _value;
            }
            else
            {
                throw new Exception("Could not read value of type 'long'!");
            }
        }

        /// <summary>Reads a float from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public float ReadFloat(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                float _value = BitConverter.ToSingle(readableBuffer, readPos);
                if (_moveReadPos)
                {
                    readPos += 4;
                }
                return _value;
            }
            else
            {
                throw new Exception("Could not read value of type 'float'!");
            }
        }

        /// <summary>Reads a bool from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public bool ReadBool(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                bool _value = BitConverter.ToBoolean(readableBuffer, readPos);
                if (_moveReadPos)
                {
                    readPos += 1;
                }
                return _value;
            }
            else
            {
                throw new Exception("Could not read value of type 'bool'!");
            }
        }

        /// <summary>Reads a string from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public string ReadString(bool _moveReadPos = true)
        {
            try
            {
                // Get the length of the string
                int _length = ReadInt();
                // TODO: Handle errors that are sometimes thrown on the following line. (idk null charachters or something)
                string _value = Encoding.ASCII.GetString(readableBuffer, readPos, _length);
                if (_moveReadPos && _value.Length > 0)
                {
                    // If _moveReadPos is true string is not empty
                    readPos += _length;
                }
                return _value;
            }
            catch
            {
                throw new Exception("Could not read value of type 'string'!");
            }
        }
        #endregion

        private bool disposed = false;

        protected virtual void Dispose(bool _disposing)
        {
            if (!disposed)
            {
                if (_disposing)
                {
                    buffer = null;
                    readableBuffer = null;
                    readPos = 0;
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
