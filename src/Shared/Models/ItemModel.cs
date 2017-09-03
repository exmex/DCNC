using System;
using System.Collections.Generic;
using System.Data.Common;
using MySql.Data.MySqlClient;
using Shared.Database;
using Shared.Objects;

namespace Shared.Models
{
    public class ItemModel
    {
        public static List<Item> RetrieveAll(MySqlConnection dbconn, ulong characterId)
        {
            var command = new MySqlCommand(
                "SELECT * FROM items WHERE CharacterId = @cid",
                dbconn);
            command.Parameters.AddWithValue("@cid", characterId);

            var items = new List<Item>();
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var item = Item.ReadFromDb(reader);
                    items.Add(item);
                }
            }
            return items;
        }
        
        public static void Update(MySqlConnection dbconn, Item item)
        {
            using (var cmd = new UpdateCommand("UPDATE items SET {0} WHERE Id=@id", dbconn))
            {
                cmd.AddParameter("@id", item.DbId);
                var updateCommand = cmd;
                item.WriteToDb(ref updateCommand);
            }
        }
        
        public static Item RetrieveOne(MySqlConnection dbconn, long id)
        {
            return null;
        }
    }
}