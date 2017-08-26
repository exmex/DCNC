using System.IO;
using Shared.Network.AreaServer;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class GameCharInfoAnswer : IOutPacket
    {
        public Character Character;
        public Vehicle Vehicle;
        public StatInfo StatisticInfo;
        public Team Team;
        public uint Serial;
        public char LocType;
        public char ChId;
        public ushort LocId = 1;
            
        public Packet CreatePacket()
        {
            var ack = new Packet(Packets.GameCharInfoAck);
            ack.Writer.Write(GetBytes());
            return ack;
        }

        public byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    Character.Serialize(bs);
                    Vehicle.Serialize(bs);
                    StatisticInfo.Serialize(bs);
                    Team.Serialize(bs);
                    bs.Write(Serial);
                    bs.Write(LocType);
                    bs.Write(ChId);
                    bs.Write(LocId);
                }
                return ms.GetBuffer();
            }
        }
    }
}