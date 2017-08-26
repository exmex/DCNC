using System.IO;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class FirstPositionAnswer : OutPacket
    {
        public int City;
        public int LastChannel;
        public float PositionX;
        public float PositionY;
        public float PositionZ;
        public float Rotation;
        public int PositionState;
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.FirstPositionAck);
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(City);
                    bs.Write(LastChannel);
                    bs.Write(PositionX);
                    bs.Write(PositionY);
                    bs.Write(PositionZ);
                    bs.Write(Rotation);
                    bs.Write(PositionState);
                }
                return ms.GetBuffer();
            }
        }
        
        /*
        ack.Writer.Write(packet.Sender.User.ActiveCharacter.City); // City ID
        ack.Writer.Write(packet.Sender.User.ActiveCharacter.LastChannel); // Channel ID
        ack.Writer.Write(packet.Sender.User.ActiveCharacter.PositionX); // x
        ack.Writer.Write(packet.Sender.User.ActiveCharacter.PositionY); // y
        ack.Writer.Write(packet.Sender.User.ActiveCharacter.PositionZ); // z
        ack.Writer.Write(packet.Sender.User.ActiveCharacter.Rotation); // w
        ack.Writer.Write(packet.Sender.User.ActiveCharacter.posState); // PosState
        */
    }
}