using System.IO;
using Shared.Network.LobbyServer;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// PKTSIZE: Wrong Packet Size. CMD(467) CmdLen: : 240, AnalysisSize: 62
    /// sub_5402E0
    /// </summary>
    public class RoomNotifyChangeAnswer: OutPacket
    {
        public ushort Age; // ??? // <-- Long maybe?
        public ushort Serial; // ???
        
        public XiCarAttr CarAttr = new XiCarAttr();
        public XiPlayerInfo PlayerInfo = new XiPlayerInfo();
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.RoomNotifyChangeAck);
        }
        
        public override int ExpectedSize() => 240;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(Serial);
                    bs.Write(Age);
                    bs.Write(CarAttr.___u0.__s0.Sort);
                    bs.Write(CarAttr.___u0.__s0.Body);
                    bs.Write(CarAttr.___u0.__s0.Color);
                    bs.Write(CarAttr.___u0.__s1.lvalSortBody);
                    bs.Write(CarAttr.___u0.__s1.lvalColor);
                    bs.Write(CarAttr.___u0.llval);
                    bs.WriteUnicodeStatic(PlayerInfo.CharacterName, 21);
                    bs.Write(PlayerInfo.Serial);
                    bs.Write(PlayerInfo.Age);
                    bs.Write(PlayerInfo.CharacterId);
                    bs.Write(PlayerInfo.Level);
                    bs.Write(PlayerInfo.Exp);
                    bs.Write(PlayerInfo.TeamId);
                    bs.Write(PlayerInfo.TeamMarkId);
                    bs.WriteUnicodeStatic(PlayerInfo.TeamName, 14);
                    bs.Write(PlayerInfo.TeamNLevel);
                    bs.Write(PlayerInfo.VisualItem.Neon);
                    bs.Write(PlayerInfo.VisualItem.Plate);
                    bs.Write(PlayerInfo.VisualItem.Decal);
                    bs.Write(PlayerInfo.VisualItem.DecalColor);
                    bs.Write(PlayerInfo.VisualItem.AeroBumper);
                    bs.Write(PlayerInfo.VisualItem.AeroIntercooler);
                    bs.Write(PlayerInfo.VisualItem.AeroSet);
                    bs.Write(PlayerInfo.VisualItem.MufflerFlame);
                    bs.Write(PlayerInfo.VisualItem.Wheel);
                    bs.Write(PlayerInfo.VisualItem.Spoiler);
                    bs.Write(PlayerInfo.VisualItem.Reserve[0]);
                    bs.Write(PlayerInfo.VisualItem.Reserve[1]);
                    bs.Write(PlayerInfo.VisualItem.Reserve[2]);
                    bs.Write(PlayerInfo.VisualItem.Reserve[3]);
                    bs.Write(PlayerInfo.VisualItem.Reserve[4]);
                    bs.Write(PlayerInfo.VisualItem.Reserve[5]);
                    bs.WriteUnicodeStatic(PlayerInfo.VisualItem.PlateString, 9);
                    bs.Write(PlayerInfo.UseTime);
                }
                return ms.ToArray();
            }
            
            /*
            
            ack2.Writer.Write((ushort) 0); // Serial?
            ack2.Writer.Write((ushort) 0); // Age?

            ack2.Writer.Write((ushort) 0); // CarAttr Sort
            ack2.Writer.Write((ushort) 0); // CarAttr Body
            ack2.Writer.Write(new char[4]); // CarAttr Color
            ack2.Writer.Write(0); // CarAttr lvalSortBody
            ack2.Writer.Write(0); // CarAttr lvalColor
            ack2.Writer.Write((long) 0); // CarAttr llval

            ack2.Writer.WriteUnicodeStatic("Gigatoni", 21); // PlayerInfo Cname
            ack2.Writer.Write((ushort) 5); // PlayerInfo serial
            ack2.Writer.Write((ushort) 0); // PlayerInfo age
            ack2.Writer.Write((long) 4); // PlayerInfo Cid
            ack2.Writer.Write((ushort) 1); // PlayerInfo Level
            ack2.Writer.Write((uint) 0); // PlayerInfo Exp
            ack2.Writer.Write((long) 0); // PlayerInfo TeamId
            ack2.Writer.Write((long) 0); // PlayerInfo TeamMarkId
            ack2.Writer.WriteUnicodeStatic("Staff", 14); // PlayerInfo TeamName
            ack2.Writer.Write((ushort) 1); // PlayerInfo teamNLevel

            ack2.Writer.Write((ushort) 1); // VisualItem Neon
            ack2.Writer.Write((ushort) 1); // VisualItem Plate
            ack2.Writer.Write((ushort) 1); // VisualItem Decal
            ack2.Writer.Write((ushort) 1); // VisualItem DecalColor
            ack2.Writer.Write((ushort) 1); // VisualItem AeroBumper
            ack2.Writer.Write((ushort) 1); // VisualItem AeroIntercooler
            ack2.Writer.Write((ushort) 1); // VisualItem AeroSet
            ack2.Writer.Write((ushort) 1); // VisualItem MufflerFlame
            ack2.Writer.Write((ushort) 1); // VisualItem Wheel
            ack2.Writer.Write((ushort) 1); // VisualItem Spoiler

            ack2.Writer.Write((ushort) 0); // VisualItem Reserved?
            ack2.Writer.Write((ushort) 0); // VisualItem Reserved?
            ack2.Writer.Write((ushort) 0); // VisualItem Reserved?
            ack2.Writer.Write((ushort) 0); // VisualItem Reserved?
            ack2.Writer.Write((ushort) 0); // VisualItem Reserved?
            ack2.Writer.Write((ushort) 0); // VisualItem Reserved?

            ack2.Writer.WriteUnicodeStatic("HELLO", 9); // VisualItem PlateString

            ack2.Writer.Write(100.0f); // PlayerInfo UseItem   
            */
        }
    }
}