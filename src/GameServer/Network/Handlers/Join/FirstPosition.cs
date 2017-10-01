using System.Numerics;
using Shared.Network;
using Shared.Network.GameServer;
using Shared.Util;

namespace GameServer.Network.Handlers.Join
{
    public class FirstPosition
    {
        [Packet(Packets.CmdFirstPosition)] // TODO: Actual position and not just dummies
        public static void Handle(Packet packet)
        {
            var character = packet.Sender.User.ActiveCharacter;
            var ack = new MyCityPositionAnswer();
            ack.PositionState = character.PosState;
            if (packet.Sender.User.ActiveCharacter.PosState != 1)
            {
                if (packet.Sender.User.ActiveCharacter.PosState == 2)
                {
                    ack.City = character.City;
                    ack.LastChannel = RandomProvider.Get().Next(0, 10);
                    /*
                    if ( *(_BYTE *)&BS_Global::ContentsFlag.Main.0 >> 8 )
                        lpAckPkt->m_ChannelId = rand() % 2;
                    */
                    ack.Position = character.Position; // AreaPos?
                }
                else
                {
                    ack.City = 0;
                    ack.LastChannel = packet.Sender.User.ActiveCharacter.LastChannel;
                    ack.Position = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
                }
            }
            else
            {
                character.City = 1;
                character.PosState = 1;
                
                ack.City = 1;
                ack.Position.X = -2157.2f + 4.0f * RandomProvider.Get().Next(0, 10);
                ack.Position.Y = -205.05f + 4.0f * RandomProvider.Get().Next(0, 10);
                ack.Position.Z = 85.720001f + 4.0f * RandomProvider.Get().Next(0, 10);
                ack.Position.W = 90.967003f + 4.0f * RandomProvider.Get().Next(0, 10);
                ack.LastChannel = -1;
            }
                
            // Check dormant event?
            
            packet.Sender.Send(ack.CreatePacket());

            if (character.PosState == 1 && character.Level == 1)
            {
                // Recommend friend
            }
            
            // Team notify
            
            /*packet.Sender.Send(new FirstPositionAnswer()
            {
                City = packet.Sender.User.ActiveCharacter.City,
                LastChannel = packet.Sender.User.ActiveCharacter.LastChannel,
                Position = packet.Sender.User.ActiveCharacter.Position,
                PositionState = packet.Sender.User.ActiveCharacter.PosState
            }.CreatePacket());*/
        }
    }
}