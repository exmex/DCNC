using System;
using System.Collections.Generic;
using System.IO;
using Shared.Util;

namespace Shared.Objects
{
    public class XiStrQuest
    {
        public uint Car_01;
        public uint Car_02;
        public uint ClearQuestIdN;
        public int Count;
        public int[] CrashTime; // 5
        public string EndPost; // char 56
        public XiStrIcon EndPostPtr;
        public uint Event;
        public string GivePost; // char 56
        public XiStrIcon GivePostPtr;
        public uint Idx;
        public string Item01; // char 56
        public string Item02; // char 56
        public string Item03; // char 56
        public int[] MaxLadius; // 5
        public int[] MaxSpeed; // 5
        public int[] MinLadius; // 5
        public int[] MinSpeed; // 5
        public uint NeedLevel;
        public uint NeedLevelPercent;
        public XiStrQuest NextQuestPtr;
        public string[] Place; // char 5,56
        public uint PrevQuestIdN;
        public XiStrQuest PrevQuestPtr;
        public string QuestId; // char 56
        public uint QuestIdN;
        public uint QuestPath_01;
        public uint QuestPath_02;
        public int RewardExp;
        public uint RewardItemNum;
        public int RewardMoney;
        public int[] TimeLimit; // 5

        public string Title; // char 128
        /*public XiStrItem Item01Ptr;
        public XiStrItem Item02Ptr;
        public XiStrItem Item03Ptr;
        public XIVISUALITEM_INFO VSItemPtr;*/

        public static int QuestEventStrToVar(string evt)
        {
            switch (evt.ToUpper())
            {
                default:
                    return 0;

                case "CHASE":
                    return 1;

                case "ARBEIT":
                    return 2;

                case "CITY":
                    return 3;

                case "CITY_2ND":
                    return 4;

                case "CITY_3RD":
                    return 5;

                case "OMD":
                    return 8;

                case "ROO":
                    return 9;
            }
        }

        // TODO: move to XiQuestTable?
        public static Dictionary<uint, XiStrQuest> LoadFromTdf(TdfReader tdfReader)
        {
            var questList = new Dictionary<uint, XiStrQuest>();
            using (var reader = new BinaryReaderExt(new MemoryStream(tdfReader.ResTable)))
            {
                for (var row = 0; row < tdfReader.Header.Row; row++)
                {
                    var quest = new XiStrQuest();
                    quest.QuestId = reader.ReadUnicode();
                    /*quest.QuestIdN = Convert.ToUInt32(reader.ReadUnicode());
                    quest.PrevQuestIdN = Convert.ToUInt32(reader.ReadUnicode());*/
                    quest.QuestIdN =
                        Convert.ToUInt32(reader.ReadUnicode()) -
                        1; // -1 since the request the client sents us are 0 based
                    quest.PrevQuestIdN =
                        Convert.ToUInt32(reader.ReadUnicode()) -
                        1; // -1 since the request the client sents us are 0 based
                    quest.Event = (uint) QuestEventStrToVar(reader.ReadUnicode());
                    quest.NeedLevel = Convert.ToUInt32(reader.ReadUnicode());
                    quest.NeedLevelPercent = Convert.ToUInt32(reader.ReadUnicode());
                    quest.GivePost = reader.ReadUnicode();
                    quest.Title = reader.ReadUnicode();
                    quest.EndPost = reader.ReadUnicode();

                    quest.Place = new string[5];
                    for (var i = 0; i < 5; i++)
                        quest.Place[i] = reader.ReadUnicode();

                    quest.CrashTime = new int[5];
                    for (var i = 0; i < 5; i++)
                        quest.CrashTime[i] = Convert.ToInt32(reader.ReadUnicode());

                    quest.TimeLimit = new int[5];
                    for (var i = 0; i < 5; i++)
                        quest.TimeLimit[i] = Convert.ToInt32(reader.ReadUnicode());

                    quest.MinSpeed = new int[5];
                    for (var i = 0; i < 5; i++)
                        quest.MinSpeed[i] = Convert.ToInt32(reader.ReadUnicode());

                    quest.MaxSpeed = new int[5];
                    for (var i = 0; i < 5; i++)
                        quest.MaxSpeed[i] = Convert.ToInt32(reader.ReadUnicode());

                    quest.MinLadius = new int[5];
                    for (var i = 0; i < 5; i++)
                        quest.MinLadius[i] = Convert.ToInt32(reader.ReadUnicode());

                    quest.MaxLadius = new int[5];
                    for (var i = 0; i < 5; i++)
                        quest.MaxLadius[i] = Convert.ToInt32(reader.ReadUnicode());

                    quest.QuestPath_01 = Convert.ToUInt32(reader.ReadUnicode());
                    quest.QuestPath_02 = Convert.ToUInt32(reader.ReadUnicode());
                    quest.Car_01 = Convert.ToUInt32(reader.ReadUnicode());
                    quest.Car_02 = Convert.ToUInt32(reader.ReadUnicode());
                    quest.ClearQuestIdN = Convert.ToUInt32(reader.ReadUnicode());
                    quest.Count = Convert.ToInt32(reader.ReadUnicode());
                    quest.RewardExp = Convert.ToInt32(reader.ReadUnicode());
                    quest.RewardMoney = Convert.ToInt32(reader.ReadUnicode());

                    quest.Item01 = reader.ReadUnicode();
                    quest.Item02 = reader.ReadUnicode();
                    quest.Item03 = reader.ReadUnicode();

                    // TODO: Get Icon by Index
                    //quest.GivePostPtr = GetIconByIndex(quest.GivePost);
                    //quest.EndPostPtr = GetIconByIndex(quest.EndPost);
                    /*
                    v4 = BS_SingletonHeap < XiIconTable,5 >::GetInstance();
                    v28.GivePostPtr = XiIconTable::GetIconByIndex(v4, v28.GivePost);
                    v5 = BS_SingletonHeap < XiIconTable,5 >::GetInstance();
                    v28.EndPostPtr = XiIconTable::GetIconByIndex(v5, v28.EndPost);
                    */

                    // TODO: Get Item by Id
                    /*
                    v6 = BS_SingletonHeap < XiItemTable,5 >::GetInstance();
                    v28.Item01Ptr = XiItemTable::GetItemByID(v6, v28.Item01);
                    v7 = BS_SingletonHeap < XiItemTable,5 >::GetInstance();
                    v28.Item02Ptr = XiItemTable::GetItemByID(v7, v28.Item02);
                    v8 = BS_SingletonHeap < XiItemTable,5 >::GetInstance();
                    v28.Item03Ptr = XiItemTable::GetItemByID(v8, v28.Item03);
                    v9 = BS_SingletonHeap < XiVisualItemTable,5 >::GetInstance();
                    v29 = XiVisualItemTable::GetVisualItemInfo(v9, v28.Item01);
                    if (v29)
                        v28.VSItemPtr = v29;
                    if (v28.Item01Ptr)
                        ++v28.RewardItemNum;
                    if (v28.Item02Ptr)
                        ++v28.RewardItemNum;
                    if (v28.Item03Ptr)
                        ++v28.RewardItemNum;
                    */

                    questList.Add(quest.QuestIdN, quest);
                }
            }

            return questList;
        }
    }
}