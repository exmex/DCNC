using System.IO;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class CheckStatAnswer : OutPacket
    {
        public int CarSpeed;
        public int CarDurability;
        public int CarAcceleration;
        public int CarBoost;
        
        public int PartSpeed;
        public int PartDurability;
        public int PartAcceleration;
        public int PartBoost;
        
        public int UserSpeed;
        public int UserDurability;
        public int UserAcceleration;
        public int UserBoost;
        
        public int CharSpeed;
        public int CharDurability;
        public int CharAcceleration;
        public int CharBoost;

        public int ItemUseSpeed;
        public int ItemUseCrash;
        public int ItemUseAcceleration;
        public int ItemUseBoost;
        
        public int VehicleSpeed;
        public int VehicleDurability;
        public int VehicleAcceleration;
        public int VehicleBoost;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.StatUpdateAck);
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    // Speed (Car) Testvalue:100 -> http://i.imgur.com/AndRGwK.png
                    bs.Write(CarSpeed);
                    // Durability (Car) Testvalue:100 -> http://i.imgur.com/zuaxZu5.png
                    bs.Write(CarDurability);
                    // Acceleration (Car) Testvalue:100 -> http://i.imgur.com/97UkLkj.png
                    bs.Write(CarAcceleration);
                    // Boost (Car) Testvalue:100 -> http://i.imgur.com/FQ9EYVO.png
                    bs.Write(CarBoost);
                    
                    // Speed (Parts) Testvalue:300 -> http://i.imgur.com/FQ9EYVO.png
                    bs.Write(PartSpeed);
                    // Durability (Parts) Testvalue:400 -> http://i.imgur.com/FQ9EYVO.png
                    bs.Write(PartDurability);
                    // Acceleration (Parts) Testvalue:500 -> http://i.imgur.com/FQ9EYVO.png
                    bs.Write(PartAcceleration);
                    // Boost (Parts) Testvalue:600 -> http://i.imgur.com/FQ9EYVO.png
                    bs.Write(PartBoost);
                    
                    bs.Write(UserSpeed);
                    bs.Write(UserDurability);
                    bs.Write(UserAcceleration);
                    bs.Write(UserBoost);
                    
                    bs.Write(UserSpeed); // WTF?
                    bs.Write(UserDurability); // WTF?
                    bs.Write(UserAcceleration); // WTF?
                    bs.Write(UserBoost); // WTF?
                    
                    bs.Write(CharSpeed);
                    bs.Write(CharDurability);
                    bs.Write(CharAcceleration);
                    bs.Write(CharBoost);

                    bs.Write(ItemUseSpeed);
                    bs.Write(ItemUseCrash);
                    bs.Write(ItemUseAcceleration);
                    bs.Write(ItemUseBoost);

                    bs.Write(0); // Unknown1
                    bs.Write(0); // Unknown2
                    bs.Write(0); // Unknown3
                    bs.Write(0); // Unknown4
                    
                    // Vehicle Speed Testvalue:100 -> http://i.imgur.com/3GV9enQ.png
                    bs.Write(VehicleSpeed);
                    // Vehicle Durability Testvalue:200 -> http://i.imgur.com/3GV9enQ.png
                    bs.Write(VehicleDurability);
                    // Vehicle Acceleration Testvalue:300 -> http://i.imgur.com/3GV9enQ.png
                    bs.Write(VehicleAcceleration);
                    // Vehicle Boost Testvalue:400 -> http://i.imgur.com/3GV9enQ.png
                    bs.Write(VehicleBoost);
                    
                    
                    bs.Write(0); // Unknown5
                    bs.Write(0); // Unknown6
                    bs.Write(0); // Unknown7
                    bs.Write(0); // Unknown8
                    bs.Write(0); // Unknown9
                    bs.Write(0); // Unknown10
                    bs.Write((short)0); // Unknown11
                }
                return ms.ToArray();
            }
            /*ack.Writer.Write(0); // Speed (Car) Testvalue:100 -> http://i.imgur.com/AndRGwK.png
            ack.Writer.Write(0); // Durability (Car) Testvalue:100 -> http://i.imgur.com/zuaxZu5.png
            ack.Writer.Write(0); // Acceleration (Car) Testvalue:100 -> http://i.imgur.com/97UkLkj.png
            ack.Writer.Write(0); // Boost (Car) Testvalue:100 -> http://i.imgur.com/FQ9EYVO.png
            ack.Writer.Write(0); // Speed (Parts) Testvalue:300 -> http://i.imgur.com/FQ9EYVO.png
            ack.Writer.Write(0); // Durability (Parts) Testvalue:400 -> http://i.imgur.com/FQ9EYVO.png
            ack.Writer.Write(0); // Acceleration (Parts) Testvalue:500 -> http://i.imgur.com/FQ9EYVO.png
            ack.Writer.Write(0); // Boost (Parts) Testvalue:600 -> http://i.imgur.com/FQ9EYVO.png

            ack.Writer.Write(0); // Speed (User)
            ack.Writer.Write(0); // Durability (User)
            ack.Writer.Write(0); // Acceleration (User)
            ack.Writer.Write(0); // Boost (User)
            ack.Writer.Write(0); // Speed (User) WTF?
            ack.Writer.Write(0); // Durability (User) WTF?
            ack.Writer.Write(0); // Acceleration (User) WTF?
            ack.Writer.Write(0); // Boost (User) WTF?
            ack.Writer.Write(0); // Char Speed
            ack.Writer.Write(0); // Char Durability
            ack.Writer.Write(0); // Char Acceleration
            ack.Writer.Write(0); // Char Boost
            ack.Writer.Write(0); // int ItemUseSpeed;
            ack.Writer.Write(0); // int ItemUseCrash;
            ack.Writer.Write(0); // int ItemUseAccel;
            ack.Writer.Write(0); // int ItemUseBoost;
            ack.Writer.Write(0); // Unknown
            ack.Writer.Write(0); // Unknown
            ack.Writer.Write(0); // Unknown
            ack.Writer.Write(0); // Unknown
            ack.Writer.Write(0); // Vehicle Speed Testvalue:100 -> http://i.imgur.com/3GV9enQ.png
            ack.Writer.Write(0); // Vehicle Durability Testvalue:200 -> http://i.imgur.com/3GV9enQ.png
            ack.Writer.Write(0); // Vehicle Acceleration Testvalue:300 -> http://i.imgur.com/3GV9enQ.png
            ack.Writer.Write(0); // Vehicle Boost Testvalue:400 -> http://i.imgur.com/3GV9enQ.png
            ack.Writer.Write(0); // Unknown
            ack.Writer.Write(0); // Unknown
            ack.Writer.Write(0); // Unknown
            ack.Writer.Write(0); // Unknown
            ack.Writer.Write(0); // Unknown
            ack.Writer.Write(0); // Unknown
            ack.Writer.Write((short) 0); // Unknown
            */
        }
    }
}