using System;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace Shared.Objects
{
    public class TeamModel
    {
        public static Team Retrieve(MySqlConnection dbconn, long Tid)
        {
            var command = new MySqlCommand("SELECT * FROM Teams WHERE TID = @tid", dbconn);

            command.Parameters.AddWithValue("@tid", Tid);

            var team = new Team();

            using (DbDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    team.TeamId = Convert.ToInt64(reader["TID"]);
                    team.TeamMarkId = Convert.ToInt64(reader["TMARKID"]);
                    team.TeamName = reader["TEAMNAME"] as string;
                    team.TeamDesc = reader["TEAMDESC"] as string;
                    team.TeamUrl = reader["TEAMURL"] as string;
                    team.CreateDate = Convert.ToUInt32(reader["CREATEDATE"]);
                    team.CloseDate = Convert.ToUInt32(reader["CLOSEDATE"]);
                    team.BanishDate = Convert.ToUInt32(reader["BANISHDATE"]);
                    team.OwnChannel = reader["OWNCHANNEL"] as string;
                    team.TeamState = reader["TEAMSTATE"] as string;
                    team.TeamRanking = Convert.ToUInt32(reader["TEAMRANKING"]);
                    team.TeamPoint = Convert.ToUInt32(reader["TEAMPOINT"]);
                    team.ChannelWinCnt = Convert.ToUInt32(reader["CHANNELWINCNT"]);
                    team.MemberCnt = Convert.ToUInt32(reader["MEMBERCNT"]);
                    team.TeamTotalExp = 0L; //Convert.ToInt64(reader["TEAMTOTALEXP"]);
                    team.TeamTotalMoney = 0L; //reader["TeamTotalMoney"];
                    team.Version = 0; //reader["Version"];
                    team.OwnerId = Convert.ToUInt32(reader["CID"]);
                    team.LeaderId = Convert.ToUInt32(reader["CID"]);
                    team.OwnerName = ""; //reader["OwnerName"];
                    team.LeaderName = ""; //reader["LeaderName"];
                }
            }

            return team;
        }
    }
}