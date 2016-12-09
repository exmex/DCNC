using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Network;
using Shared.Util;

namespace Shared.Objects
{
    public class Character
    {
        public User User;
        public ulong Uid;
        public ulong Cid;

        public string Name;
        public int CreationDate;
        public ushort Avatar;
        public ushort Level;
        public int City;
        public int CurrentCarId;
        public int GarageLevel;
        public int InventoryLevel;
        public long Tid;
        
        public int posState;

        public ulong CID;
        public string LastMessageFrom; // 0xB
        public int LastDate;

        public long CurExp;
        public long NextExp;
        public long BaseExp;

        public long MitoMoney;
        public long TeamId;
        public long TeamMarkId;
        public string TeamName;		 // 0xD / 13
        public int TeamRank;
        public byte PType;
        public uint PvpCnt;
        public uint PvpWinCnt;
        public uint PvpPoint;
        public uint TPvpCnt;
        public uint TPvpWinCnt;
        public uint TPvpPoint;
        public uint QuickCnt;
        public float TotalDistance, PositionX, PositionY, PositionZ, Rotation;
        public int LastChannel;
        public int PosState;
        public uint QuickSlot1;
        public uint QuickSlot2;
        public int TeamJoinDate;
        public int TeamCloseDate;
        public int TeamLeaveDate;
        public int HancoinInven;
        public int HancoinGarage;
        public int Flags;
        public int Guild;
        public long Mileage;
        

        public void Serialize(PacketWriter writer)
        {
            writer.Write(CID);
            writer.WriteUnicodeStatic(Name, 21);
            writer.WriteUnicodeStatic(LastMessageFrom, 0xB);
            writer.Write(LastDate);
            writer.Write(Avatar);
            writer.Write(Level);

            writer.Write(CurExp);
            writer.Write(NextExp);
            writer.Write(BaseExp);

            writer.Write(MitoMoney);
            writer.Write(TeamId);
            writer.Write(TeamMarkId);
            writer.WriteUnicodeStatic(TeamName, 0xD);
            writer.Write(TeamRank);
            writer.Write(PType);
            writer.Write(PvpCnt);
            writer.Write(PvpWinCnt);
            writer.Write(PvpPoint);
            writer.Write(TPvpCnt);
            writer.Write(TPvpWinCnt);
            writer.Write(TPvpPoint);
            writer.Write(QuickCnt);
            writer.Write(0); // unknown
            writer.Write(0); // unknown
            writer.Write(TotalDistance);
            writer.Write(PositionX);
            writer.Write(PositionY);
            writer.Write(PositionZ);
            writer.Write(Rotation);
            writer.Write(LastChannel);
            writer.Write(City);
            writer.Write(PosState);
            writer.Write(CurrentCarId);
            writer.Write(QuickSlot1);
            writer.Write(QuickSlot2);
            writer.Write(TeamJoinDate);
            writer.Write(TeamCloseDate);
            writer.Write(TeamLeaveDate);
            writer.Write(new byte[12]); // filler
            writer.Write(HancoinInven);
            writer.Write(HancoinGarage);
            writer.Write(new byte[42]); // filler
            writer.Write(Flags);
            writer.Write(Guild);
        }
    }
}
