using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Shared.Objects.GameDatas
{
    [Serializable]
    [XmlRoot(ElementName = "VShopItems")]
    public class VShopItemList
    {
        public class VShopItem
        {
            [XmlAttribute("support")] public string Support;
            
            [XmlAttribute("id")] public string UniqueId;
            [XmlAttribute("item")] public string ItemName;

            [XmlAttribute("useMito")] public string UseMito;
            
            [XmlAttribute("mitoPrice")] public string MitoPrice;
            [XmlAttribute("mitoSell")] public string SellMitoPrice;
            [XmlAttribute("mito7dPrice")] public string Mito7dPrice;
            [XmlAttribute("mito30dPrice")] public string Mito30dPrice;
            [XmlAttribute("mito90dPrice")] public string Mito90dPrice;
            [XmlAttribute("mito365dPrice")] public string Mito365dPrice;
            [XmlAttribute("mito0dPrice")] public string Mito0dPrice;

            [XmlAttribute("useHancoin")] public string UseHancoin;
            [XmlAttribute("hancoin7dPrice")] public string Hancoin7dPrice;
            [XmlAttribute("hancoin30dPrice")] public string Hancoin30dPrice;
            [XmlAttribute("hancoin90dPrice")] public string Hancoin90dPrice;
            [XmlAttribute("hancoin3650dPrice")] public string Hancoin365dPrice;
            [XmlAttribute("hancoin0dPrice")] public string Hancoin0dPrice;
            /*
            0 Index
            1 Support
            2 UniqueId
            3 ItemName
            4 MarketName
            5 Desc (Tooltip)
            6 Top
            7 Top Category
            8 Main
            9 Category
            10 Sub
            11 Sub Catergory
            12 Name
            13 Refer VisualItem
            14 SellStage
            15 CloseStage
            16 UnEquipable
            17 PcRoomPart
            18 Feature
            19 Default Part
            20 Display Order
            21 Instant
            22 New
            23 Carshop Hot
            24 Event
            25 Hot
            26 Use Mito
            27 Mito Price
            28 Sell Mito
            29 UseHancoin
            30 Use Mileage
            31 Period 7D
            32 $ Price7D
            33 Mito Price7D
            34 Mile Price7D
            35 Bonus Mito 7D
            36 Period 30D
            37 $ Price30D
            38 Mito Price30D
            39 Mile Price30D
            40 Bonus Mito 30D
            41 Period 90D
            42 $ Price90D
            43 Mito Price90D
            44 Mile Price90D
            45 Bonus Mito 090D
            46 Period 365D
            47 $ Price365D
            48 Mito Price365D
            49 Mile Price365D
            50 Bonus Mito 365D
            51 Period 0D
            52 $ Price0D
            53 Mito Price0D
            54 Mile Price0D
            55 Bonus Mito 0D
            56 Bonus Speed
            57 Bonus Accel
            58 Bonus Boost
            59 Bonus Crash
            60 Bonus Assist
            61 Refund
            62 Itemcode_007
            63 Itemcode_030
            64 Itemcode_090
            65 Itemcode_365
            66 Itemcode_000
            67 web_sellable
            68 Itemcode_w007
            69 Itemcode_w030
            70 Itemcode_w090
            71 Itemcode_w365
            72 Itemcode_w000
            73 SellByCarType
            74 MagicNumber
            75
            76 New
            77 Hot
            78
            79 $ Price7D
            80 $ Price30D
            81 $ Price90D
            82 $ Price360D
            83 $ Price0D
            */
        }

        [XmlElement(ElementName = "VShopItem")]
        public List<VShopItem> Items = new List<VShopItem>();
        
        public void Save(string fileName)
        {
            var serializer = new XmlSerializer(typeof(VShopItemList));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            using (var writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, this, ns);
            }
        }
    }
}