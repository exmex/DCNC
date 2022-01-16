using Shared.Util;


namespace Shared.Objects
{
    public class XiStrUserPermission : BinaryWriterExt.ISerializable
    {
        public uint m_Flag;

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(m_Flag);

        }

    }
}
