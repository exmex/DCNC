using Shared.Network;
using Shared.Objects;

namespace GameServer.Network.Handlers.Join
{
    public class PlayerInfoReq
    {
        [Packet(Packets.CmdPlayerInfoReq)]
        public static void Handle(Packet packet)
        {
            var reqCnt = packet.Reader.ReadUInt32();
            var serial = packet.Reader.ReadUInt16(); // No known scenarios where the requested info count is > 1
            
            foreach (var client in GameServer.Instance.Server.GetClients())
            {
                if (client.User.ActiveCharacter == null || client.User.VehicleSerial != serial) continue;
                
                var character = client.User.ActiveCharacter;
                var playerInfo = new XiPlayerInfo()
                {
                    CharacterName = character.Name,
                    Serial = (ushort)client.User.VehicleSerial,
                    Age = 0
                };
                var res = new Packet(Packets.PlayerInfoOldAck);
                res.Writer.Write(1); // Result
                res.Writer.Write(playerInfo);
                packet.Sender.Send(res);
                break;
            }
        }
        
        /* Pulled from Rice:
		[RicePacket(801, RiceServer.ServerType.Game)]
        public static void PlayerInfoReq(RicePacket packet)
        {
            var reqCnt = packet.Reader.ReadUInt32();
            var serial = packet.Reader.ReadUInt16(); // No known scenarios where the requested info count is > 1
            // Followed by the session age of the player we are requesting info for. nplutowhy.avi
 
            foreach(var p in RiceServer.GetPlayers())
            {
                if(p.ActiveCharacter != null && p.ActiveCharacter.CarSerial == serial)
                {
                    var character = p.ActiveCharacter;
                    var playerInfo = new Structures.PlayerInfo()
                    {
                        Name = character.Name,
                        Serial = character.CarSerial,
                        Age = 0
                    };
                    var res = new RicePacket(802);
                    res.Writer.Write(1);
                    res.Writer.Write(playerInfo);
                    packet.Sender.Send(res);
                    break;
                }
            }
        }
		*/
    }
}