using Shared.Util;


namespace Shared.Objects
{
    public class XiStrTeamName : BinaryWriterExt.ISerializable
    {
        public string m_Name;//13

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(m_Name);


        }
    }
}
