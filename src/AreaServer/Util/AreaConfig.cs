using Shared.Util.Configuration;

namespace AreaServer.Util
{
    public class AreaConf : BaseConf
    {
        public AreaConf()
        {
            Area = new AreaConfFile();
        }

        /// <summary>
        ///     login.conf
        /// </summary>
        public AreaConfFile Area { get; protected set; }

        public override void Load()
        {
            LoadDefault();
            Area.Load();
        }
    }

    /// <summary>
    ///     Represents login.conf
    /// </summary>
    public class AreaConfFile : ConfFile
    {
        public int Port { get; protected set; }

        public bool NewAccounts { get; protected set; }

        public void Load()
        {
            Require("system/conf/area.conf");

            Port = GetInt("port", 11031);
        }
    }
}