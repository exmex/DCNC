using System.IO;
using System.Net.Configuration;
using Shared.Network.AreaServer;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class GetDateTimePacketAnswer : OutPacket
    {
        public uint Action;
        public uint GlobalTime;
        public uint LocalTime;
        public int TotalSeconds;
        public int ServerTickTime;
        public int ServerTick;
        public short DayOfYear;
        public short Month;
        public short Day;
        public short DayOfWeek;
        public byte Hour;
        public byte Minute;
        public byte Second;

        public GetDateTimePacketAnswer()
        {
            ServerTickTime = 0;
            ServerTick = 0;
        }

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.GetDateTimeAck);
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(Action);
                    bs.Write(GlobalTime);
                    bs.Write(LocalTime);
                    bs.Write(TotalSeconds);
                    bs.Write(ServerTickTime);
                    bs.Write(ServerTick);
                    bs.Write(DayOfYear);
                    bs.Write(Month);
                    bs.Write(Day);
                    bs.Write(DayOfWeek);
                    bs.Write(Hour);
                    bs.Write(Minute);
                    bs.Write(Second);
                }
                return ms.GetBuffer();
            }
        }
    }
}