using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Network.AreaServer
{
    public class EnterAreaPacket
    {
        public readonly short SessionId;

        public readonly string Username;

        public readonly uint AreaId;

        public readonly uint LocalTime;

        public readonly uint GroupId;

        public EnterAreaPacket(Packet packet)
        {
            SessionId = packet.Reader.ReadInt16();

            Username = packet.Reader.ReadUnicode();
            Username = Username.Substring(Username.Length - 1); // Strip trailing nullbyte

            AreaId = packet.Reader.ReadUInt32();
            LocalTime = packet.Reader.ReadUInt32();
            GroupId = packet.Reader.ReadUInt32();
        }
    }
}
