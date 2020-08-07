namespace SharedClassLibrary.Networking
{
    public enum ServerPacketIds
    {
        ping,
        welcome,
        synchronise,
    }

    public enum ClientPacketIds
    {
        pingRespond,
        welcomeReceived,
        udpTestRecieve,
        buttonDown,
        buttonUp
    }
}
