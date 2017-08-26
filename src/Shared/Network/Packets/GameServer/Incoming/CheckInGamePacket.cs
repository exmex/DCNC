namespace Shared.Network.GameServer
{
    public class CheckInGamePacket
    {
        public uint Version;
        public int Ticket;
        public string Username;

        /*
        02 00 00 00 00 00 00 00 61 00 64 00 6D 00 69 00  · · · · · · · · a · d · m · i · 
        6E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  n · · · · · · · · · · · · · · · 
        00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  · · · · · · · · · · · · · · · · 
        00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  · · · · · · · · · · · · · · · · 
        00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  · · · · · · · · · · · · · · · · 
        00 00 00 00 00 00 00 00 72 00 61 00 00 00 00 00  · · · · · · · · r · a · · · · · 
        00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  · · · · · · · · · · · · · · · · 
        00 00 00 00 00 00  · · · · · ·
        
          unsigned int m_Version;
          unsigned int m_Ticket;
          int m_PcRoom;
          char m_crmcode[21]; 
        */
        public CheckInGamePacket(Packet packet)
        {
            Version = packet.Reader.ReadUInt32();
            Ticket = packet.Reader.Read();
            Username = packet.Reader.ReadUnicodeStatic(21);
        }
    }
}