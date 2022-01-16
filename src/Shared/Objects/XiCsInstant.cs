using Shared.Util;


namespace Shared.Objects
{
    public class XiCsInstant : BinaryWriterExt.ISerializable
    {
        public XiStrInstant m_pInstant;
        public XiStrMyInstant m_MyInstant;
        public uint StartTime;
        public int CheckCount;
        public uint CheckTime; //200

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write((BinaryWriterExt.ISerializable)m_pInstant);
            writer.Write(m_MyInstant);
            writer.Write(StartTime);
            writer.Write(CheckCount);
            writer.Write(CheckTime);

        }


    }
}
