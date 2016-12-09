using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Shared;
using Shared.Database;
using Shared.Util;

namespace AuthServer.Database
{
    public class AuthDatabase : BaseDatabase
    {
        /// <summary>
        /// Checks whether the SQL update file has already been applied.
        /// </summary>
        /// <param name="updateFile"></param>
        /// <returns></returns>
        public bool CheckUpdate(string updateFile)
        {
            using (var conn = this.Connection)
            using (var mc = new MySqlCommand("SELECT * FROM `updates` WHERE `path` = @path", conn))
            {
                mc.Parameters.AddWithValue("@path", updateFile);

                using (var reader = mc.ExecuteReader())
                    return reader.Read();
            }
        }

        /// <summary>
        /// Executes SQL update file.
        /// </summary>
        /// <param name="updateFile"></param>
        public void RunUpdate(string updateFile)
        {
            try
            {
                using (var conn = this.Connection)
                {
                    // Run update
                    using (var cmd = new MySqlCommand(File.ReadAllText(Path.Combine("sql", updateFile)), conn))
                        cmd.ExecuteNonQuery();

                    // Log update
                    using (var cmd = new InsertCommand("INSERT INTO `updates` {0}", conn))
                    {
                        cmd.Set("path", updateFile);
                        cmd.Execute();
                    }

                    Log.Info("Successfully applied '{0}'.", updateFile);
                }
            }
            catch (Exception ex)
            {
                Log.Error("RunUpdate: Failed to run '{0}': {1}", updateFile, ex.Message);
                ConsoleUtil.Exit(1);
            }
        }
    }
}
