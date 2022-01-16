using Shared.Util;


namespace Shared.Objects
{
    public class XiPvpEventCounter : BinaryWriterExt.ISerializable
    {
        public ushort nOvervoltage;
        public ushort nColWall;
        public ushort nColTraffic;
        public ushort nColEtc;
        public float fBoosterRun;
        public float fFirstRun;
        public ushort nPoint;//4
        public ushort nItem;
        public ushort nReward; //8
        public ushort nRevCombo;
        public ushort nAcrobatCombo;
        public ushort nDriftCombo;
        public ushort nJumpCombo;


        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(nOvervoltage);
            writer.Write(nColWall);
            writer.Write(nColTraffic);
            writer.Write(nColEtc);
            writer.Write(fBoosterRun);
            writer.Write(fFirstRun);
            writer.Write(nPoint);
            writer.Write(nItem);
            writer.Write(nReward);
            writer.Write(nRevCombo);
            writer.Write(nAcrobatCombo);
            writer.Write(nDriftCombo);
            writer.Write(nJumpCombo);
            
        }
    }
}
