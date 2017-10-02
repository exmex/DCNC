using System;
using System.IO;
using Shared.Network.AreaServer;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// sub_529160
    /// PKTSIZE: Wrong Packet Size. CMD(661) CmdLen: : 1177, AnalysisSize: 831
    /// </summary>
    public class GameCharInfoAnswer : OutPacket
    {
        public Character Character;
        public Vehicle Vehicle;
        public XiStrStatInfo StatisticInfo;
        public Team Team;
        public uint Serial; // ushort (2)?
        public int LocType = 2;
        public char ChId;
        public ushort LocId;

        public GameCharInfoAnswer()
        {
            Character = new Character();
            Vehicle = new Vehicle();
            StatisticInfo = new XiStrStatInfo();
            Team = new Team();
        }

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.GameCharInfoAck);
        }
        
        public override int ExpectedSize() => 1177;

        /// <summary>
        /// 346 Bytes missing
        /// 
        /// packetId => 2
        /// field_0 => 4 (CharId?)
        /// field_1 => 155 (0x6->0xA1)
        /// field_2 => 4
        /// field_3 => 8 (0xA5->0xAD)
        /// field_4 => 4
        /// field_5 => 146 (0xB1->0x143) - Char Info end?! (321)
        /// -----------------------------------------------------
        /// field_6 => 50 (0x143->0x175) - VEHICLE INFO!! (50)
        /// -----------------------------------------------------
        /// field_7 => 112 (0x175->0x1E5) - Statistic Info
        /// -----------------------------------------------------
        /// field_8 => 4 - TeamId
        /// field_9 => 660 (0x1E9->0x47D) - Team
        /// -----------------------------------------------------
        /// field_10 => 12 (0x47D->0x489)
        /// field_11 => 6 (0x489->0x48F)
        ///     field_0 => 4
        ///     field_1 => 2
        /// field_12 => 6 (0x48F->0x495)
        ///     field_0 => 4
        ///     field_1 => 2
        /// field_13 => 4 - LocType
        /// 
        /// If LocType == 1 -> LocId == AreaId
        /// </summary>
        /// <returns></returns>
        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    Character.Serialize(bs); // 321 here, 363 in rice.
                    Vehicle.Serialize(bs);
                    StatisticInfo.Serialize(bs); // 112 vs 80 -> 32 bytes missing?
                    if (Team == null) // Character is in not team.
                        bs.Write(new byte[664]);
                    else
                        Team.Serialize(bs);
                    
                    // field_10
                    bs.Write(new byte[12]);
                    
                    // field_11
                    bs.Write((int)0);
                    bs.Write((short)0);
                    
                    // field_12
                    bs.Write((int)0);
                    bs.Write((short)0);
                    
                    bs.Write(LocType);
                }
                return ms.ToArray();
            }
        }
    }
}