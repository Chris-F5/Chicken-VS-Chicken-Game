using System.Net;
using System.Net.Sockets;
using SharedClassLibrary.Logging;

namespace SharedClassLibrary.Networking
{
    public class TestConnection : Connection
    {
        TestConnection(IPAddress _remoteIp, int _remotePort) : base(_remoteIp, _remotePort) { }
        TestConnection(TcpClient _client) : base(_client) { }

        public override void HandlePacket(Packet _packet)
        {
            Logger.WriteLine(_packet.ToString());
        }
    }
}
