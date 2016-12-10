using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Network;

namespace Shared.Objects
{
    public class StatInfo : ISerializable
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


        public void Serialize(PacketWriter writer)
        {
            writer.Write(BasedSpeed);
            writer.Write(BasedCrash);
            writer.Write(BasedAccel);
            writer.Write(BasedBoost);
            writer.Write(EquipSpeed);
            writer.Write(EquipCrash);
            writer.Write(EquipAccel);
            writer.Write(EquipBoost);
            writer.Write(CharSpeed);
            writer.Write(CharCrash);
            writer.Write(CharAccel);
            writer.Write(CharBoost);
            writer.Write(ItemUseSpeed);
            writer.Write(ItemUseCrash);
            writer.Write(ItemUseAccel);
            writer.Write(ItemUseBoost);
            writer.Write(TotalSpeed);
            writer.Write(TotalCrash);
            writer.Write(TotalAccel);
            writer.Write(TotalBoost);
        }
    }
}
