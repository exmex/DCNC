using Shared.Util;

namespace Shared.Objects
{
    public class VisualItem : BinaryWriterExt.ISerializable
    {
        public uint CarId;
        public int ItemState;
        public uint TableIdx;
        public uint InvenIdx;
        
        /// <summary>
        /// Unicode 9 Chars
        /// </summary>
        public string PlateName;
        public int Period;
        public int UpdateTime;
        public int CreateTime;
        
        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(CarId);
            writer.Write(ItemState);
            writer.Write(TableIdx);
            writer.Write(InvenIdx);
            writer.Write(PlateName);
            writer.Write(Period);
            writer.Write(UpdateTime);
            writer.Write(CreateTime);
        }
    }
}