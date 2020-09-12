using System;
using System.Collections.Generic;
using System.Text;
using SharedClassLibrary.GameLogic;

namespace SharedClassLibrary.Networking
{
    public abstract class PacketWriter
    {
        private List<byte> buffer;
        private byte[] bufferBytes;

        public byte[] GetGeneratedBytes(bool _regenerateBuffer = false)
        {
            if (_regenerateBuffer || buffer == null) {
                buffer = new List<byte>();
                GenerateBufferContent();
                bufferBytes = buffer.ToArray();
            }
            return bufferBytes;
        }

        protected abstract void GenerateBufferContent();

        protected void Write(byte[] _value)
        {
            buffer.AddRange(_value);
        }
        protected void Insert(byte[] _value)
        {
            buffer.InsertRange(0, _value);
        }

        protected void WriteByte(byte _value)
        {
            Write(new byte[1] { _value });
        }

        protected void WriteShort(short _value)
        {
            Write(GetBytes(_value));
        }

        protected void WriteInt(int _value)
        {
            Write(GetBytes(_value));
        }
        protected void WriteLong(long _value)
        {
            Write(GetBytes(_value));
        }

        protected void WriteFloat(float _value)
        {
            Write(GetBytes(_value));
        }

        protected void WriteBool(bool _value)
        {
            Write(GetBytes(_value));
        }

        // TODO: Fix this. Idk make an extention method for byte[] or something.
        protected void WriteInputState(InputState _value)
        {
            WriteBool(_value.upKey);
            WriteBool(_value.downKey);
            WriteBool(_value.leftKey);
            WriteBool(_value.rightKey);
        }

        protected void InsertByte(byte _value)
        {
            Insert(new byte[1] { _value });
        }

        protected void WriteString(string _value)
        {
            WriteInt(_value.Length);
            Write(GetBytes(_value));
        }

        protected void InsertShort(short _value)
        {
            Insert(GetBytes(_value));
        }

        protected void InsertInt(int _value)
        {
            Insert(GetBytes(_value));
        }
        protected void InsertLong(long _value)
        {
            Insert(GetBytes(_value));
        }

        protected void InsertFloat(float _value)
        {
            Insert(GetBytes(_value));
        }

        protected void InsertBool(bool _value)
        {
            Insert(GetBytes(_value));
        }

        protected void InsertString(string _value)
        {
            Insert(GetBytes(_value));
            InsertInt(_value.Length);
        }

        private byte[] GetBytes(short _value)
        {
            return BitConverter.GetBytes(_value);
        }
        private byte[] GetBytes(int _value)
        {
            return BitConverter.GetBytes(_value);
        }
        private byte[] GetBytes(long _value)
        {
            return BitConverter.GetBytes(_value);
        }
        private byte[] GetBytes(float _value)
        {
            return BitConverter.GetBytes(_value);
        }
        private byte[] GetBytes(bool _value)
        {
            return BitConverter.GetBytes(_value);
        }
        private byte[] GetBytes(string _value)
        {
            return Encoding.ASCII.GetBytes(_value);
        }
    }
}
