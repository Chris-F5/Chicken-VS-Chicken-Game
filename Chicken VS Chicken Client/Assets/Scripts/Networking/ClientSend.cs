using UnityEngine;
using SharedClassLibrary.Networking;

namespace GameClient
{
    public class ClientSend : MonoBehaviour
    {
        public static void PingRecieved(byte _id)
        {
            using (Packet _packet = new Packet((byte)ClientPacketIds.pingRespond))
            {
                _packet.WriteByte(_id);

                NetworkManager.instance.SendUdp(_packet);
            }
        }

        public static void WelcomeRecieved()
        {
            using (Packet _packet = new Packet((byte)ClientPacketIds.welcomeReceived))
            {
                _packet.WriteInt(NetworkManager.instance.myId);
                _packet.WriteString(UIManager.instance.usernameField.text);

                NetworkManager.instance.SendTcp(_packet);
            }
        }

        public static void UDPTestRecieved()
        {
            using (Packet _packet = new Packet((byte)ClientPacketIds.udpTestRecieve))
            {
                _packet.WriteString("Recieved the udp test");
                NetworkManager.instance.SendUdp(_packet);
            }
        }

        public static void ButtonDown(KeyButton _btn)
        {
            using (Packet _packet = new Packet((byte)ClientPacketIds.buttonDown))
            {
                _packet.WriteByte((byte)_btn);
                NetworkManager.instance.SendUdp(_packet);
            }
        }

        public static void ButtonUp(KeyButton _btn)
        {
            using (Packet _packet = new Packet((byte)ClientPacketIds.buttonUp))
            {
                _packet.WriteByte((byte)_btn);
                NetworkManager.instance.SendUdp(_packet);
            }
        }
    }
}
