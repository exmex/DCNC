using System;
using Shared.Network;
using Shared.Network.GameServer;

namespace GameServer.Network.Handlers
{
    public class CityLeaveCheck
    {
        //signed __int16 __usercall sub_52CAB0@<ax>(int a1@<ebp>, double a2@<st0>, int a3, int a4)
        [Packet(Packets.CmdCityLeaveCheck)]
        public static void Handle(Packet packet)
        {
            var cityLeaveCheckPacket = new CityLeaveCheckPacket(packet);

            Console.WriteLine(cityLeaveCheckPacket.Post2);

            var ack = new CityLeaveCheckAnswer
            {
                Result = 1,
                CityId = cityLeaveCheckPacket.CityId,
                Post1 = cityLeaveCheckPacket.Post1,
                Post2 = cityLeaveCheckPacket.Post2
            };
            packet.Sender.Send(ack.CreatePacket());

            /*
             * Used bytes are:
             * 2 - 6 -> Result? // int?
             * 6 - 10 -> CityId? // int?
             * 10 - ?? -> Some kind of string, maybe Gate? // 255?
             * 265 - ?? -> Some kind of string, maybe Gate? // 255?
             */
        }
    }
}