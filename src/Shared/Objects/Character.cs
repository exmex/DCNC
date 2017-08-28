using System;
using System.Data;
using Shared.Database;
using Shared.Network;
using Shared.Util;

namespace Shared.Objects
{
    public class Character
    {
        public Vehicle ActiveCar;
        public ushort Avatar;
        public long BaseExp;
        public ulong Cid;
        public int City;
        public int CreationDate;

        public long CurExp;
        public int CurrentCarId;
        public int Flags;
        public int GarageLevel;
        public int Guild;
        [Obsolete("Same as GarageLevel")] public int HancoinGarage; // TODO/FIXME: Same as GarageLevel?
        [Obsolete("Same as InventoryLevel")] public int HancoinInven; // TODO/FIXME: Same as InventoryLevel?
        public int InventoryLevel;
        public int LastChannel;
        public int LastDate;

        //public ulong CID;
        public string LastMessageFrom; // 0xB

        public ushort Level;
        public long Mileage;

        public long MitoMoney;

        public string Name;
        public long NextExp;

        public int posState;
        public int PosState;
        public byte PType;
        public uint PvpCnt;
        public uint PvpPoint;
        public uint PvpWinCnt;
        public uint QuickCnt;
        public uint QuickSlot1;
        public uint QuickSlot2;
        public int TeamCloseDate;
        public long TeamId;
        public int TeamJoinDate;
        public int TeamLeaveDate;
        public long TeamMarkId;
        public string TeamName; // 0xD / 13
        public int TeamRank;
        public long Tid; // TODO/FIXME: Same as teamId?
        public float TotalDistance, PositionX, PositionY, PositionZ, Rotation;
        public uint TPvpCnt;
        public uint TPvpPoint;
        public uint TPvpWinCnt;
        public ulong Uid;
        public User User;

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(Cid);
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
            writer.Write(InventoryLevel);
            writer.Write(GarageLevel);
            writer.Write(new byte[42]); // filler
            writer.Write(Flags);
            writer.Write(Guild);
        }

        public void ReadFromDb(IDataReader reader)
        {
            Cid = Convert.ToUInt64(reader["CID"]);
            Uid = Convert.ToUInt64(reader["UID"]);
            Name = reader["Name"] as string;
            TeamName = reader["TEAMNAME"] as string;
            CreationDate = Convert.ToInt32(reader["CreationDate"]);
            MitoMoney = Convert.ToInt64(reader["Mito"]);
            Avatar = Convert.ToUInt16(reader["Avatar"]);
            Level = Convert.ToUInt16(reader["Level"]);
            BaseExp = Convert.ToInt32(reader["BaseExp"]);
            CurExp = Convert.ToInt32(reader["CurExp"]);
            NextExp = Convert.ToInt32(reader["NextExp"]);
            City = Convert.ToInt32(reader["City"]);
            CurrentCarId = Convert.ToInt32(reader["CurrentCarID"]);
            InventoryLevel = Convert.ToInt32(reader["InventoryLevel"]);
            GarageLevel = Convert.ToInt32(reader["GarageLevel"]);
            Tid = Convert.ToInt64(reader["TID"]);
            PositionX = Convert.ToSingle(reader["posX"]);
            PositionY = Convert.ToSingle(reader["posY"]);
            PositionZ = Convert.ToSingle(reader["posZ"]);
            Rotation = Convert.ToSingle(reader["posW"]);
            LastChannel = Convert.ToInt32(reader["channelId"]);
            posState = Convert.ToInt32(reader["posState"]);
            ActiveCar = new Vehicle
            {
                BaseColor = Convert.ToUInt32(reader["baseColor"]),
                CarType = Convert.ToUInt32(reader["carType"])
            };
        }
        
        public void WriteToDb(UpdateCommand cmd)
        {
            //cmd.Set("CID", Cid);
            //cmd.Set("UID", Uid);
            cmd.Set("Name", Name);
            //cmd.Set("TEAMNAME",  TeamName );
            cmd.Set("CreationDate", CreationDate);
            cmd.Set("Mito", MitoMoney);
            cmd.Set("Avatar", Avatar);
            cmd.Set("Level", Level);
            cmd.Set("BaseExp", BaseExp);
            cmd.Set("CurExp", CurExp);
            cmd.Set("NextExp", NextExp);
            cmd.Set("City", City);
            cmd.Set("CurrentCarID", CurrentCarId);
            cmd.Set("InventoryLevel", InventoryLevel);
            cmd.Set("GarageLevel", GarageLevel);
            cmd.Set("TID", Tid);
            cmd.Set("posX", PositionX);
            cmd.Set("posY", PositionY);
            cmd.Set("posZ", PositionZ);
            cmd.Set("posW", Rotation);
            cmd.Set("channelId", LastChannel);
            cmd.Set("posState", posState);
            /*ActiveCar = new Vehicle
            {
            BaseColor = Convert.ToUInt32(cmd.Set("baseColor", ),
            CarType = Convert.ToUInt32(cmd.Set("carType", )
            };*/
        }

        public int CalculateExp(int exp, out bool bLevelChangeOut, bool bUseBonus, bool bUseMita500Bonus)
        {
            // TODO: Make this function!
            /*
            *  float v5; // et1@4
            int v7; // ST44_4@8
            int v8; // ST3C_4@12
            float fDeltaRate; // ST50_4@24
            BS_GameDB *v10; // eax@26
            XiCsCharInfo *thisa; // [sp+8h] [bp-64h]@1
            int nCurDeltaExp; // [sp+40h] [bp-2Ch]@23
            XiCsTeam *pTeam; // [sp+44h] [bp-28h]@27
            float fExp; // [sp+50h] [bp-1Ch]@2
            float fBonusExp; // [sp+54h] [bp-18h]@2
            bool bLevelChange; // [sp+5Bh] [bp-11h]@14
            int Level; // [sp+5Ch] [bp-10h]@14
            
            thisa = this;
            if ( bUseBonus == 1 )
            {
            fExp = (float)Exp;
            fBonusExp = *(float *)&FLOAT_0_0;
            if ( this->m_bBonusExp == 1 )
            fBonusExp = (float)(fExp * 0.30000001) + 0.0;
            v5 = this->m_EnChantBonus.Exp;
            __asm { lahf }
            if ( __SETP__(_AH & 0x44, 0) )
            fBonusExp = (float)(fExp * this->m_EnChantBonus.Exp) + fBonusExp;
            if ( fBonusExp > 0.0 )
            fExp = fExp + fBonusExp;
            this->m_FExp.m_fFraction = this->m_FExp.m_fFraction + fExp;
            v7 = (signed int)ffloor(this->m_FExp.m_fFraction);
            this->m_FExp.m_fFraction = this->m_FExp.m_fFraction - (float)v7;
            Exp = v7;
            }
            if ( bUseMita500Bonus && this->m_Mita500Buff.m_bBuffState )
            {
            if ( XiCsCharInfo::GetMita500BuffCheck(this) )
            {
            thisa->m_FExp.m_fFraction = thisa->m_FExp.m_fFraction
            + (float)((float)((float)thisa->m_Mita500Buff.m_RewardPoint / 100.0) * (float)Exp);
            v8 = (signed int)ffloor(thisa->m_FExp.m_fFraction);
            thisa->m_FExp.m_fFraction = thisa->m_FExp.m_fFraction - (float)v8;
            Exp = v8;
            }
            else
            {
            XiCsCharInfo::SetMita500Buff(thisa, 0);
            }
            }
            bLevelChange = 0;
            thisa->m_CharInfo.ExpInfo.CurExp += Exp;
            Level = XiExpTable::GetLevel(thisa->m_ExpTable, &thisa->m_CharInfo.ExpInfo);
            if ( thisa->m_CharInfo.Level != Level )
            {
            thisa->m_CharInfo.Level = Level;
            XiCsCharInfo::ResetChaseFrequency(thisa);
            bLevelChange = 1;
            XiCsCharInfo::StatUpdate(thisa);
            if ( (unsigned int)thisa->m_CharInfo.Level >= 4 )
            thisa->m_bArbeitEnabled = 1;
            if ( (unsigned int)thisa->m_CharInfo.Level >= 6 )
            {
            thisa->m_bChaseEnabled = 1;
            thisa->m_nextChaseTime = GetSystemTick() + 1000;
            }
            }
            if ( !Exp || bLevelChange == 1 )
            XiCsCharInfo::InitDrop(thisa, Level);
            if ( Exp )
            {
            nCurDeltaExp = Exp + InterlockedExchangeAdd(&thisa->m_Delta.m_deltaExp, Exp);
            if ( bLevelChange
            || (fDeltaRate = (double)nCurDeltaExp
            / (double)(thisa->m_CharInfo.ExpInfo.NextExp - thisa->m_CharInfo.ExpInfo.BaseExp),
            fDeltaRate > 0.1)
            || nCurDeltaExp > 200 )
            {
            v10 = BS_SingletonHeap<BS_GameDB,1>::GetInstance();
            BS_GameDB::UpdateCharInfo(v10, thisa);
            }
            pTeam = thisa->m_pTeam.m_pObj;
            if ( pTeam )
            InterlockedIncrement(&pTeam->m_nRef);
            if ( pTeam )
            {
            pTeam->m_bTeamDataDirty = 1;
            pTeam->m_bTeamMemberDirty = 1;
            }
            if ( pTeam )
            XiCsTeam::Release(pTeam);
            }
            if ( bLevelChange == 1 )
            {
            XiCsCharInfo::LevelUped(thisa);
            *bLevelChangeOut = 1;
            }
            return Exp;
            */
            bLevelChangeOut = false;
            return exp;
        }
    }
}