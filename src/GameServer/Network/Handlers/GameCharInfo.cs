using Shared.Models;
using Shared.Network;
using Shared.Network.GameServer;
using Shared.Objects;
using Shared.Util;

namespace GameServer.Network.Handlers
{
    public class GameCharInfo
    {
        [Packet(Packets.CmdGameCharInfo)]
        public static void Handle(Packet packet) // TODO: Send data correspoding to the charname, not user
        {
            /*
            [Debug] - GameCharInfo: 000000: 41 00 64 00 6D 00 69 00 6E 00 69 00 73 00 74 00  A · d · m · i · n · i · s · t ·
            000016: 72 00 61 00 74 00 6F 00 72 00 00 00 0A 00 06 00  r · a · t · o · r · · · · · · ·
            000032: 0D 25 33 65 63 65 69 76 65 64 00 00 00 00  · % 3 e c e i v e d · · · ·

            [Info] - Received GameCharInfo (id 660, 0x294) on 11021.

            Wrong Packet Size. CMD(661) CmdLen: : 1177, AnalysisSize: 831
            // We're missing 346 bytes of data.
            */
            var gameCharInfoPacket = new GameCharInfoPacket(packet);

            // TODO: Combine this into 1 MySQL Query.
            var character = CharacterModel.Retrieve(GameServer.Instance.Database.Connection,
                gameCharInfoPacket.CharacterName);
            if (character == null)
            {
                Log.Error($"Character {gameCharInfoPacket.CharacterName} was not found in DB!");
                packet.Sender.SendDebugError("Character not found");
#if !DEBUG
                packet.Sender.KillConnection("Character for CmdGameCharInfo not found");
#endif
                return;
            }
            var user = AccountModel.Retrieve(GameServer.Instance.Database.Connection, character.Uid);
            
            var ack = new GameCharInfoAnswer
            {
                Character = character,
                Vehicle = character.ActiveCar,
                StatisticInfo = new XiStrStatInfo(),
                Crew = character.Crew,
                Serial = user.VehicleSerial,
                //LocType = 'A',
                ChId = (char)character.LastChannel, //'A',
                //LocId = 1
            };
            packet.Sender.Send(ack.CreatePacket());
        }
    }
}