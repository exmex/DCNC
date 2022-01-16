using Shared.Util;


namespace Shared.Objects
{
    public class XiDayEvent : BinaryWriterExt.ISerializable
    {
        public int m_DayMissionTodayCnt;
        public int m_DayMissionTotalCnt;
        public int m_DayMissionEventCnt;
        public int m_DayMileageTodayCnt;
        public int m_DayMileageTotalCnt;
        public int m_DayEventTodayCnt;
        public int m_DayEventTotalCnt;

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(m_DayMissionTodayCnt);
            writer.Write(m_DayMissionTotalCnt);
            writer.Write(m_DayMissionEventCnt);
            writer.Write(m_DayMileageTodayCnt);
            writer.Write(m_DayMileageTotalCnt);
            writer.Write(m_DayEventTodayCnt);
            writer.Write(m_DayEventTotalCnt);
            
        }

    }
}
