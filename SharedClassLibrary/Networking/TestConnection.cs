using System.Net;
using System.Net.Sockets;
using SharedClassLibrary.Logging;

namespace SharedClassLibrary.Networking
{
    public class TestConnection : Connection
    {
        public TestConnection(IPEndPoint _remoteEndPoint) : base(_remoteEndPoint) { }
        public TestConnection(TcpClient _client) : base(_client) { }

        public override void HandlePacket(Packet _packet)
        {
            Logger.LogDebug(_packet.ToString());
        }
    }
}
