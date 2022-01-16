using Shared.Util;


namespace Shared.Objects
{
    public class XiStrGiftMsg : BinaryWriterExt.ISerializable
    {
        public string m_Msg;//50

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(m_Msg);


        }
    }
}
