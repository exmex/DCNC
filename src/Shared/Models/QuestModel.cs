using System;
using System.Collections.Generic;
using System.Data.Common;
using MySql.Data.MySqlClient;
using Shared.Database;

namespace Shared.Models
{
    public class Quest
    {
        public ulong CharacterId;
        public string CharacterName;
        public short FailNum;
        public int LastDate;
        public int PlaceIdx;
        public uint QuestId;
        public int ServerId;
        public int State;
    }

    public class QuestModel
    {
        public static List<Quest> Retrieve(MySqlConnection dbconn, int serverId, ulong characterId)
        {
            var command = new MySqlCommand("SELECT * FROM quests WHERE CID=@charId AND ServerId=@serverId", dbconn);

            command.Parameters.AddWithValue("@serverId", serverId);
            command.Parameters.AddWithValue("@charId", characterId);

            var quests = new List<Quest>();
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var quest = new Quest
                    {
                        ServerId = Convert.ToInt32(reader["ServerId"]),
                        CharacterId = Convert.ToUInt64(reader["CID"]),
                        CharacterName = reader["CNAME"] as string,
                        QuestId = Convert.ToUInt32(reader["QuestId"]),
                        State = Convert.ToInt32(reader["State"]),
                        FailNum = Convert.ToInt16(reader["FailNum"]),
                        PlaceIdx = Convert.ToInt32(reader["PlaceIdx"]),
                        LastDate = Convert.ToInt32(reader["LastDate"])
                    };
                    quests.Add(quest);
                }
            }

            return quests;
        }

        public static Quest RetrieveOne(MySqlConnection dbconn, int serverId, ulong characterId, uint questId)
        {
            var command =
                new MySqlCommand("SELECT * FROM quests WHERE CID=@charId AND ServerId=@serverId AND QuestId=@questId",
                    dbconn);

            command.Parameters.AddWithValue("@serverId", serverId);
            command.Parameters.AddWithValue("@charId", characterId);
            command.Parameters.AddWithValue("@questId", questId);

            using (DbDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                    return new Quest
                    {
                        ServerId = Convert.ToInt32(reader["ServerId"]),
                        CharacterId = Convert.ToUInt64(reader["CID"]),
                        CharacterName = reader["CNAME"] as string,
                        QuestId = Convert.ToUInt32(reader["QuestId"]),
                        State = Convert.ToInt32(reader["State"]),
                        FailNum = Convert.ToInt16(reader["FailNum"]),
                        PlaceIdx = Convert.ToInt32(reader["PlaceIdx"]),
                        LastDate = Convert.ToInt32(reader["LastDate"])
                    };
            }

            return null;
        }

        public static void Add(MySqlConnection dbconn, Quest quest)
        {
            using (var cmd = new InsertCommand("INSERT INTO `Quests` {0}", dbconn))
            {
                cmd.Set("ServerId", quest.ServerId);
                cmd.Set("CID", quest.CharacterId);
                cmd.Set("CNAME", quest.CharacterName);
                cmd.Set("QuestId", quest.QuestId);
                cmd.Set("State", quest.State);
                cmd.Set("FailNum", quest.FailNum);
                cmd.Set("PlaceIdx", quest.PlaceIdx);
                cmd.Set("LastDate", DateTimeOffset.Now.ToUnixTimeSeconds());

                cmd.Execute();
            }
        }

        public static void Update(MySqlConnection dbconn, int serverId, ulong characterId, uint questId, int state)
        {
            using (var cmd =
                new UpdateCommand("UPDATE Quests SET {0} WHERE CID=@charId AND QuestId=@questId AND ServerId=@serverId",
                    dbconn))
            {
                cmd.AddParameter("@serverId", serverId);
                cmd.AddParameter("@charId", characterId);
                cmd.AddParameter("@questId", questId);

                cmd.Set("State", state);
                cmd.Execute();
            }
        }

        public static void Update(MySqlConnection dbconn, int serverId, ulong characterId, uint questId, Quest quest)
        {
            using (var cmd =
                new UpdateCommand("UPDATE Quests SET {0} WHERE CID=@charId AND QuestId=@questId AND ServerId=@serverId",
                    dbconn))
            {
                cmd.AddParameter("@serverId", serverId);
                cmd.AddParameter("@charId", characterId);
                cmd.AddParameter("@questId", questId);

                cmd.Set("ServerId", quest.ServerId);
                cmd.Set("CNAME", quest.CharacterName);
                cmd.Set("State", quest.State);
                cmd.Set("FailNum", quest.FailNum);
                cmd.Set("PlaceIdx", quest.PlaceIdx);
                cmd.Execute();
            }
        }

        public static void Delete(MySqlConnection dbconn, int serverId, ulong characterId, uint questId)
        {
            var command =
                new MySqlCommand("DELETE FROM Quests WHERE CID = @charId AND QuestId = @questId AND ServerId=@serverId",
                    dbconn);
            command.Parameters.AddWithValue("@serverId", serverId);
            command.Parameters.AddWithValue("@charId", characterId);
            command.Parameters.AddWithValue("@questId", questId);
            command.ExecuteNonQuery();
        }
    }
}