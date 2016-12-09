using Shared.Util.Configuration;

namespace AuthServer.Util
{
    public class AuthConf : BaseConf
    {
        /// <summary>
        /// login.conf
        /// </summary>
        public AuthConfFile Auth { get; protected set; }

        public AuthConf()
        {
            Auth = new AuthConfFile();
        }

        public override void Load()
        {
            LoadDefault();
            Auth.Load();
        }
    }

    /// <summary>
    /// Represents login.conf
    /// </summary>
    public class AuthConfFile : ConfFile
    {
        public int Port { get; protected set; }

        public bool NewAccounts { get; protected set; }

        public void Load()
        {
            Require("system/conf/auth.conf");

            Port = GetInt("port", 11005);
            NewAccounts = GetBool("new_accounts", true);
        }
    }
}
