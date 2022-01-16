using Shared.Util;


namespace Shared.Objects
{
    public class XiStrMyItem : BinaryWriterExt.ISerializable
    {
        public uint CarID;
        //$90E2572CC35A071924DAD0BC1A98978B Itm;//TODO
        XiStrItemUnit ItemUnit;
        public uint TableIdx;
        public uint InvenIdx;

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(CarID);
           // writer.Write(Itm); TODO
            writer.Write(ItemUnit);
            writer.Write(TableIdx);
            writer.Write(InvenIdx);

        }

    }
}
