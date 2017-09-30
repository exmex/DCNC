using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Shared.Objects.GameDatas
{
    public static class GameData
    {
        public static List<VehicleList.VehicleData> LoadVehicleData(string vehicleList)
        {
            var vehicles = new List<VehicleList.VehicleData>();
            
            var serializer = new XmlSerializer(typeof(VehicleList));

            using (var reader = new StreamReader(vehicleList))
            {
                var items = (VehicleList) serializer.Deserialize(reader);
                vehicles.AddRange(items.VehList);
            }
            return vehicles;
        }
        
        public static List<BasicItem> LoadItems(string itemFileName, string useItemFileName)
        {
            var basicItems = new List<BasicItem>();
            
            var serializer = new XmlSerializer(typeof(ItemTable));

            using (var reader = new StreamReader(itemFileName))
            {
                var items = (ItemTable) serializer.Deserialize(reader);
                basicItems.AddRange(items.ItemList);
            }
            
            serializer = new XmlSerializer(typeof(UseItemTable));
            UseItemTable useItems;
            using (var reader = new StreamReader(useItemFileName))
            {
                var items = (UseItemTable) serializer.Deserialize(reader);
                basicItems.AddRange(items.UseItemList);
            }
            return basicItems;
        }
        
        public static List<QuestTable.Quest> LoadQuests(string fileName)
        {
            var quests = new List<QuestTable.Quest>();
            
            var serializer = new XmlSerializer(typeof(QuestTable));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            using (var reader = new StreamReader(fileName))
            {
                var item = (QuestTable) serializer.Deserialize(reader);
                quests.AddRange(item.QuestList);
            }
            return quests;
        }
        
        public static List<VShopItemList.VShopItem> LoadVShopItems(string fileName)
        {
            var shopItems = new List<VShopItemList.VShopItem>();
            
            var serializer = new XmlSerializer(typeof(VShopItemList));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            using (var reader = new StreamReader(fileName))
            {
                var item = (VShopItemList) serializer.Deserialize(reader);
                shopItems.AddRange(item.Items);
            }
            return shopItems;
        }
    }
}