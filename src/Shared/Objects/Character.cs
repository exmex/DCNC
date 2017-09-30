using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using MySql.Data.MySqlClient;
using Shared.Database;
using Shared.Models;
using Shared.Network;
using Shared.Network.GameServer;
using Shared.Util;

namespace Shared.Objects
{
    public class Character
    {
        /// <summary>
        /// The character Id from DB
        /// </summary>
        public ulong Id;
        
        /// <summary>
        /// Character Name
        /// Unicode 21 Chars
        /// </summary>
        public string Name;
        
        /// <summary>
        /// ?
        /// 11 (0xB) Chars
        /// </summary>
        public string LastMessageFrom;
        
        /// <summary>
        /// Presumably the last date the character was played
        /// </summary>
        public int LastDate;
        
        /// <summary>
        /// The Avatar the character is using
        /// </summary>
        public ushort Avatar;
        
        /// <summary>
        /// The current level of character
        /// </summary>
        public ushort Level;

        /// <summary>
        /// All about the expierence of the user
        /// </summary>
        public ExpInfo ExperienceInfo;
        
        /// <summary>
        /// How much money the character has
        /// </summary>
        public long MitoMoney;
        
        /// <summary>
        /// The DB Id of the team the user is in
        /// </summary>
        public long TeamId;
        
        /// <summary>
        /// The Image Id of the team 
        /// </summary>
        [Obsolete("Use Team")]
        public long TeamMarkId;
        
        /// <summary>
        /// The name of the team
        /// 13 (0xD) Chars
        /// </summary>
        [Obsolete("Use Team")]
        public string TeamName;
        
        /// <summary>
        /// Presumably the rank in the team the char has
        /// </summary>
        public int TeamRank;
        
        /// <summary>
        /// The party type
        /// 65 != Party is null?
        /// </summary>
        public byte PartyType;
        
        /// <summary>
        /// Presumably How much PvP he has done?
        /// </summary>
        public uint PvpCount;
        
        /// <summary>
        /// Presumably how much Pvp Points
        /// </summary>
        public uint PvpPoint;
        
        /// <summary>
        /// Presumably how many wins he got in PvP
        /// </summary>
        public uint PvpWinCount;
        
        /// <summary>
        /// Presumably Team Pvp Count
        /// <see cref="PvpCount"/>
        /// </summary>
        public uint TeamPvpCount;
        
        /// <summary>
        /// Presumably Team Pvp Points
        /// <see cref="PvpPoint"/>
        /// </summary>
        public uint TeamPvpPoint;
        
        /// <summary>
        /// Presumably Team Pvp Wins
        /// <see cref="PvpWinCount"/>
        /// </summary>
        public uint TeamPvpWinCount;
        
        /// <summary>
        /// Presumably How many Quick services he had?
        /// </summary>
        public uint QuickCount;
        
        /// <summary>
        /// Presumably the total distance traveled
        /// </summary>
        public float TotalDistance;
        
        /// <summary>
        /// The current position & rotation
        /// </summary>
        public Vector4 Position;
        
        /// <summary>
        /// The last channel Id he was in
        /// </summary>
        public int LastChannel;
        
        /// <summary>
        /// The current city Id
        /// 0 = Moon Palace
        /// 1 = Koinonia
        /// 2 = Cras
        /// </summary>
        public int City;

        /// <summary>
        /// The current position state
        /// 0 = Moon Palace Introduction
        /// 2 = Fresh spawn
        /// 3 = ??
        /// </summary>
        public int PosState;
        
        /// <summary>
        /// Db Id of the car he is driving
        /// </summary>
        public uint ActiveVehicleId;
        
        /// <summary>
        /// TableIndex of Item he has in his first quick slot
        /// </summary>
        public uint QuickSlot1;
        
        /// <summary>
        /// TableIndex of Item he has in his second quick slot
        /// </summary>
        public uint QuickSlot2;
        
        /// <summary>
        /// Unix timestamp when he joined the team
        /// </summary>
        public int TeamJoinDate;
        
        /// <summary>
        /// Unix timestamp when his team got closed
        /// </summary>
        public int TeamCloseDate;
        
        /// <summary>
        /// Unix timestamp when he left his team
        /// </summary>
        public int TeamLeaveDate;
        
        /// <summary>
        /// Zero-based inventory level (pages)
        /// </summary>
        public int InventoryLevel;

        /// <summary>
        /// Zero-based garage level (floors)
        /// </summary>
        public int GarageLevel;
        
        /// <summary>
        /// Some kind of flags
        /// nBattleTutorialCnt |= 0x4000000u
        /// 
        /// enum XiStrCharInfo::FlagType
        /// {
        ///     Beginner_Tutorial = 0x8000000,
        ///     Battle_Tutorial = 0x4000000,
        /// };
        /// </summary>
        public int Flags = 0x8000000;
        
        /// <summary>
        /// 
        /// </summary>
        public int Guild;
        
        /// <summary>
        /// The DCGP Db Id
        /// </summary>
        public uint GPTeam;
        
        /// CharacterInfo End
        
        /// <summary>
        /// Unix timestamp when the chararacter was created
        /// </summary>
        public int CreationDate;
        
        /// <summary>
        /// Db Id of the user associated with this character
        /// </summary>
        public ulong Uid;

        //private Vehicle _activeCar;

        /// <summary>
        /// The current active vehicle fetched from Db
        /// NEW: Now points to a car in GarageVehicles
        /// </summary>
        public Vehicle ActiveCar;
        /*{
            get
            { return _activeCar ?? (_activeCar = GarageVehicles.Find(vehicle => vehicle.CarID == ActiveVehicleId)); }
        }*/

        public Team Team;
        
        /// <summary>
        /// Items in his inventory
        /// Size: 20 * (InventoryLevel+1)
        /// </summary>
        public List<InventoryItem> InventoryItems;

        /// <summary>
        /// Visual Items in his inventory
        /// </summary>
        public List<InventoryVisualItem> InventoryVisualItems;

        public List<Vehicle> GarageVehicles;
        
        /// <summary>
        /// All pending item modifications
        /// Item moved, item added, item deleted / used
        /// </summary>
        private List<ItemMod> ItemModificationBuffer;

        public ushort VehicleSerial;

        public Character()
        {
            ItemModificationBuffer = new List<ItemMod>();
            InventoryItems = new List<InventoryItem>();
            GarageVehicles = new List<Vehicle>();
            ExperienceInfo = new ExpInfo();
            
            //ActiveCar = new Vehicle();

            // Default vals for new chars
            CreationDate = (int)DateTimeOffset.Now.ToUnixTimeSeconds();
            LastDate = (int)DateTimeOffset.Now.ToUnixTimeSeconds();
            
            City = 1;
            Level = 1;
            PosState = 1;
            LastChannel = -1;
        }

        /// <summary>
        /// Adds a new inventory modification
        /// </summary>
        /// <param name="item">The item that was modified</param>
        /// <param name="moved">If the item was moved</param>
        public void AddItemMod(InventoryItem item, bool moved = false)
        {
            ItemModificationBuffer.Add(new ItemMod()
            {
                InventoryItem = item,
                State = moved ? 3 : item.StackNum == 0 ? 2 : 0,
            });
        }

        /// <summary>
        /// Flushes the pending inventory modifications
        /// Sends ItemModListAnswer to client
        /// </summary>
        /// <param name="client">The client to send to</param>
        public void FlushItemModBuffer(Client client)
        {
            var mods = ItemModificationBuffer.ToArray();
            ItemModificationBuffer.Clear();
            
            client.Send(new ItemModListAnswer()
            {
                Items = mods,
            }.CreatePacket());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(Id);
            writer.WriteUnicodeStatic(Name, 21);
            writer.WriteUnicodeStatic(LastMessageFrom, 11);
            writer.Write(LastDate);
            writer.Write(Avatar);
            writer.Write(Level);
            
            writer.Write(ExperienceInfo);

            writer.Write(MitoMoney);
            if (Team == null)
            {
                writer.Write(-1L);
                writer.Write(0L);
                writer.WriteUnicodeStatic("", 13);
                writer.Write(0);
            }
            else
            {
                writer.Write(TeamId);
                writer.Write(Team.TeamMarkId);
                writer.WriteUnicodeStatic(Team.TeamName, 13);
                writer.Write(TeamRank);
            }
            //writer.Write(TeamMarkId);
            //writer.WriteUnicodeStatic(TeamName, 13);
            //writer.Write(TeamRank);
            writer.Write(PartyType);
            writer.Write(PvpCount);
            writer.Write(PvpWinCount);
            writer.Write(PvpPoint);
            writer.Write(TeamPvpCount);
            writer.Write(TeamPvpWinCount);
            writer.Write(TeamPvpPoint);
            writer.Write(QuickCount);
            writer.Write(0); // unknown
            writer.Write(0); // unknown
            writer.Write(TotalDistance); // NOPE! TotalDistance says 62!
            writer.Write(Position);
            writer.Write(LastChannel);
            writer.Write(City);
            writer.Write(PosState);
            writer.Write(ActiveVehicleId);
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
            //writer.Write(new byte[38]); // filler
            //writer.Write(GPTeam); // DCGP team
        }

        public void ReadFromDb(IDataReader reader)
        {
            Id = Convert.ToUInt64(reader["CID"]);
            Uid = Convert.ToUInt64(reader["UID"]);
            Name = reader["Name"] as string;
            /*if (!reader.IsDBNull(reader.GetOrdinal("TEAMNAME")))
                TeamName = reader["TEAMNAME"] as string;
            else
                TeamName = "";*/
            CreationDate = Convert.ToInt32(reader["CreationDate"]);
            MitoMoney = Convert.ToInt64(reader["Mito"]);
            Avatar = Convert.ToUInt16(reader["Avatar"]);
            Level = Convert.ToUInt16(reader["Level"]);
            
            ExperienceInfo.BaseExp = Convert.ToInt32(reader["BaseExp"]);
            ExperienceInfo.CurExp = Convert.ToInt32(reader["CurExp"]);
            ExperienceInfo.NextExp = Convert.ToInt32(reader["NextExp"]);
            City = Convert.ToInt32(reader["City"]);
            ActiveVehicleId = Convert.ToUInt32(reader["CurrentCarID"]);
            InventoryLevel = Convert.ToInt32(reader["InventoryLevel"]);
            
            //InventoryItems = new InventoryItem[(InventoryLevel+1)*20];
            
            GarageLevel = Convert.ToInt32(reader["GarageLevel"]);
            TeamId = Convert.ToInt64(reader["TeamId"]);
            Position = new Vector4(Convert.ToSingle(reader["posX"]), Convert.ToSingle(reader["posY"]), Convert.ToSingle(reader["posZ"]), Convert.ToSingle(reader["posW"]));
            LastChannel = Convert.ToInt32(reader["channelId"]);
            PosState = Convert.ToInt32(reader["posState"]);

            /*ActiveCar = new Vehicle();
            if (!reader.IsDBNull(reader.GetOrdinal("baseColor")))
                ActiveCar.BaseColor = Convert.ToUInt32(reader["baseColor"]);
            if(!reader.IsDBNull(reader.GetOrdinal("carType")))
                ActiveCar.CarType = Convert.ToUInt32(reader["carType"]);*/

            /*if(!reader.IsDBNull(reader.GetOrdinal("TMARKID")))
                TeamMarkId = Convert.ToInt64(reader["TMARKID"]);
            if(!reader.IsDBNull(reader.GetOrdinal("CLOSEDATE")))
                TeamCloseDate = Convert.ToInt32(reader["CLOSEDATE"]);
            if(!reader.IsDBNull(reader.GetOrdinal("TEAMRANKING")))
                TeamRank = Convert.ToInt32(reader["TEAMRANKING"]);*/
        }

        public void WriteToDb(ref UpdateCommand cmd)
        {
            //cmd.Set("CID", Cid);
            //cmd.Set("UID", Uid);
            cmd.Set("Name", Name);
            //cmd.Set("TEAMNAME",  TeamName );
            cmd.Set("CreationDate", CreationDate);
            cmd.Set("Mito", MitoMoney);
            cmd.Set("Avatar", Avatar);
            cmd.Set("Level", Level);
            cmd.Set("BaseExp", ExperienceInfo.BaseExp);
            cmd.Set("CurExp", ExperienceInfo.CurExp);
            cmd.Set("NextExp", ExperienceInfo.NextExp);
            cmd.Set("City", City);
            cmd.Set("CurrentCarID", ActiveVehicleId);
            cmd.Set("InventoryLevel", InventoryLevel);
            cmd.Set("GarageLevel", GarageLevel);
            cmd.Set("TeamId", TeamId);
            cmd.Set("posX", Position.X);
            cmd.Set("posY", Position.Y);
            cmd.Set("posZ", Position.Z);
            cmd.Set("posW", Position.W);
            cmd.Set("channelId", LastChannel);
            cmd.Set("posState", PosState);
            /*ActiveCar = new Vehicle
            {
            BaseColor = Convert.ToUInt32(cmd.Set("baseColor", ),
            CarType = Convert.ToUInt32(cmd.Set("carType", )
            };*/
        }

        /// <summary>
        /// Caluclates level from exp get
        /// </summary>
        /// <param name="exp">The exp to get</param>
        /// <param name="bLevelChangeOut">if the user leveledup</param>
        /// <param name="bUseBonus">If we should use a bonus</param>
        /// <param name="bUseMita500Bonus">If we should use mita bonus</param>
        /// <returns></returns>
        public void CalculateExp(int exp, out bool bLevelChangeOut, bool bUseBonus, bool bUseMita500Bonus)
        {
            if (bUseBonus)
            {
                /*
                fExp = (float) Exp;
                fBonusExp = *(float*) &FLOAT_0_0;
                if (this->m_bBonusExp == 1)
                    fBonusExp = (float) (fExp * 0.30000001) + 0.0;
                v5 = this->m_EnChantBonus.Exp;
                __asm {
                    lahf
                }
                if (__SETP__(_AH & 0x44, 0))
                    fBonusExp = (float) (fExp * this->m_EnChantBonus.Exp) + fBonusExp;
                if (fBonusExp > 0.0)
                    fExp = fExp + fBonusExp;
                this->m_FExp.m_fFraction = this->m_FExp.m_fFraction + fExp;
                v7 = (signed int)ffloor(this->m_FExp.m_fFraction);
                this->m_FExp.m_fFraction = this->m_FExp.m_fFraction - (float) v7;
                Exp = v7;
                */
            }

            if (bUseMita500Bonus)
            {
                /*
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
                }*/
            }

            bLevelChangeOut = false;
            var newExp = ExperienceInfo.CurExp + exp;

            ushort newLevel = 0;
            long newBaseExp = 0;
            long newNextExp = 0;
            for (var i = 1; i < ServerMain.LevelTable.Count; i++)
            {
                var expLevelInfo = ServerMain.LevelTable[i];
                if (ServerMain.LevelTable[i - 1].Value <= newExp && expLevelInfo.Value > newExp)
                {
                    newBaseExp = ServerMain.LevelTable[i - 1].Value;
                    newNextExp = ServerMain.LevelTable[i].Value;
                    newLevel = expLevelInfo.Key;
                }
            }

#if DEBUG
            Log.Debug($"CaclulateExp: CurExp: {ExperienceInfo.CurExp}, Level: {Level}, BaseExp: {ExperienceInfo.BaseExp}, NextExp {ExperienceInfo.NextExp}");
            Log.Debug($"NEW DATA CaclulateExp: CurExp: {newExp}, Level: {newLevel}, BaseExp: {newBaseExp}, NextExp {newNextExp}");
#endif
            
            ExperienceInfo.CurExp = newExp;

            if (Level < newLevel)
            {
                Level = newLevel;
                ExperienceInfo.BaseExp = newBaseExp;
                ExperienceInfo.NextExp = newNextExp;
                //XiCsCharInfo::ResetChaseFrequency(thisa);
                bLevelChangeOut = true;
                //XiCsCharInfo::StatUpdate(thisa);
                /*
                if(character.Level >= 4)
                    thisa->m_bArbeitEnabled = 1;
                if(character.Level >= 6)
                {
                    thisa->m_bChaseEnabled = 1;
                    thisa->m_nextChaseTime = GetSystemTick() + 1000;
                }
                */
            }
            /*
            if(!exp || bLevelChangeOut)
                XiCsCharInfo::InitDrop(thisa, Level);
            if (Exp)
            {
                nCurDeltaExp = Exp + InterlockedExchangeAdd(&thisa->m_Delta.m_deltaExp, Exp);
                if (bLevelChange
                    || (fDeltaRate = (double) nCurDeltaExp
                                     / (double) (thisa->m_CharInfo.ExpInfo.NextExp - thisa->m_CharInfo.ExpInfo.BaseExp),
                    fDeltaRate > 0.1)
                    || nCurDeltaExp > 200)
                {
                    v10 = BS_SingletonHeap < BS_GameDB,1 >::GetInstance();
                    BS_GameDB::UpdateCharInfo(v10, thisa);
                }
                pTeam = thisa->m_pTeam.m_pObj;
                if (pTeam)
                    InterlockedIncrement(&pTeam->m_nRef);
                if (pTeam)
                {
                    pTeam->m_bTeamDataDirty = 1;
                    pTeam->m_bTeamMemberDirty = 1;
                }
                if (pTeam)
                    XiCsTeam::Release(pTeam);
            }
            */


            if (bLevelChangeOut)
            {
                //XiCsCharInfo::LevelUped(thisa);
            }

            /*
            bLevelChange = 0;
            thisa->m_CharInfo.ExpInfo.CurExp += Exp;
            Level = XiExpTable::GetLevel(thisa->m_ExpTable, &thisa->m_CharInfo.ExpInfo);
            if (thisa->m_CharInfo.Level != Level)
            {
                thisa->m_CharInfo.Level = Level;
                XiCsCharInfo::ResetChaseFrequency(thisa);
                bLevelChange = 1;
                XiCsCharInfo::StatUpdate(thisa);
                if ((unsigned int)thisa->m_CharInfo.Level >= 4 )
                thisa->m_bArbeitEnabled = 1;
                if ((unsigned int)thisa->m_CharInfo.Level >= 6 )
                {
                    thisa->m_bChaseEnabled = 1;
                    thisa->m_nextChaseTime = GetSystemTick() + 1000;
                }
            }
            if (!Exp || bLevelChange == 1)
                XiCsCharInfo::InitDrop(thisa, Level);
            if (Exp)
            {
                nCurDeltaExp = Exp + InterlockedExchangeAdd(&thisa->m_Delta.m_deltaExp, Exp);
                if (bLevelChange
                    || (fDeltaRate = (double) nCurDeltaExp
                                     / (double) (thisa->m_CharInfo.ExpInfo.NextExp - thisa->m_CharInfo.ExpInfo.BaseExp),
                    fDeltaRate > 0.1)
                    || nCurDeltaExp > 200)
                {
                    v10 = BS_SingletonHeap < BS_GameDB,1 >::GetInstance();
                    BS_GameDB::UpdateCharInfo(v10, thisa);
                }
                pTeam = thisa->m_pTeam.m_pObj;
                if (pTeam)
                    InterlockedIncrement(&pTeam->m_nRef);
                if (pTeam)
                {
                    pTeam->m_bTeamDataDirty = 1;
                    pTeam->m_bTeamMemberDirty = 1;
                }
                if (pTeam)
                    XiCsTeam::Release(pTeam);
            }
            if (bLevelChange == 1)
            {
                XiCsCharInfo::LevelUped(thisa);
                *bLevelChangeOut = 1;
            }
            return Exp;
            */

            // TODO: Make this function!
            /*
            float v5; // et1@4
            int v7; // ST44_4@8
            int v8; // ST3C_4@12
            float fDeltaRate; // ST50_4@24
            BS_GameDB* v10; // eax@26
            XiCsCharInfo* thisa; // [sp+8h] [bp-64h]@1
            int nCurDeltaExp; // [sp+40h] [bp-2Ch]@23
            XiCsTeam* pTeam; // [sp+44h] [bp-28h]@27
            float fExp; // [sp+50h] [bp-1Ch]@2
            float fBonusExp; // [sp+54h] [bp-18h]@2
            bool bLevelChange; // [sp+5Bh] [bp-11h]@14
            int Level; // [sp+5Ch] [bp-10h]@14

            thisa = this;
            if (bUseBonus == 1)
            {
                fExp = (float) Exp;
                fBonusExp = *(float*) &FLOAT_0_0;
                if (this->m_bBonusExp == 1)
                    fBonusExp = (float) (fExp * 0.30000001) + 0.0;
                v5 = this->m_EnChantBonus.Exp;
                __asm {
                    lahf
                }
                if (__SETP__(_AH & 0x44, 0))
                    fBonusExp = (float) (fExp * this->m_EnChantBonus.Exp) + fBonusExp;
                if (fBonusExp > 0.0)
                    fExp = fExp + fBonusExp;
                this->m_FExp.m_fFraction = this->m_FExp.m_fFraction + fExp;
                v7 = (signed int)ffloor(this->m_FExp.m_fFraction);
                this->m_FExp.m_fFraction = this->m_FExp.m_fFraction - (float) v7;
                Exp = v7;
            }
            if (bUseMita500Bonus && this->m_Mita500Buff.m_bBuffState)
            {
                if (XiCsCharInfo::GetMita500BuffCheck(this))
                {
                    thisa->m_FExp.m_fFraction = thisa->m_FExp.m_fFraction
                                                + (float) ((float) ((float) thisa->m_Mita500Buff.m_RewardPoint /
                                                                    100.0) * (float) Exp);
                    v8 = (signed int)ffloor(thisa->m_FExp.m_fFraction);
                    thisa->m_FExp.m_fFraction = thisa->m_FExp.m_fFraction - (float) v8;
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
            if (thisa->m_CharInfo.Level != Level)
            {
                thisa->m_CharInfo.Level = Level;
                XiCsCharInfo::ResetChaseFrequency(thisa);
                bLevelChange = 1;
                XiCsCharInfo::StatUpdate(thisa);
                if ((unsigned int)thisa->m_CharInfo.Level >= 4 )
                thisa->m_bArbeitEnabled = 1;
                if ((unsigned int)thisa->m_CharInfo.Level >= 6 )
                {
                    thisa->m_bChaseEnabled = 1;
                    thisa->m_nextChaseTime = GetSystemTick() + 1000;
                }
            }
            if (!Exp || bLevelChange == 1)
                XiCsCharInfo::InitDrop(thisa, Level);
            if (Exp)
            {
                nCurDeltaExp = Exp + InterlockedExchangeAdd(&thisa->m_Delta.m_deltaExp, Exp);
                if (bLevelChange
                    || (fDeltaRate = (double) nCurDeltaExp
                                     / (double) (thisa->m_CharInfo.ExpInfo.NextExp - thisa->m_CharInfo.ExpInfo.BaseExp),
                    fDeltaRate > 0.1)
                    || nCurDeltaExp > 200)
                {
                    v10 = BS_SingletonHeap < BS_GameDB,1 >::GetInstance();
                    BS_GameDB::UpdateCharInfo(v10, thisa);
                }
                pTeam = thisa->m_pTeam.m_pObj;
                if (pTeam)
                    InterlockedIncrement(&pTeam->m_nRef);
                if (pTeam)
                {
                    pTeam->m_bTeamDataDirty = 1;
                    pTeam->m_bTeamMemberDirty = 1;
                }
                if (pTeam)
                    XiCsTeam::Release(pTeam);
            }
            if (bLevelChange == 1)
            {
                XiCsCharInfo::LevelUped(thisa);
                *bLevelChangeOut = 1;
            }
            return Exp;
            */
        }

        public bool FindFreeSlot(MySqlConnection dbconn, InventoryItem inventoryItem)
        {
            foreach (var item in InventoryItems)
            {
                if (item.TableIndex != inventoryItem.TableIndex) continue;
                
                // check if the item is stackable!
                item.StackNum++;
                ItemModel.Update(dbconn, item);
                return false;
            }
            return true;
        }

        public void LevelUp()
        {
            /*
            int v1; // ST130_4@6
            int v2; // ST134_4@6
            _DWORD* v3; // eax@6
            int v4; // ST128_4@9
            int v5; // ST12C_4@9
            _DWORD* v6; // eax@9
            char* v7; // ST120_4@12
            int v8; // ST124_4@12
            char* v9; // ST11C_4@12
            WebCallReq* v10; // ecx@12
            _DWORD* v11; // eax@15
            __int64 v12; // rax@19
            int v13; // ecx@19
            int v14; // STFC_4@19
            char* v16; // ST0C_4@27
            __int64 v17; // ST04_8@27
            BS_GameDB* v18; // eax@27
            int v19; // eax@29
            int v20; // ST24_4@43
            __int64 v21; // ST14_8@44
            const char* v22; // ST10_4@44
            char* v23; // ST0C_4@44
            __int64 v24; // ST04_8@44
            BS_GameDB* v25; // eax@44
            __int64 v26; // rax@47
            int v27; // ecx@47
            WebCallReq* v28; // [sp+4h] [bp-C14h]@12
            WebCallReq* v29; // [sp+8h] [bp-C10h]@9
            WebCallReq* v30; // [sp+Ch] [bp-C0Ch]@6
            XiCsCharInfo* thisa; // [sp+10h] [bp-C08h]@1
            BS_GameDispatch* v32; // [sp+20h] [bp-BF8h]@45
            BS_GameDispatch* v33; // [sp+7Ch] [bp-B9Ch]@27
            BS_StrUtils::BS_MBtoWide v34; // [sp+11Ch] [bp-AFCh]@15
            WebCallReq* v35; // [sp+124h] [bp-AF4h]@11
            WebCallReq* v36; // [sp+128h] [bp-AF0h]@14
            void* v37; // [sp+12Ch] [bp-AECh]@8
            WebCallReq* v38; // [sp+130h] [bp-AE8h]@11
            void* v39; // [sp+134h] [bp-AE4h]@5
            WebCallReq* v40; // [sp+138h] [bp-AE0h]@8
            int v41; // [sp+13Ch] [bp-ADCh]@47
            _RTL_CRITICAL_SECTION* v42; // [sp+140h] [bp-AD8h]@45
            wchar_t txt[256]; // [sp+144h] [bp-AD4h]@45
            BS_StrUtils::BS_WidetoMB v44; // [sp+344h] [bp-8D4h]@44
            const char* eventName; // [sp+34Ch] [bp-8CCh]@43
            int v46; // [sp+350h] [bp-8C8h]@43
            int Mito; // [sp+354h] [bp-8C4h]@34
            int v48; // [sp+358h] [bp-8C0h]@29
            _RTL_CRITICAL_SECTION* v49; // [sp+35Ch] [bp-8BCh]@27
            BS_StrUtils::BS_WidetoMB szCName; // [sp+360h] [bp-8B8h]@27
            int nExist; // [sp+368h] [bp-8B0h]@24
            wchar_t wbuff[1024]; // [sp+36Ch] [bp-8ACh]@15
            int msg; // [sp+B70h] [bp-A8h]@19
            BS_CriticalSection* v54; // [sp+B74h] [bp-A4h]@17
            int nLen; // [sp+B78h] [bp-A0h]@17
            char szurl[128]; // [sp+B7Ch] [bp-9Ch]@5
            int nEventMoney; // [sp+C04h] [bp-14h]@14
            BS_GameDispatch* pGameDispatch; // [sp+C08h] [bp-10h]@14
            int v59; // [sp+C14h] [bp-4h]@5

            thisa = this;
            XiCsCharInfo::CheckRecommendFriend(this);
            if (thisa->m_CharInfo.Level == 7
                && BS_SingletonHeap < BS_Config,5 >::GetInstance()->m_Config.InviteFriendEvent > 0
                && thisa->m_Recommend.trying
                && thisa->m_Recommend.cardid[0] )
            {
                StringCbPrintfA(
                    szurl,
                    0x80u,
                    "exfriend/api/pubupdate.nhn?gameid=SKIDRUSH&userid=%s&cardid=%I64d&gubun=R",
                    thisa->m_szMemberId,
                    thisa->m_Recommend.cardid);
                v39 = operator new(0x28u);
                v59 = 0;
                if (v39)
                {
                    v1 = thisa->m_CharInfo.Cid;
                    v2 = HIDWORD(thisa->m_CharInfo.Cid);
                    *(_DWORD*) v39 = &WebCallReq::`vftable';
                    std::basic_string < char,std::char_traits<char>,std::allocator < char >>::basic_string<char,
                        std::char_traits<char>, std::allocator<char>>(
                        (std::basic_string<char, std::char_traits<char>, std::allocator<char>>*) ((char*) v39 + 4),
                        szurl);
                    *(_DWORD*) v39 = &WebCallUpdateForInvited::`vftable';
                    v3 = v39;
                    *((_DWORD*) v39 + 8) = v1;
                    v3[9] = v2;
                    LOBYTE(v59) = 0;
                    v30 = (WebCallReq*) v39;
                }
                else
                {
                    v30 = 0;
                }
                v40 = v30;
                v59 = -1;
                BS_Global::g_friendInviteCheckMgr->vfptr->EnqueueReq(BS_Global::g_friendInviteCheckMgr, v30);
                StringCbPrintfA(
                    szurl,
                    0x80u,
                    "exfriend/api/pubupdate.nhn?gameid=SKIDRUSH&userid=%s&cardid=%I64d&gubun=S",
                    &thisa->m_Recommend.from,
                    thisa->m_Recommend.cardid);
                v37 = operator new(0x28u);
                v59 = 3;
                if (v37)
                {
                    v4 = thisa->m_CharInfo.Cid;
                    v5 = HIDWORD(thisa->m_CharInfo.Cid);
                    *(_DWORD*) v37 = &WebCallReq::`vftable';
                    std::basic_string < char,std::char_traits<char>,std::allocator < char >>::basic_string<char,
                        std::char_traits<char>, std::allocator<char>>(
                        (std::basic_string<char, std::char_traits<char>, std::allocator<char>>*) ((char*) v37 + 4),
                        szurl);
                    *(_DWORD*) v37 = &WebCallUpdateForInvited::`vftable';
                    v6 = v37;
                    *((_DWORD*) v37 + 8) = v4;
                    v6[9] = v5;
                    LOBYTE(v59) = 3;
                    v29 = (WebCallReq*) v37;
                }
                else
                {
                    v29 = 0;
                }
                v38 = v29;
                v59 = -1;
                BS_Global::g_friendInviteCheckMgr->vfptr->EnqueueReq(BS_Global::g_friendInviteCheckMgr, v29);
                StringCbPrintfA(szurl, 0x80u, "gameinfo.nhn?m=getDaipyoChar&memberid=%s", &thisa->m_Recommend.from);
                v35 = (WebCallReq*)operator new(0x70u);
                v59 = 6;
                if (v35)
                {
                    v7 = (char*) thisa->m_CharInfo.Cid;
                    v8 = HIDWORD(thisa->m_CharInfo.Cid);
                    v9 = (char*) thisa->m_Recommend.ctime;
                    WebCallReq::WebCallReq(v35, szurl);
                    LOBYTE(v59) = 7;
                    v35->vfptr = (WebCallReqVtbl*) &WebCallInvitorReward::`vftable';
                    LOBYTE(v35[1].vfptr) = 0;
                    v10 = v35;
                    v35[1].url._Bx._Ptr = v7;
                    *((_DWORD*) &v10[1].url._Bx._Ptr + 1) = v8;
                    std::basic_string < unsigned short,std::char_traits < unsigned short >,std::allocator <
                        unsigned short >>::basic_string < unsigned short,std::char_traits <
                        unsigned short >,std::allocator < unsigned short >> (
                        (std::basic_string < unsigned short,std::char_traits < unsigned short >,std::allocator <
                        unsigned short > > *)(&v35[1].url._Bx._Ptr + 2),
                    thisa->m_CharInfo.Name.m_Name);
                    LOBYTE(v59) = 8;
                    std::basic_string < char,std::char_traits<char>,std::allocator < char >>::basic_string<char,
                        std::char_traits<char>, std::allocator<char>>(
                        (std::basic_string<char, std::char_traits<char>, std::allocator<char>>*)
                        ((char*) &v35[2].url + 8),
                        (const char* )&thisa->m_Recommend.from);
                    v35[3].url._Bx._Ptr = v9;
                    LOBYTE(v59) = 6;
                    v28 = v35;
                }
                else
                {
                    v28 = 0;
                }
                v36 = v28;
                v59 = -1;
                BS_Global::g_getRepresentativeCNameMgr->vfptr->EnqueueReq(BS_Global::g_getRepresentativeCNameMgr, v28);
                nEventMoney = 70000;
                XiCsCharInfo::CalcMoney(thisa, 70000i64, "EVENT", "I", "NONE", byte_6BEFB5);
                pGameDispatch = thisa->m_pGameDispatch;
                if (pGameDispatch)
                {
                    BS_StrUtils::BS_MBtoWide::BS_MBtoWide(&v34, thisa->m_szMemberId, 0);
                    v59 = 10;
                    StringCbPrintfW(wbuff, 0x800u, L"%s", *v11);
                    v59 = -1;
                    if (v34.m_pBuffer)
                        operator delete[](v34.m_pBuffer);
                    PacketSend::Send_ChatMessage((BS_PacketDispatch*) &pGameDispatch->vfptr, &off_6BEFD4, wbuff);
                    StringCbPrintfW(wbuff, 0x800u, &word_6BEFE8);
                    nLen = wcslen(wbuff) + 1;
                    v54 = &pGameDispatch->m_lock;
                    if (pGameDispatch != (BS_GameDispatch*) -28)
                        EnterCriticalSection(&v54->m_csLock);
                    v59 = 11;
                    BS_MessageDispatch::GetMessageBuffer<BS_PktMoneyMessage>(pGameDispatch, (char*) &msg, nLen);
                    v59 = 12;
                    *(_DWORD*) (msg + 6) = nLen;
                    *(_DWORD*) (msg + 2) = 1;
                    v12 = nEventMoney;
                    v13 = msg;
                    *(_DWORD*) (msg + 10) = nEventMoney;
                    *(_DWORD*) (v13 + 14) = HIDWORD(v12);
                    v14 = HIDWORD(thisa->m_CharInfo.MitoMoney);
                    HIDWORD(v12) = msg;
                    *(_DWORD*) (msg + 18) = thisa->m_CharInfo.MitoMoney;
                    *(_DWORD*) (HIDWORD(v12) + 22) = v14;
                    wcsncpy((unsigned __int16 *)(msg + 26), wbuff, nLen);
                    ((void  (__thiscall*) (BS_GameDispatch*))pGameDispatch->vfptr[1].__vecDelDtor)(pGameDispatch);
                    v59 = -1;
                    if (v54)
                        LeaveCriticalSection(&v54->m_csLock);
                }
                BS_SingletonHeap < BS_GameDB,1 >::GetInstance();
                BS_GameDB::SetPREventValue(
                    thisa->m_szMemberId,
                    "InviteFriend",
                    0i64,
                (char*) &thisa->m_Recommend.from,
                thisa->m_Recommend.cardid);
                thisa->m_Recommend.trying = 0;
            }
            if (thisa->m_CharInfo.Level == 3 && BS_SingletonHeap < BS_Config,5 >::GetInstance()->m_VipEvent > 0 )
            {
                BS_SingletonHeap < BS_GameDB,1 >::GetInstance();
                nExist = BS_GameDB::CheckPREventById(thisa->m_szMemberId, "VIPEVENT", 2);
                if (nExist > 0)
                    return 1;
                BS_SingletonHeap < BS_GameDB,1 >::GetInstance();
                nExist = BS_GameDB::CheckPREventById(thisa->m_szMemberId, "VIPEVENT", 1);
                if (nExist == 1)
                {
                    BS_StrUtils::BS_WidetoMB::BS_WidetoMB(&szCName, thisa->m_CharInfo.Name.m_Name, 0);
                    v59 = 14;
                    v16 = szCName.m_pBuffer;
                    v17 = thisa->m_CharInfo.Cid;
                    v18 = BS_SingletonHeap < BS_GameDB,1 >::GetInstance();
                    BS_GameDB::InsertPREvent(v18, thisa->m_szMemberId, v17, v16, "VIPEVENT",
                        2i64, byte_6BF012, &byte_6BF011, 0);
                    PacketSend::Send_Error(thisa->m_pGameDispatch->m_pSession, "֍X֩ֈӤҮ");
                    v33 = thisa->m_pGameDispatch;
                    v49 = &v33->m_lock.m_csLock;
                    if (v33 != (BS_GameDispatch*) -28)
                        EnterCriticalSection(v49);
                    LOBYTE(v59) = 15;
                    BS_MessageDispatch::GetMessageBuffer<BS_PktCheckEventAck>(v33, (char**) &v48);
                    LOBYTE(v59) = 16;
                    *(_DWORD*) (v48 + 2) = 1;
                    v19 = v48;
                    *(_DWORD*) (v48 + 6) = 0;
                    *(_DWORD*) (v19 + 10) = 0;
                    ((void  (__thiscall*) (BS_GameDispatch*))thisa->m_pGameDispatch->vfptr[1]
                        .__vecDelDtor)(thisa->m_pGameDispatch);
                    LOBYTE(v59) = 14;
                    if (v49)
                        LeaveCriticalSection(v49);
                    v59 = -1;
                    if (szCName.m_pBuffer)
                        operator delete[](szCName.m_pBuffer);
                }
            }
            if (BS_SingletonHeap < BS_Config,5 >::GetInstance()->m_Config.LevelMitoEvent )
            {
                Mito = 0;
                switch (thisa->m_CharInfo.Level)
                {
                    case 0xBu:
                        Mito = 10000;
                        break;
                    case 0x15u:
                        Mito = 20000;
                        break;
                    case 0x1Fu:
                        Mito = 30000;
                        break;
                    case 0x29u:
                        Mito = 40000;
                        break;
                }
                if (Mito > 0)
                {
                    eventName = "LEVELMITO";
                    v20 = thisa->m_CharInfo.Level;
                    BS_SingletonHeap < BS_GameDB,1 >::GetInstance();
                    v46 = BS_GameDB::CheckPREventById(thisa->m_szMemberId, "LEVELMITO", v20);
                    if (!v46)
                    {
                        BS_StrUtils::BS_WidetoMB::BS_WidetoMB(&v44, thisa->m_CharInfo.Name.m_Name, 0);
                        v59 = 18;
                        v21 = thisa->m_CharInfo.Level;
                        v22 = eventName;
                        v23 = v44.m_pBuffer;
                        v24 = thisa->m_CharInfo.Cid;
                        v25 = BS_SingletonHeap < BS_GameDB,1 >::GetInstance();
                        BS_GameDB::InsertPREvent(v25, thisa->m_szMemberId, v24, v23, v22, v21, &byte_6BF0AF,
                            &byte_6BF0AE, 0);
                        if (XiCsCharInfo::CalcMoney(thisa, Mito, "Event", "I", "NONE", byte_6BF0B0))
                        {
                            GTWBufFmt("20276", txt, 512, thisa->m_CharInfo.Level, Mito);
                            PacketSend::Send_Error(thisa->m_pGameDispatch->m_pSession, txt);
                            v32 = thisa->m_pGameDispatch;
                            v42 = &v32->m_lock.m_csLock;
                            if (v32 != (BS_GameDispatch*) -28)
                                EnterCriticalSection(v42);
                            LOBYTE(v59) = 19;
                            BS_MessageDispatch::GetMessageBuffer<BS_PktCheckEventAck>(v32, (char**) &v41);
                            LOBYTE(v59) = 20;
                            *(_DWORD*) (v41 + 2) = 0;
                            v26 = Mito;
                            v27 = v41;
                            *(_DWORD*) (v41 + 6) = Mito;
                            *(_DWORD*) (v27 + 10) = HIDWORD(v26);
                            ((void  (__thiscall*) (BS_GameDispatch*))thisa->m_pGameDispatch->vfptr[1]
                                .__vecDelDtor)(thisa->m_pGameDispatch);
                            LOBYTE(v59) = 18;
                            if (v42)
                                LeaveCriticalSection(v42);
                        }
                        v59 = -1;
                        if (v44.m_pBuffer)
                            operator delete[](v44.m_pBuffer);
                    }
                }
            }*/
        }

        /// <summary>
        /// Gives the character the specified item and quantity
        /// </summary>
        /// <param name="dbconn">The database connection</param>
        /// <param name="tableIndex">Table index of the item to give</param>
        /// <param name="quantity">The amount</param>
        /// <returns>InventoryItem on success, null on failure</returns>
        public InventoryItem GiveItem(MySqlConnection dbconn, int tableIndex, uint quantity)
        {
            var invIdx = InventoryItems.Count;
            /*int invIdx = 0;
            foreach (var item in InventoryItems)
            {
                if (item == null)
                    break;
                invIdx++;
            }*/

            var invItem = new InventoryItem(Id, ActiveVehicleId, tableIndex, (uint)invIdx, quantity);
            if (!ItemModel.Create(dbconn, invItem)) return null;
            
            AddItemMod(invItem);
            InventoryItems.Add(invItem);
            return invItem;
        }

        /// <summary>
        /// Removes the specified item from the character
        /// </summary>
        /// <param name="dbconn">The database connection</param>
        /// <param name="slot">The slot the item is in</param>
        /// <param name="quantity">The quantity to remove</param>
        /// <returns>true on success, false on failure</returns>
        public bool RemoveItem(MySqlConnection dbconn, int slot, uint quantity)
        {
            if (InventoryItems[slot] == null) return false;
            var itemInSlot = InventoryItems[slot];
            if (itemInSlot.StackNum < quantity) return false;

            // Check if the item should be removed, or just decreased
            if (itemInSlot.StackNum - quantity == 0)
            {
                if (!ItemModel.Remove(dbconn, Id, slot))
                    return false;
                InventoryItems.Remove(itemInSlot);
                //InventoryItems[slot] = null;
                itemInSlot.StackNum = 0;
            }
            else
            {
                InventoryItems[slot].StackNum -= quantity;
                itemInSlot.StackNum -= quantity;
                
                ItemModel.Update(dbconn, InventoryItems[slot]);
            }

            AddItemMod(itemInSlot);
            return true;
        }
    }
}