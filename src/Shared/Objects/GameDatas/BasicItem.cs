using System.Xml.Serialization;

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
}