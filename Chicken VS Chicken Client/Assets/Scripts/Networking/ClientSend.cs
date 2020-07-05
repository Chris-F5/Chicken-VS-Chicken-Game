using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameClient
{
    public class ClientSend : MonoBehaviour
    {
        public static void WelcomeRecieved()
        {
            using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
            {
                _packet.WriteInt(Client.instance.myId);
                _packet.WriteString(UIManager.instance.usernameField.text);

                SendTCPData(_packet);
            }
        }

        public static void UDPTestRecieved()
        {
            using (Packet _packet = new Packet((int)ClientPackets.udpTestRecieve))
            {
                _packet.WriteString("Recieved the udp test");
                SendUDPData(_packet);
            }
        }

        public static void ButtonDown(KeyButton _btn)
        {
            using (Packet _packet = new Packet((int)ClientPackets.buttonDown))
            {
                _packet.WriteByte((byte)_btn);
                SendUDPData(_packet);
            }
        }

        public static void ButtonUp(KeyButton _btn)
        {
            using (Packet _packet = new Packet((int)ClientPackets.buttonUp))
            {
                _packet.WriteByte((byte)_btn);
                SendUDPData(_packet);
            }
        }

        #region SendTo
        private static void SendTCPData(Packet _packet)
        {
            _packet.WriteLength();
            Client.instance.tcp.SendData(_packet);
        }

        private static void SendUDPData(Packet _packet)
        {
            if (Client.instance.myId != -1)
            {
                _packet.WriteLength();
                Client.instance.udp.SendData(_packet);
            }
            else
            {
                Debug.LogWarning("Canot send udp until tcp is connected.");
            }
        }
        #endregion
    }
}
