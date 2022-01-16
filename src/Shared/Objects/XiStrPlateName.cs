using Shared.Util;


namespace Shared.Objects
{
    public class XiStrPlateName : BinaryWriterExt.ISerializable
    {
        public string m_Name; //10

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(m_Name);
            
        }

    }
}
