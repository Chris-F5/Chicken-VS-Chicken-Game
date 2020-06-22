using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Welcome message form server: {_msg}");
        Client.instance.myId = _myId;

        ClientSend.WelcomeRecieved();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void UDPTest(Packet _packet)
    {
        string _msg = _packet.ReadString();
        Debug.Log($"Udp test message from server: {_msg}");
        ClientSend.UDPTestRecieved();
    }

    public static void NewGameObject(Packet _packet)
    {
        Debug.Log("Recieved new GameObject message from server.");
        ObjectManager.instance.NewGameObject(_packet);
    }

    public static void ObjectUpdates(Packet _packet)
    {
        while (_packet.UnreadLength() > 0) {
            short _objectId = _packet.ReadShort();
            ObjectManager.instance.HandleObjectUpdate(_objectId, _packet);
        }
    }
}
