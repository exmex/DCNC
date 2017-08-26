namespace Shared.Network
{
    public interface IOutPacket
    {
        Packet CreatePacket();
        byte[] GetBytes();
    }
}