using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Shared.Util;

namespace Shared.Objects
{
    public class BasicItem
    {
        [XmlAttribute("id")] public string Id;

        [XmlAttribute("category")] public string Category;

        [XmlAttribute("name")] public string Name;

        [XmlAttribute("description")] public string Description;

        [XmlAttribute("function")] public string Function;

        [XmlAttribute("nextstate")] public string NextState;

        [XmlAttribute("buyvalue")] public string BuyValue;

        [XmlAttribute("sellvalue")] public string SellValue;

        [XmlAttribute("expirationtime")] public string ExpirationTime;

        [XmlAttribute("auctionable")] public string Auctionable;

        [XmlAttribute("partsshop")] public string PartsShop;

        [XmlAttribute("sendable")] public string Sendable;
        
        virtual public bool IsStackable()
        {
            return false;
        }

        virtual public uint GetMaxStack()
        {
            return 1;
        }
    }

    [Serializable]
    [XmlRoot(ElementName = "Items", Namespace = "")]
    public class ItemTable
    {
        public class ItemData : BasicItem
        {
            [XmlAttribute("setid")] public string SetId;

            [XmlAttribute("setname")] public string SetName;

            [XmlAttribute("grade")] public string Grade;

            [XmlAttribute("requiredLevel")] public string RequiredLevel;

            [XmlAttribute("basepoints")] public string BasePoints;

            [XmlAttribute("basepointmodifier")] public string BasePointModifier;

            [XmlAttribute("basepointvariable")] public string BasePointVariable;

            [XmlAttribute("partassist")] public string PartAssist;

            [XmlAttribute("lube")] public string Lube;

            [XmlAttribute("neostats")] public string NeoStats;
        }

        [XmlElement(ElementName = "Item")] public List<ItemData> ItemList = new List<ItemData>();

        public void LoadTdf(string fileName)
        {
            var tdfFile = new TdfReader();
            tdfFile.Load(fileName);

            using (var reader = new BinaryReaderExt(new MemoryStream(tdfFile.ResTable)))
            {
                for (var row = 0; row < tdfFile.Header.Row; row++)
                {
                    var item = new ItemData();
                    reader.ReadUnicode(); // Empty
                    reader.ReadUnicode(); // Type
                    reader.ReadUnicode(); // Set Type
                    item.Id = reader.ReadUnicode();
                    item.Category = reader.ReadUnicode();
                    item.Name = reader.ReadUnicode();
                    reader.ReadUnicode();
                    //item.Function
                    item.Grade = reader.ReadUnicode();
                    item.RequiredLevel = reader.ReadUnicode();
                    reader.ReadUnicode(); //???
                    //item.NextState
                    item.BasePoints = reader.ReadUnicode(); // Value
                    item.BasePointModifier = reader.ReadUnicode(); // Min
                    item.BasePointVariable = reader.ReadUnicode(); // max
                    item.BuyValue = reader.ReadUnicode();
                    item.SellValue = reader.ReadUnicode();
                    reader.ReadUnicode(); // Next id
                    item.PartsShop = reader.ReadUnicode();
                    item.Sendable = reader.ReadUnicode();
                    item.Auctionable = reader.ReadUnicode();
                    reader.ReadUnicode(); // Set rate
                    item.Description = reader.ReadUnicode(); // Set desc
                    item.PartAssist = reader.ReadUnicode(); // Set assist
                    //
                    item.ExpirationTime = reader.ReadUnicode();
                    ItemList.Add(item);
                }
            }
        }

        public static ItemTable Load(string fileName)
        {
            var serializer = new XmlSerializer(typeof(ItemTable));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            ItemTable item;
            using (var reader = new StreamReader(fileName))
            {
                item = (ItemTable) serializer.Deserialize(reader);
            }
            return item;
        }

        public void Save(string fileName)
        {
            var serializer = new XmlSerializer(typeof(ItemTable));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            using (var writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, this, ns);
            }
        }
    }
}