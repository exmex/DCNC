using System;
using System.Collections.Generic;
using System.IO;
using Shared.Database;
using Shared.Objects;
using Shared.Objects.GameDatas;
using Shared.Util;
using Shared.Util.Configuration;

namespace Shared
{
    /// <summary>
    ///     General methods needed by all servers.
    /// </summary>
    public abstract class ServerMain
    {
        public const int ProtocolVersion = 10249;

        public static List<VShopItemList.VShopItem> VisualItems;
        public static List<VehicleList.VehicleData> Vehicles;
        public static List<QuestTable.Quest> Quests;
        public static List<BasicItem> Items;
        public static Dictionary<int, KeyValuePair<ushort, long>> LevelTable;

        /// <summary>
        ///     Tries to find root folder and changes the working directory to it.
        ///     Exits if not successful.
        /// </summary>
        protected static void NavigateToRoot()
        {
            // Go back max 2 folders, the bins should be in /bin/(Debug|Release)
            for (var i = 0; i < 3; ++i)
            {
                if (Directory.Exists("system"))
                    return;

                Directory.SetCurrentDirectory("..");
            }

            Log.Error("Unable to find root directory.");
            ConsoleUtil.Exit(1);
        }

        /// <summary>
        ///     Tries to call conf's load method, exits on error.
        /// </summary>
        protected static void LoadConf(BaseConf conf)
        {
            Log.Info("Reading configuration...");

            try
            {
                conf.Load();
            }
            catch (Exception ex)
            {
                Log.Exception(ex, "Unable to read configuration. ({0})", ex.Message);
                ConsoleUtil.Exit(1);
            }
        }

        /// <summary>
        ///     Tries to initialize database with the information from conf,
        ///     exits on error.
        /// </summary>
        protected static void InitDatabase(BaseDatabase db, BaseConf conf)
        {
            Log.Info("Initializing database...");

            try
            {
                db.Init(conf.Database.Host, conf.Database.Port, conf.Database.User, conf.Database.Pass,
                    conf.Database.Db);
            }
            catch (Exception ex)
            {
                Log.Error("Unable to open database connection. ({0})", ex.Message);
                ConsoleUtil.Exit(1);
            }
        }
    }
}