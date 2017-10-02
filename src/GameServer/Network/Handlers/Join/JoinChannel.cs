using Shared.Models;
using Shared.Network;
using Shared.Network.GameServer;

namespace GameServer.Network.Handlers.Join
{
    public class JoinChannel
    {
        [Packet(Packets.CmdJoinChannel)]
        public static void Handle(Packet packet)
        {
            var serial = GameServer.Instance.Server.LastSerial++;
            DefaultServer.ActiveSerials.Add(serial, packet.Sender.User);
            packet.Sender.User.VehicleSerial = serial;
            if (!AccountModel.UpdateVehicleSerial(GameServer.Instance.Database.Connection, packet.Sender.User.Id, serial))
            {
                packet.Sender.KillConnection("Failed to update serial.");
                return;
            }
            
            packet.Sender.Send(new JoinChannelAnswer()
            {
                ChannelName = "speeding",
                CharacterName = packet.Sender.User.ActiveCharacter.Name,
                Serial = (short)serial,
                SessionAge = 0,
            }.CreatePacket());

            packet.Sender.Send(new WeatherAnswer()
            {
                CurrentWeather = WeatherAnswer.Weather.Rain
            }.CreatePacket());
            
            // As per legal requirements, this shall not be removed!
            packet.Sender.SendChatMessage($"Server powered by DCNC (v{Shared.Util.Version.GetVersion()}) - GigaToni");
            // As per legal requirements, this shall not be removed!
        }
    }
}