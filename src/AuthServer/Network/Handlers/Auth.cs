using Shared.Models;
using Shared.Network;
using Shared.Network.AuthServer;
using Shared.Objects;
using Shared.Util;

namespace AuthServer.Network.Handlers
{
    public static class Authentication
    {
        [Packet(Packets.CmdUserAuth)]
        public static void UserAuth(Packet packet)
        {
            UserAuthPacket authPacket = new UserAuthPacket(packet);

            Log.Debug("Login (v{0}) request from {1}", authPacket.ProtocolVersion.ToString(), authPacket.Username);

            if (authPacket.ProtocolVersion < AuthServer.ProtocolVersion)
            {
                Log.Debug("Client too old?");
                packet.Sender.Error("Your client is outdated!");
            }

            if (!AccountModel.AccountExists(AuthServer.Instance.Database.Connection, authPacket.Username))
            {
                Log.Debug("Account {0} not found!", authPacket.Username);
                packet.Sender.Error("Invalid Username or password!");
                return;
            }

            User user = AccountModel.Retrieve(AuthServer.Instance.Database.Connection, authPacket.Username);
            if (user == null)
            {
                Log.Debug("Account {0} not found!", authPacket.Username);
                packet.Sender.Error("Invalid Username or password!");
                return;
            }
            var passwordHashed = Password.GenerateSaltedHash(authPacket.Password, user.Salt);
            if(passwordHashed != user.Password)
            {
                Log.Debug("Account {0} found but invalid password! ({1} ({2}) vs {3})", authPacket.Username, passwordHashed, user.Salt, user.Password);
                packet.Sender.Error("Invalid Username or password!");
                return;
            }

            uint ticket = AccountModel.CreateSession(AuthServer.Instance.Database.Connection, authPacket.Username);

            // Wrong protocol -> 20070

            /*var ack = new Packet(Packets.UserAuthAck);
            packet.Sender.Error("Invalid Username or password!");*/

            var ack = new UserAuthAnswerPacket
            {
                Ticket = ticket,
                Servers = ServerModel.Retrieve(AuthServer.Instance.Database.Connection).ToArray()
            };

            ack.Send(Packets.UserAuthAck, packet.Sender);
        }
    }
}

