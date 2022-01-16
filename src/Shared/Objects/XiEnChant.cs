using Shared.Util;

namespace Shared.Objects
{
    public class XiEnChant : BinaryWriterExt.ISerializable
    {
        public uint m_nType;
        public uint m_nValue;

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(m_nType);
            writer.Write(m_nValue);
            
        }

    }
}
