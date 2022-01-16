using Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Objects
{
    public class XiStrCarUnit : BinaryWriterExt.ISerializable
    {
        public uint CarID;
        public uint CarType;
        public uint BaseColor;
        public uint Grade;
        public uint SlotType;
        public uint AuctionCnt;
        public float Mitron;
        public float Kmh;

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(CarID);
            writer.Write(CarType);
            writer.Write(BaseColor);
            writer.Write(Grade);
            writer.Write(SlotType);
            writer.Write(AuctionCnt);
            writer.Write(Mitron);
            writer.Write(Kmh);
        }

        public static XiStrCarUnit Deserialize(BinaryReaderExt reader)
        {
            return new XiStrCarUnit()
            {
                CarID = reader.ReadUInt32(),
                CarType = reader.ReadUInt32(),
                BaseColor = reader.ReadUInt32(),
                Grade = reader.ReadUInt32(),
                SlotType = reader.ReadUInt32(),
                AuctionCnt = reader.ReadUInt32(),
                Mitron = reader.ReadInt32(),
                Kmh = reader.ReadInt32()
            };
        }

    }
}
