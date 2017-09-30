using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Shared.Objects.GameDatas
{
    [Serializable]
    [XmlRoot(ElementName = "Items")]
    public class ItemTable
    {
        public class Item : BasicItem
        {
            [XmlAttribute("setid")] public string SetID;

            [XmlAttribute("setname")] public string SetName;

            [XmlAttribute("grade")] public string Grade;

            [XmlAttribute("requiredLevel")]
            public string RequiredLevel;

            [XmlAttribute("basepoints")] public string BasePoints;

            [XmlAttribute("basepointmodifier")] public string BasePointModifier;

            [XmlAttribute("basepointvariable")] public string BasePointVariable;

            [XmlAttribute("partassist")] public string PartAssist;

            [XmlAttribute("lube")] public string Lube;

            [XmlAttribute("neostats")] public string NeoStats;
        }

        [XmlElement(ElementName = "Item")]
        public List<Item> ItemList = new List<Item>();

        public void Save(string fileName)
        {
            var serializer = new XmlSerializer(typeof(ItemTable));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            using (var writer = new StreamWriter("Items.xml"))
            {
                serializer.Serialize(writer, this, ns);
            }
        }
    }
}