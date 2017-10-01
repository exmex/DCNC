using System;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace Shared.Objects
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
            team.OwnerId = Convert.ToUInt32(reader["CID"]);
            team.LeaderId = Convert.ToUInt32(reader["CID"]);
            team.OwnerName = ""; //reader["OwnerName"];
            team.LeaderName = ""; //reader["LeaderName"];

            return team;
        }
        
        public static Team Retrieve(MySqlConnection dbconn, long Tid)
        {
            var command = new MySqlCommand("SELECT * FROM Teams WHERE TID = @tid", dbconn);

            command.Parameters.AddWithValue("@tid", Tid);

            Team team;
            using (DbDataReader reader = command.ExecuteReader())
            {
                if (!reader.Read()) return null;
                team = GetTeam(reader);
            }

            return team;
        }
    }
}