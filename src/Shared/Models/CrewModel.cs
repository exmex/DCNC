using System;
using System.Data.Common;
using MySql.Data.MySqlClient;
using Shared.Database;
using Shared.Objects;

namespace Shared.Models
{
    public static class CrewModel
    {
        public static Crew GetTeam(DbDataReader reader)
        {
            var team = new Crew();
            
            team.Id = Convert.ToInt64(reader["TID"]);
            team.MarkId = Convert.ToInt64(reader["TMARKID"]);
            team.Name = reader["TEAMNAME"] as string;
            team.Description = reader["TEAMDESC"] as string;
            team.Url = reader["TEAMURL"] as string;
            team.CreateDate = Convert.ToUInt32(reader["CREATEDATE"]);
            team.CloseDate = Convert.ToUInt32(reader["CLOSEDATE"]);
            team.BanishDate = Convert.ToUInt32(reader["BANISHDATE"]);
            team.OwnChannel = reader["OWNCHANNEL"] as string;
            team.State = reader["TEAMSTATE"] as string;
            team.Ranking = Convert.ToUInt32(reader["TEAMRANKING"]);
            team.Point = Convert.ToUInt32(reader["TEAMPOINT"]);
            team.ChannelWinCnt = Convert.ToUInt32(reader["CHANNELWINCNT"]);
            team.MemberCnt = Convert.ToUInt32(reader["MEMBERCNT"]);
            team.TotalExp = 0L; //Convert.ToInt64(reader["TEAMTOTALEXP"]);
            team.TotalMoney = 0L; //reader["TeamTotalMoney"];
            team.Version = 0; //reader["Version"];
            team.OwnerId = Convert.ToInt64(reader["CID"]);
            team.LeaderId = Convert.ToInt64(reader["CID"]);
            team.OwnerName = reader["CNAME"] as string;
            team.LeaderName = reader["CNAME"] as string;
            //team.LeaderName = ""; //reader["LeaderName"];

            return team;
        }
        
        public static Crew Retrieve(MySqlConnection dbconn, long tid)
        {
            var command = new MySqlCommand("SELECT * FROM Teams WHERE TID = @tid", dbconn);

            command.Parameters.AddWithValue("@tid", tid);

            Crew crew;
            using (DbDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read()) return null;
                crew = GetTeam(reader);
            }

            return crew;
        }

        public static bool CheckNameExists(MySqlConnection dbconn, string teamName)
        {
            var command = new MySqlCommand(
                "SELECT * FROM `teams` WHERE TEAMNAME = @teamName", dbconn);

            command.Parameters.AddWithValue("@teamName", teamName);

            using (DbDataReader reader = command.ExecuteReader())
            {
                return reader.HasRows;
            }
        }

        public static bool Create(MySqlConnection dbconn, ref Crew crew)
        {
            var result = false;
            using (var cmd = new InsertCommand("INSERT INTO `teams` {0}", dbconn))
            {
                cmd.Set("TMARKID", crew.MarkId);
                cmd.Set("TEAMNAME", crew.Name);
                cmd.Set("UTEAMNAME", crew.Name);
                cmd.Set("TEAMLEVEL", 0); // Why doesn't this get saved in Team?
                cmd.Set("TEAMPOINT", crew.Point);
                cmd.Set("CID", crew.OwnerId);
                cmd.Set("CNAME", crew.OwnerName);
                cmd.Set("MEMBERCNT", crew.MemberCnt);
                cmd.Set("CREATEDATE", DateTimeOffset.Now.ToUnixTimeSeconds());

                result = cmd.Execute() == 1;
                crew.Id = cmd.LastId;
            }
            return result;
        }
    }
}