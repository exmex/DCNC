using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Shared.Util;

namespace Shared.Objects.GameDatas
{
    [Serializable]
    [XmlRoot(ElementName = "Quests", Namespace = "")]
    public class QuestTable
    {
        public class Quest
        {
            [XmlAttribute("id")] public string Id;

            [XmlAttribute("missiontype")] public string MissionType;

            [XmlAttribute("tableindex")] public int TableIndex;

            [XmlAttribute("title")] public string Title;

            [XmlAttribute("introprompt")] public string IntroPrompt;
            [XmlAttribute("station")] public string StartStation;

            [XmlAttribute("completestation")] public string CompleteStation;
            [XmlAttribute("completeprompt")] public string CompletePrompt;

            [XmlAttribute("objective1")] public string Objective1;
            [XmlAttribute("objective2")] public string Objective2;
            [XmlAttribute("objective3")] public string Objective3;
            [XmlAttribute("objective4")] public string Objective4;
            [XmlAttribute("objective5")] public string Objective5;

            [XmlAttribute("exp")] public int Experience;
            [XmlAttribute("mito")] public int Mito;

            [XmlAttribute("reward1")] public string RewardItem1;
            [XmlAttribute("reward2")] public string RewardItem2;
            [XmlAttribute("reward3")] public string RewardItem3;

            public string[] GetRewards()
            {
                var rewards = new List<string>();
                if (RewardItem1 != "0")
                    rewards.Add(RewardItem1);
                if (RewardItem2 != "0")
                    rewards.Add(RewardItem2);
                if (RewardItem3 != "0")
                    rewards.Add(RewardItem3);
                return rewards.ToArray();
            }
        }

        [XmlElement(ElementName = "Quest")] public List<Quest> QuestList = new List<Quest>();

        public void Save(string fileName)
        {
            var serializer = new XmlSerializer(typeof(QuestTable));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            using (var writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, this, ns);
            }
        }
    }
}