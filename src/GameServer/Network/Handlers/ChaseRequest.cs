using System.Numerics;
using Shared.Network;
using Shared.Network.GameServer;

namespace GameServer.Network.Handlers
{
    public class ChaseRequest
    {
        [Packet(Packets.CmdChaseRequest)]
        public static void Handle(Packet packet)
        {
            /*
            [Debug] - ChaseRequest: 000000: 01 9A 45 91 45 66 4A AA 45 A6 52 E7 40 00 00 00  · · E · E f J · E · R · @ · · ·
            000016: 00  ·

            [Info] - Received ChaseRequest (id 189, 0xBD) on 11021.
            */

            var chaseRequestPacket = new ChaseRequestPacket(packet);

            var ack = new ChaseRequestAnswer
            {
                StartPos = new Vector4(chaseRequestPacket.PosX, chaseRequestPacket.PosY, chaseRequestPacket.PosZ, chaseRequestPacket.Rot),
                EndPos = new Vector4(chaseRequestPacket.PosX, chaseRequestPacket.PosY, chaseRequestPacket.PosZ, chaseRequestPacket.Rot),
                CourseId = 0,
                Type = 2 - (chaseRequestPacket.BNow ? 1 : 0),
                PosName = "test",
                FirstHuvLevel = 1,
                FirstHuvId = 10001
            };

            //Wrong Packet Size. CMD(186) CmdLen: : 252, AnalysisSize: 250
            /*var ack = new Packet(Packets.ChasePropose);
            ack.Writer.Write((ushort) 0);
            ack.Writer.Write(chaseRequestPacket.PosX); // Start X
            ack.Writer.Write(chaseRequestPacket.PosY); // Start Y
            ack.Writer.Write(chaseRequestPacket.PosZ); // Start Z
            ack.Writer.Write(chaseRequestPacket.Rot); // Start W

            ack.Writer.Write(chaseRequestPacket.PosX); // End X
            ack.Writer.Write(chaseRequestPacket.PosY); // End Y
            ack.Writer.Write(chaseRequestPacket.PosZ); // End Z

            ack.Writer.Write(0); // CourseId
            ack.Writer.Write(2 - (chaseRequestPacket.BNow ? 1 : 0)); // Type?
            ack.Writer.WriteUnicodeStatic("test", 100);

            ack.Writer.Write(1); // HUV first level
            ack.Writer.Write(10001); // HUV first Id
            ack.Writer.Write(new byte[2]); // Not sure.
            */
            packet.Sender.Send(ack.CreatePacket());
        }
    }
}