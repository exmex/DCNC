using System.IO;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class LoadCharThreadAnswer : OutPacket
    {
        public uint ServerId;
        public uint ServerStartTime;
        public Character Character;
        public Vehicle[] Vehicles;

        public int CurrentCarId;
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.LoadCharThreadAck);
        }

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
                        /*if (vehicle.CarID == CurrentCarId) // Moved to Joining.cs
                            packet.Sender.User.ActiveCar = vehicle;*/
                        bs.Write(vehicle.CarID);
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
            
            /*
            ack.Writer.Write((uint) 0); // ServerId
            ack.Writer.Write((uint) 0); // ServerStartTime

            // Character
            character.Serialize(ack.Writer);

            ack.Writer.Write((uint) vehicles.Count);

            foreach (var vehicle in vehicles)
            {
                if (vehicle.CarID == character.CurrentCarId)
                    packet.Sender.User.ActiveCar = vehicle;
                ack.Writer.Write(vehicle.CarID);
                ack.Writer.Write(vehicle.CarType);
                ack.Writer.Write(vehicle.BaseColor);
                ack.Writer.Write(vehicle.Grade);
                ack.Writer.Write(vehicle.SlotType);
                ack.Writer.Write(vehicle.AuctionCnt);
                ack.Writer.Write(vehicle.Mitron);
                ack.Writer.Write(vehicle.Kmh);

                ack.Writer.Write(vehicle.Color);
                ack.Writer.Write(vehicle.Color2);
                ack.Writer.Write(vehicle.MitronCapacity);
                ack.Writer.Write(vehicle.MitronEfficiency);
                ack.Writer.Write(vehicle.AuctionOn);
                ack.Writer.Write(vehicle.SBBOn);
            }
            */
        }
    }
}