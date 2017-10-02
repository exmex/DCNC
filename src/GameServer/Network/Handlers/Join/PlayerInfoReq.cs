using System.Collections.Generic;
using System.Linq;
using Shared.Network;
using Shared.Network.GameServer;
using Shared.Objects;
using Shared.Util;

namespace GameServer.Network.Handlers.Join
{
    public class PlayerInfoReq
    {
        [Packet(Packets.CmdPlayerInfoReq)]
        public static void Handle(Packet packet)
        {
            /* 
            amPerl: No known scenarios where the requested info count is > 1
            We'll see about that. For now just sending multiple if we have multiple.
            */
            
            var pinfoReq = new PlayerInfoReqPacket(packet);
            var clients = (List<Client>)GameServer.Instance.Server.GetClients(pinfoReq.VehicleSerials);
                
            var ack = new PlayerInfoOldAnswer();
            if (clients == null || clients.Count == 0)
            {
                Log.Error($"PlayerInfoReq: Requested serial {pinfoReq.VehicleSerials[0]} no client was found.");
                
                packet.Sender.Send(ack.CreatePacket());
                return;
            }
            var firstClient = clients[0];

            if (firstClient.User.ActiveCharacter == null) // Make sure we have loaded a char for first client
            {
                Log.Error("Clients character info was invalid.");
                
                packet.Sender.Send(ack.CreatePacket());
                return;
            }
            
            ack.PlayerInfo = new XiPlayerInfo()
            {
                CharacterName = firstClient.User.ActiveCharacter.Name,
                Serial = (ushort)firstClient.User.VehicleSerial,
                Age = 0
            };
            clients.RemoveAt(0); // Make sure we don't have a first player info anymore
            ack.PlayerInfos = new XiPlayerInfo[clients.Count];
            for (var i = 0; i < clients.Count; i++)
            {
                var client = clients[i];

                if (client.User.ActiveCharacter == null) // Make sure we have loaded a char
                {
                    Log.Error("Clients character info was invalid.");
                
                    ack.PlayerInfos[i] = ack.PlayerInfo = new XiPlayerInfo()
                    {
                        CharacterName = "InvalidChar",
                        Serial = (ushort)client.User.VehicleSerial,
                        Age = 0
                    };
                    continue;
                }
                
                ack.PlayerInfos[i] = ack.PlayerInfo = new XiPlayerInfo()
                {
                    CharacterName = client.User.ActiveCharacter.Name,
                    Serial = (ushort)client.User.VehicleSerial,
                    Age = 0
                };          
            }
            
            /* Send only one:
            var serial = packet.Reader.ReadUInt16(); // amPerl: No known scenarios where the requested info count is > 1
            
            var res = new Packet(Packets.PlayerInfoOldAck);

            var ack = new PlayerInfoOldAnswer();

            var client = GameServer.Instance.Server.GetClient(serial);
            if (client == null)
            {
                Log.Error($"No client with vehicle serial {serial} was found.");
                
                packet.Sender.Send(ack.CreatePacket());
                return;
            }

            if (client.User.ActiveCharacter == null)
            {
                Log.Error("Clients character info was invalid.");
                
                packet.Sender.Send(ack.CreatePacket());
                return;
            }
            
            var character = client.User.ActiveCharacter;
            ack.PlayerInfo = new XiPlayerInfo()
            {
                CharacterName = character.Name,
                Serial = (ushort)client.User.VehicleSerial,
                Age = 0
            };
            
            packet.Sender.Send(ack.CreatePacket());
            */
            
            // Old leaked Server sends also BS_PktStickerInfoRes
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