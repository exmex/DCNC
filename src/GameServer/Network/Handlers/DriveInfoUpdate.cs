using Shared.Models;
using Shared.Network;
using Shared.Network.GameServer;

namespace GameServer.Network.Handlers
{
    public class DriveInfoUpdate
    {
        [Packet(Packets.CmdDriveInfoUpdate)]
        public static void Handle(Packet packet)
        {
            var driveInfo = new DriveInfoPacket(packet);

            var fDeltaFuel = packet.Sender.User.ActiveCharacter.ActiveCar.Mitron - driveInfo.TotalFuel;
            if (fDeltaFuel > 0.0f)
                packet.Sender.User.ActiveCharacter.ActiveCar.Mitron -= fDeltaFuel;
            var fDelta = driveInfo.TotalDistance - packet.Sender.User.ActiveCharacter.ActiveCar.Kmh;
            if (fDelta > 0.0f)
            {
                packet.Sender.User.ActiveCharacter.ActiveCar.Kmh += fDelta;
                packet.Sender.User.ActiveCharacter.TotalDistance += fDelta;
            }

            if (packet.Sender.User.ActiveCharacter.ActiveCar.Mitron <= 0.0f)
                packet.Sender.User.ActiveCharacter.ActiveCar.Mitron = 0.0f;

            // Save car to db.
            VehicleModel.Update(GameServer.Instance.Database.Connection, packet.Sender.User.ActiveCharacter.ActiveCar);


            /*
            fDeltaFuel = pCarInfo->m_CarInfo.CarUnit.Mitron - *(float *)(lpBuffer + 14);
            if ( fDeltaFuel > 0.0 )
              pCarInfo->m_CarInfo.CarUnit.Mitron = pCarInfo->m_CarInfo.CarUnit.Mitron - fDeltaFuel;
            fDelta = *(float *)(lpBuffer + 10) - pCarInfo->m_CarInfo.CarUnit.Kmh;
            if ( fDelta > 0.0 )
            {
              pCarInfo->m_CarInfo.CarUnit.Kmh = pCarInfo->m_CarInfo.CarUnit.Kmh + fDelta;
              pCharInfo->m_CharInfo.TotalDistance = pCharInfo->m_CharInfo.TotalDistance + fDelta;
              pCarInfo->m_fFuelConsume = pCarInfo->m_fFuelConsume + fDelta;
            }
            pGame->m_lastUpdateTime = GetSystemTick();
            if ( pCarInfo->m_CarInfo.CarUnit.Mitron < 0.0 )
              pCarInfo->m_CarInfo.CarUnit.Mitron = *(float *)&FLOAT_0_0;
                */
        }
    }
}