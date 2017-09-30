using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Shared.Objects
{
    /// <summary>
    /// TODO: Combine Items.xml and UseItems.xml
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "UseItems")]
    public class UseItemTable
    {
        public class UseItem : BasicItem
        {
            [XmlAttribute("maxstack")] public string MaxStack;

            [XmlAttribute("stat")] public string StatModifier;

            [XmlAttribute("cooldown")] public string CooldownTime;

            [XmlAttribute("duration")] public string Duration;

            public override bool IsStackable() => Category != "car";

            public override uint GetMaxStack()
            {
                if (MaxStack == "n/a" || MaxStack == "0")
                    return 99;
                return Convert.ToUInt32(MaxStack);
            }
        }
        
        [XmlElement(ElementName = "UseItem")]
        public List<UseItem> UseItemList = new List<UseItem>();
        
        public void Save(string fileName)
        {
            var serializer = new XmlSerializer(typeof(UseItemTable));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            using (var writer = new StreamWriter("UseItems.xml"))
            {
                serializer.Serialize(writer, this, ns);
            }
        }
    }
}