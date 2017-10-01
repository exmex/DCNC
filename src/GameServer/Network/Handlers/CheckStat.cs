using Shared.Network;
using Shared.Network.GameServer;

namespace GameServer.Network.Handlers
{
    public class CheckStat
    {
        [Packet(Packets.CmdCheckStat)]
        public static void Handle(Packet packet)
        {
            var ack = new CheckStatAnswer()
            {
                /*CarSpeed = 0,
                CarDurability = 0,
                CarAcceleration = 0,
                CarBoost = 0,

                PartSpeed = 0,
                PartDurability = 0,
                PartAcceleration = 0,
                PartBoost = 0,

                UserSpeed = 0,
                UserDurability = 0,
                UserAcceleration = 0,
                UserBoost = 0,

                CharSpeed = 0,
                CharDurability = 0,
                CharAcceleration = 0,
                CharBoost = 0,

                ItemUseSpeed = 0,
                ItemUseCrash = 0,
                ItemUseAcceleration = 0,
                ItemUseBoost = 0,

                VehicleSpeed = 0,
                VehicleDurability = 0,
                VehicleAcceleration = 0,
                VehicleBoost = 0,*/
				// TODO: Nothing.
            };

            packet.Sender.Send(ack.CreatePacket());
            /*
            Send_StatUpdate:
                XiStrStatInfo StatInfo;
                XiStrEnChantBonus EnChantBonus;
                
            struct XiStrEnChantBonus
{
  int Speed;
  int Crash;
  int Accel;
  int Boost;
  float Drop;
  float Exp;
  float MitronCapacity;
  float MitronEfficiency;
};

struct XiStrStatInfo
{
  int BasedSpeed;
  int BasedCrash;
  int BasedAccel;
  int BasedBoost;
  int EquipSpeed;
  int EquipCrash;
  int EquipAccel;
  int EquipBoost;
  int CharSpeed;
  int CharCrash;
  int CharAccel;
  int CharBoost;
  int ItemUseSpeed;
  int ItemUseCrash;
  int ItemUseAccel;
  int ItemUseBoost;
  int TotalSpeed;
  int TotalCrash;
  int TotalAccel;
  int TotalBoost;
};
            
            XiCsCharInfo::StatUpdate(pCharInfo);
            PacketSend::Send_StatUpdate(lpDispatch);
            PacketSend::Send_PartyEnChantUpdateAll(lpDispatch); // TODO
            */
        }
    }
}