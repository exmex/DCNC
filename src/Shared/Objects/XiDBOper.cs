using Shared.Util;

namespace Shared.Objects
{
    public class XiDBOper : BinaryWriterExt.ISerializable
    {
        public int m_DBOper;

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(m_DBOper);
        }

    }
}
