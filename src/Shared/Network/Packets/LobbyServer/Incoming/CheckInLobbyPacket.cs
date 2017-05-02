using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Network.LobbyServer
{
    public class CheckInLobbyPacket
    {
        public uint ProtocolVersion;
        public uint Ticket;
        public string Username;
        public uint Time;
        public string StringTicket;

        public CheckInLobbyPacket(Packet packet)
        {
            ProtocolVersion = packet.Reader.ReadUInt32();
            Ticket = packet.Reader.ReadUInt32();
            Username = packet.Reader.ReadUnicodeStatic(0x28);
            Time = packet.Reader.ReadUInt32();
            StringTicket = packet.Reader.ReadAsciiStatic(0x40);
        }
    }
}