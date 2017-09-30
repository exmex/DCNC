using Shared.Util;

namespace Shared.Objects
{
    public class XiStrStatInfo : BinaryWriterExt.ISerializable
    {
        public int BasedAccel;
        public int BasedBoost;
        public int BasedCrash;
        public int BasedSpeed;
        public int CharAccel;
        public int CharBoost;
        public int CharCrash;
        public int CharSpeed;
        public int EquipAccel;
        public int EquipBoost;
        public int EquipCrash;
        public int EquipSpeed;
        public int ItemUseAccel;
        public int ItemUseBoost;
        public int ItemUseCrash;
        public int ItemUseSpeed;
        public int TotalAccel;
        public int TotalBoost;
        public int TotalCrash;
        public int TotalSpeed;


        /// <summary>
        /// TODO: Missing 32 Bytes!
        /// </summary>
        /// <param name="writer"></param>
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
            writer.Write(new byte[32]);
        }
    }
}