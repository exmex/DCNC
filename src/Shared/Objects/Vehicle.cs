using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Network;

namespace Shared.Objects
{
    public class Vehicle : ISerializable
    {
        public uint CarID;
        public uint CarType;
        public uint BaseColor;
        public uint Grade;
        public uint SlotType;
        public uint AuctionCnt;
        public float Mitron;
        public float Kmh;

        public float MitronEfficiency;
        public uint Color;
        public float MitronCapacity;

        public uint Color2;
        public bool AuctionOn;
        public bool SBBOn;


        public void Serialize(PacketWriter writer)
        {
            writer.Write(CarID);
            writer.Write(CarType);
            writer.Write(BaseColor);
            writer.Write(Grade);
            writer.Write(SlotType);
            writer.Write(AuctionCnt);
            writer.Write(Mitron);
            writer.Write(Kmh);
            writer.Write(Color);
            writer.Write(MitronCapacity);
            writer.Write(MitronEfficiency);
            writer.Write(AuctionOn);
        }
    }
}
