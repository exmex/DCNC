using System.IO;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// sub_521CC00
    /// </summary>
    public class CheckStatAnswer : OutPacket
    {
        public int BasedSpeed;
        public int BasedDurability;
        public int BasedAcceleration;
        public int BasedBoost;
        
        public int EquipSpeed;
        public int EquipDurability;
        public int EquipAcceleration;
        public int EquipBoost;
        
        public int CharSpeed;
        public int CharDurability;
        public int CharAcceleration;
        public int CharBoost;

        public int ItemUseSpeed;
        public int ItemUseCrash;
        public int ItemUseAcceleration;
        public int ItemUseBoost;
        
        public int TotalSpeed;
        public int TotalDurability;
        public int TotalAcceleration;
        public int TotalBoost;
        
        // EnChantBonus
        public int Speed;
        public int Crash;
        public int Accel;
        public int Boost;
        public int AddSpeed;
        public float Drop;
        public float Exp;
        public float MitronCapacity;
        public float MitronEfficiency;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.StatUpdateAck);
        }
        
        public override int ExpectedSize() => 158;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(BasedSpeed);
                    bs.Write(BasedDurability);
                    bs.Write(BasedAcceleration);
                    bs.Write(BasedBoost);
                
                    bs.Write(EquipSpeed);
                    bs.Write(EquipDurability);
                    bs.Write(EquipAcceleration);
                    bs.Write(EquipBoost);
                
                    bs.Write(CharSpeed);
                    bs.Write(CharDurability);
                    bs.Write(CharAcceleration);
                    bs.Write(CharBoost);
        
                    bs.Write(ItemUseSpeed);
                    bs.Write(ItemUseCrash);
                    bs.Write(ItemUseAcceleration);
                    bs.Write(ItemUseBoost);
                
                    bs.Write(TotalSpeed);
                    bs.Write(TotalDurability);
                    bs.Write(TotalAcceleration);
                    bs.Write(TotalBoost);
                    // EnChantBonus
                    bs.Write(Speed);
                    bs.Write(Crash);
                    bs.Write(Accel);
                    bs.Write(Boost);
                    bs.Write(AddSpeed);
                    bs.Write(Drop);
                    bs.Write(Exp);
                    bs.Write(MitronCapacity);
                    bs.Write(MitronEfficiency);
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