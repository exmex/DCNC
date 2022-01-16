using Shared.Util;


namespace Shared.Objects
{
    public class XiStrMySTItem : BinaryWriterExt.ISerializable
    {
        public uint CarID;
        public uint InvenIdx;
        public string StickerName;//32
        public XiSticker ItemUnit;

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(CarID);
            writer.Write(InvenIdx);
            writer.Write(StickerName);
            writer.Write(ItemUnit);
            
        }
    }
}
