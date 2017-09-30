using System.IO;
using System.Numerics;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class MyCityPositionAnswer : OutPacket
    {
        public int City;
        public int LastChannel;
        public Vector4 Position;
        public int PositionState;
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.MyCityPositionAck);
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(City);
                    bs.Write(LastChannel);
                    bs.Write(Position);
                    bs.Write(PositionState);
                }
                return ms.ToArray();
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