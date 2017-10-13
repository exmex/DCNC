using System;
using System.IO;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// sub_53C7A0
    /// </summary>
    public class LoadCharThreadAnswer : OutPacket
    {
        public uint ServerId;
        public uint ServerStartTime;
        public Character Character = new Character();
        public Vehicle[] Vehicles = new Vehicle[0];

        public int CurrentCarId;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.LoadCharThreadAck);
        }

        public override int ExpectedSize() => (50 * Vehicles.Length - 1) + 385;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(ServerId);
                    bs.Write(ServerStartTime);
                    Character.Serialize(bs);
                    bs.Write(Vehicles.Length);
                    foreach (var vehicle in Vehicles)
                    {
                        bs.Write(vehicle.CarId);
                        bs.Write(vehicle.CarType);
                        bs.Write(vehicle.BaseColor);
                        bs.Write(vehicle.Grade);
                        bs.Write(vehicle.SlotType);
                        bs.Write(vehicle.AuctionCnt);
                        bs.Write(vehicle.Mitron);
                        bs.Write(vehicle.Kmh);

                        bs.Write(vehicle.Color);
                        bs.Write(vehicle.Color2);
                        bs.Write(vehicle.MitronCapacity);
                        bs.Write(vehicle.MitronEfficiency);
                        bs.Write(vehicle.AuctionOn);
                        bs.Write(vehicle.SBBOn);
                    }
                }
                return ms.ToArray();
            }
        }
    }
}