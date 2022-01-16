using Shared.Util;


namespace Shared.Objects
{
    public class XiChant : BinaryWriterExt.ISerializable
    {
        public uint m_nType;
        public uint m_nValue;
        public uint m_nEndTime;

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(m_nType);
            writer.Write(m_nValue);
            writer.Write(m_nEndTime);
            

        }

    }
}
