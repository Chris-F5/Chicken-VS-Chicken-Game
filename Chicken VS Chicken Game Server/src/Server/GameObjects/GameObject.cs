using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    enum ObjectTypes
    {
        player,
        wall,
        testObject
    }

    abstract class GameObject
    {
        public static Dictionary<short, GameObject> allObjects = new Dictionary<short, GameObject>();

        public readonly byte typeId;
        public short objectId { get; private set; }

        public GameObject(byte _typeId)
        {
            typeId = _typeId;
            if (SetObjectID())
            {
                Console.WriteLine($"Object (tpye ID = {typeId}, object ID = {objectId}) sucessfully created.");
                allObjects.Add(objectId, this);
            }
        }
        public abstract void Update(Packet _packet);
        public void SendNewObjectPacket()
        {
            using (Packet _packet = new Packet((int)ServerPackets.newGameObject))
            {
                SetNewObjectPacketContent(_packet);

                // Both TCP and UDP are sent to make the game object create quickly and to ensure it is created
                UDP.SendToAll(_packet);
                TCP.SendToAll(_packet);
            }
        }
        public void SendNewObjectPacket(Client _toClient)
        {
            using (Packet _packet = new Packet((int)ServerPackets.newGameObject))
            {
                SetNewObjectPacketContent(_packet);

                // Both TCP and UDP are sent to make the game object create quickly and to ensure it is created
                _toClient.udp.Send(_packet);
                _toClient.tcp.Send(_packet);
            }
        }
        public virtual void SetNewObjectPacketContent(Packet _packet)
        {
            _packet.WriteByte(typeId);
            _packet.WriteShort(objectId);
        }

        public virtual void Destroy()
        {
            // TODO: tell client object was destroyed
            // TODO: dispose object so it wont leak memory
        }

        private bool SetObjectID()
        {
            for (short i = 0; i <= short.MaxValue; i++)
            {
                if (!allObjects.ContainsKey(i))
                {
                    objectId = i;
                    break;
                }
            }
            if (objectId == short.MaxValue)
            {
                Console.WriteLine($"CRITICAL WARNING : max game object count ({short.MaxValue}) reached - destroying game object");
                // TODO: change this destroy method to the dispose method that I will impliment in future
                this.Destroy();
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
