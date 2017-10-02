using System;
using Shared.Network;
using Shared.Network.AuthServer;
using Shared.Network.GameServer;

namespace GameServer.Network.Handlers
{
    public class GetDateTime
    {
        /// <summary>
        /// TODO: Check if this is correct. Since I feel like it isn't lol
        /// </summary>
        /// <param name="packet"></param>
        [Packet(Packets.CmdGetDateTime)]
        public static void Handle(Packet packet)
        {
            /*var unixTimeNow = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            var now = DateTime.UtcNow.ToLocalTime();*/
            var now = DateTimeOffset.Now;
            var unixTimeNow = now.ToUnixTimeSeconds();

            var getDateTimePacket = new GetDateTimePacket(packet);

            var ack = new GetDateTimePacketAnswer
            {
                Action = getDateTimePacket.Action,
                GlobalTime = getDateTimePacket.GlobalTime,
                LocalTime = getDateTimePacket.LocalTime,
                TotalSeconds = (int) now.ToUnixTimeSeconds(),
                ServerTickTime = 0,
                ServerTick = 0,
                DayOfYear = (short) now.DayOfYear,
                Month = (short) now.Month,
                Day = (short) now.Day,
                DayOfWeek = (short) now.DayOfWeek,
                Hour = (byte) now.Hour,
                Minute = (byte) now.Minute,
                Second = (byte) now.Second
            };

            packet.Sender.Send(ack.CreatePacket());

            /*
              *(_DWORD *)(msg + 2) = lpMsg->Action;
              *(_DWORD *)(msg + 6) = lpMsg->GlobalTime;
              *(float *)(msg + 10) = lpMsg->LocalTime;
              *(_DWORD *)(msg + 14) = now;
              * *(_DWORD *)(msg + 18) = GetServerTickTime();
              *(_DWORD *)(msg + 22) = GetServerTick();
              *(_WORD *)(msg + 26) = ptm->tm_yday;
              *(_WORD *)(msg + 28) = ptm->tm_mon;
              *(_WORD *)(msg + 30) = ptm->tm_mday;
              *(_WORD *)(msg + 32) = ptm->tm_wday;
              *(_BYTE *)(msg + 34) = ptm->tm_hour;
              *(_BYTE *)(msg + 35) = ptm->tm_min;
              *(_BYTE *)(msg + 36) = ptm->tm_sec;
              *(_BYTE *)(msg + 37) = 0;
             */
        }
    }
}