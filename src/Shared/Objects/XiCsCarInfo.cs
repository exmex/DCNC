using Shared.Util;


namespace Shared.Objects
{
    public class XiCsCarInfo : BinaryWriterExt.ISerializable
    {
        public XiStrCarInfo m_CarInfo;
        public Vec4 m_CarPos;
        public int m_CityId;
        public float m_Rating;
        public XiVisualItem m_VisualItem;
        public float m_fFuelConsume;

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(m_CarInfo);
            writer.Write((BinaryWriterExt.ISerializable)m_CarPos);
            writer.Write(m_CityId);
            writer.Write(m_Rating);
            writer.Write(m_VisualItem);
            writer.Write(m_fFuelConsume);
        }

    }
}
