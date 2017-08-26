using Shared.Network;
using Shared.Util;

namespace Shared.Objects
{
    public class StatInfo : BinaryWriterExt.ISerializable
    {
        private int BasedAccel;
        private int BasedBoost;
        private int BasedCrash;
        private int BasedSpeed;
        private int CharAccel;
        private int CharBoost;
        private int CharCrash;
        private int CharSpeed;
        private int EquipAccel;
        private int EquipBoost;
        private int EquipCrash;
        private int EquipSpeed;
        private int ItemUseAccel;
        private int ItemUseBoost;
        private int ItemUseCrash;
        private int ItemUseSpeed;
        private int TotalAccel;
        private int TotalBoost;
        private int TotalCrash;
        private int TotalSpeed;


        public void Serialize(BinaryWriterExt writer)
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