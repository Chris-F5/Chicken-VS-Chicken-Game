using UnityEngine;
using System.Net;
using SharedClassLibrary.Networking;

namespace GameClient
{
    public class ClientHandle : MonoBehaviour
    {
        public static void RecievePing(Packet _packet)
        {
            byte _id = _packet.ReadByte();
            byte _myPing = _packet.ReadByte();
            ClientSend.PingRecieved(_id);
            Debug.Log($"Ping (measured in game ticks) : {_myPing}");
        }
        public static void Welcome(Packet _packet)
        {
            Debug.Log("test");
            byte _myId = _packet.ReadByte();
            Debug.Log($"test 2 {_myId}");
            string _msg = _packet.ReadString();
            Debug.Log("test 3");

            Debug.Log($"Welcome message form server: {_msg}");

            NetworkManager.instance.TcpConnectionConfirmed(_myId);

            ClientSend.WelcomeRecieved();
        }

        public static void Synchronise(Packet _packet)
        {
            while (_packet.UnreadLength() > 0)
            {
                short _objectId = _packet.ReadShort();
                short _typeId = _packet.ReadShort();
                Debug.Log($"Processing synchronise on object id: {_objectId} type: {_typeId}.");
                NetworkObject _networkObject = NetworkObjectManager.FindObject(_objectId);
                if (_networkObject == null)
                {
                    NetworkObjectManager.instance.HandleNewSynchroniser(_objectId, _typeId, _packet);
                }
                else
                {
                    _networkObject.HandleSynchronise(_packet);
                }
            }
        }
    }
}
