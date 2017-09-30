using System;
using System.Collections.Generic;
using System.Data.Common;
using MySql.Data.MySqlClient;
using Shared.Database;
using Shared.Objects;
using Shared.Util;

namespace Shared.Models
{
    public class ItemModel
    {
        public static List<InventoryItem> RetrieveAll(MySqlConnection dbconn, ulong characterId)
        {
            var command = new MySqlCommand(
                "SELECT * FROM items WHERE CharacterId = @cid",
                dbconn);
            command.Parameters.AddWithValue("@cid", characterId);

            var items = new List<InventoryItem>();
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var item = InventoryItem.ReadFromDb(reader);
                    items.Add(item);
                }
            }
            return items;
        }
        
        public static void RetrieveAll(MySqlConnection dbconn, ref Character character)
        {
            var command = new MySqlCommand(
                "SELECT * FROM items WHERE CharacterId = @cid",
                dbconn);
            command.Parameters.AddWithValue("@cid", character.Id);
            
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var item = InventoryItem.ReadFromDb(reader);
                    character.InventoryItems.Add(item);
                    /*if (item.InventoryIndex > character.InventoryItems.Count)
                    {
                        Log.Error("Item slot <> Inventory length mismatch");
                        continue;
                    }
                    if (character.InventoryItems[(int)item.InventoryIndex] != null)
                    {
                        Log.Error("Duplicated inventory item!");
                        continue;
                    }
                    
                    character.InventoryItems[(int)item.InventoryIndex] = item;
                    //items.Add(item);
                    */
                }
            }
        }
        
        public static void Update(MySqlConnection dbconn, InventoryItem inventoryItem)
        {
            using (var cmd = new UpdateCommand("UPDATE items SET {0} WHERE Id=@id", dbconn))
            {
                cmd.AddParameter("@id", inventoryItem.DbId);
                var updateCommand = cmd;
                inventoryItem.WriteToDb(ref updateCommand);
                cmd.Execute();
            }
        }
        
        public static InventoryItem RetrieveOne(MySqlConnection dbconn, long id)
        {
            return null;
        }

        public static bool Create(MySqlConnection dbconn, InventoryItem item)
        {
            if (item.CharacterId == 0 || item.CarId == 0 || item.StackNum == 0)
                return false;
            
            using (var cmd = new InsertCommand("INSERT INTO `items` {0}", dbconn))
            {
                var insertCommand = cmd;
                item.WriteToDb(ref insertCommand);
                return cmd.Execute() == 1;
            }
        }

        public static bool Remove(MySqlConnection dbconn, ulong charId, int slot)
        {
            var command = new MySqlCommand("DELETE FROM `items` WHERE CharacterId = @cid AND InventoryIndex = @slot", dbconn);
            command.Parameters.AddWithValue("@slot", slot);
            command.Parameters.AddWithValue("@cid", charId);
            return command.ExecuteNonQuery() == 1;
        }
    }
}