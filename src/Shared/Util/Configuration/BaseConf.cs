using Shared.Util.Configuration.Files;

namespace Shared.Util.Configuration
{
    public abstract class BaseConf : ConfFile
    {
        /// <summary>
        /// log.conf
        /// </summary>
        public LogConfFile Log { get; protected set; }

        /// <summary>
        /// database.conf
        /// </summary>
        public DatabaseConfFile Database { get; private set; }

        protected BaseConf()
        {
            Log = new LogConfFile();
            Database = new DatabaseConfFile();
        }

        /// <summary>
        /// Loads several conf files that are generally required,
        /// like log, database, etc.
        /// </summary>
        protected void LoadDefault()
        {
            Log.Load();
            Database.Load();
        }

        public abstract void Load();
    }
}
