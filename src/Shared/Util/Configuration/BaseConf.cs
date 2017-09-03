using Shared.Util.Configuration.Files;

namespace Shared.Util.Configuration
{
    public abstract class BaseConf : ConfFile
    {
        protected BaseConf()
        {
            Ip = new IPConfFile();
            Log = new LogConfFile();
            Database = new DatabaseConfFile();
        }

        /// <summary>
        ///     log.conf
        /// </summary>
        public LogConfFile Log { get; protected set; }
        
        public IPConfFile Ip { get; protected set; }

        /// <summary>
        ///     database.conf
        /// </summary>
        public DatabaseConfFile Database { get; }

        /// <summary>
        ///     Loads several conf files that are generally required,
        ///     like log, database, etc.
        /// </summary>
        protected void LoadDefault()
        {
            Ip.Load();
            Log.Load();
            Database.Load();
        }

        public abstract void Load();
    }
}