using Shared.Util.Configuration;

namespace AuthServer.Util
{
    public class AuthConf : BaseConf
    {
        public AuthConf()
        {
            Auth = new AuthConfFile();
        }

        /// <summary>
        ///     login.conf
        /// </summary>
        public AuthConfFile Auth { get; protected set; }

        public override void Load()
        {
            LoadDefault();
            Auth.Load();
        }
    }

    /// <summary>
    ///     Represents login.conf
    /// </summary>
    public class AuthConfFile : ConfFile
    {
        public int Port { get; protected set; }

        public bool NewAccountsLogin { get; protected set; }

        public void Load()
        {
            Require("system/conf/auth.conf");

            Port = GetInt("port", 11005);
            NewAccountsLogin = GetBool("new_accounts_login", true);
        }
    }
}