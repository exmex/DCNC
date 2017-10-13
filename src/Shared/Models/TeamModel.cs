using System;
using System.Data.Common;
using MySql.Data.MySqlClient;
using Shared.Database;
using Shared.Objects;

namespace Shared.Models
{
    public class TeamModel
    {
        public static Team GetTeam(DbDataReader reader)
        {
            var team = new Team();
            
            team.TeamId = Convert.ToInt64(reader["TID"]);
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
        
        public static Team Retrieve(MySqlConnection dbconn, long tid)
        {
            var command = new MySqlCommand("SELECT * FROM Teams WHERE TID = @tid", dbconn);

            command.Parameters.AddWithValue("@tid", tid);

            Team team;
            using (DbDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read()) return null;
                team = GetTeam(reader);
            }

            return team;
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

        public static bool Create(MySqlConnection dbconn, ref Team team)
        {
            var result = false;
            using (var cmd = new InsertCommand("INSERT INTO `teams` {0}", dbconn))
            {
                cmd.Set("TMARKID", team.MarkId);
                cmd.Set("TEAMNAME", team.Name);
                cmd.Set("UTEAMNAME", team.Name);
                cmd.Set("TEAMLEVEL", 0); // Why doesn't this get saved in Team?
                cmd.Set("TEAMPOINT", team.Point);
                cmd.Set("CID", team.OwnerId);
                cmd.Set("CNAME", team.OwnerName);
                cmd.Set("MEMBERCNT", team.MemberCnt);
                cmd.Set("CREATEDATE", DateTimeOffset.Now.ToUnixTimeSeconds());

                result = cmd.Execute() == 1;
                team.TeamId = cmd.LastId;
            }
            return result;
        }
    }
}