using Shared.Util;
using Shared.Objects;

namespace Shared.Objects
{
    public class XiStrCharInfo : BinaryWriterExt.ISerializable
    {
        public enum FlagType {
            Beginner_Tutorial = 0x8000000,
            Battle_Tutorial = 0x4000000,
        }

        public enum POSITION_STATE {
            FIRST_POS = 0x0,
            NOTSAVED_POS = 0x1,
            HOUSESAVED_POS = 0x2,
            BASE_SAVED_POS = 0x3
        }

        public long Cid;
        // This is actually an insance of an XiStrCharName but eh
        public string Name; // Length: 21
        public int LastDate;
        public ushort Avatar;
        public ushort Level;
        public XiStrExpInfo ExpInfo;
        public long MitoMoney;
        public long TeamId;
        public long TeamMarkId;
        public string TeamName; // wchar_t* of length 13
        public int TeamRank;

        #region PVP Data
        public char PType;
        public uint PvpCnt;
        public uint PvpWinCnt;
        public uint PvpPoint;
        public uint TPvpCnt;
        public uint TPvpWinCnt;
        public uint TPvpPoint;
        public uint QuickCnt;
        #endregion

        public float TotalDistance;
        public float[] m_Position; // Note: X, Y, Z, Rotation
        public int m_LastChannel;
        public int m_City;
        public POSITION_STATE m_PosState;
        public int CurCarId;
        public uint QuickSlot1;
        public uint QuickSlot2;
        public int TeamJoinDate;
        public int TeamCloseDate;
        public int TeamLeaveDate;
        public int HancoinInven;
        public int HancoinGarage;
        
        public FlagType Flags;
        public int Guild;
        public long Mileage;

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(Cid);
            writer.WriteUnicodeStatic(Name, 21, true); // TODO: Figure this out (null termination or not)
            writer.Write(LastDate);
            writer.Write(Avatar);
            writer.Write(Level);
            ExpInfo.Serialize(writer);
            writer.Write(MitoMoney);
            writer.Write(TeamId);
            writer.Write(TeamMarkId);
            writer.WriteUnicodeStatic(TeamName, 13, true); // TODO: Figure this out (null termination or not)
            writer.Write(TeamRank);
            writer.Write(PType);
            writer.Write(PvpCnt);
            writer.Write(PvpWinCnt);
            writer.Write(PvpPoint);
            writer.Write(TPvpCnt);
            writer.Write(TPvpWinCnt);
            writer.Write(TPvpPoint);
            writer.Write(QuickCnt);
            writer.Write(TotalDistance);
            for (int i = 0; i < m_Position.Length; i++) {
                writer.Write(m_Position[i]);
            }
            writer.Write(m_LastChannel);
            writer.Write(m_City);
            writer.Write((int)m_PosState);
            writer.Write(CurCarId);
            writer.Write(QuickSlot1);
            writer.Write(QuickSlot2);
            writer.Write(TeamJoinDate);
            writer.Write(TeamCloseDate);
            writer.Write(TeamLeaveDate);
            writer.Write(HancoinInven);
            writer.Write(HancoinGarage);
            writer.Write((int)Flags);
            writer.Write(Guild);
            writer.Write(Mileage);
        }

        public static XiStrCharInfo Deserialize(BinaryReaderExt reader)
        {
            return new XiStrCharInfo() {
                Cid = reader.ReadInt64(),
                Name = reader.ReadUnicodeStatic(21),
                LastDate = reader.ReadInt32(),
                Avatar = reader.ReadUInt16(),
                Level = reader.ReadUInt16(),
                ExpInfo = XiStrExpInfo.Deserialize(reader),
                MitoMoney = reader.ReadInt64(),
                TeamId = reader.ReadInt64(),
                TeamMarkId = reader.ReadInt64(),
                TeamName = reader.ReadUnicodeStatic(13),
                TeamRank = reader.ReadInt32(),
                PType = reader.ReadChar(),
                PvpCnt = reader.ReadUInt32(),
                PvpWinCnt = reader.ReadUInt32(),
                PvpPoint = reader.ReadUInt32(),
                TPvpCnt = reader.ReadUInt32(),
                TPvpWinCnt = reader.ReadUInt32(),
                TPvpPoint = reader.ReadUInt32(),
                QuickCnt = reader.ReadUInt32(),
                TotalDistance = reader.ReadSingle(),
                m_Position = new float[] {
                    reader.ReadSingle(), // X
                    reader.ReadSingle(), // Y
                    reader.ReadSingle(), // Z
                    reader.ReadSingle()  // Rotation
                },
                m_LastChannel = reader.ReadInt32(),
                m_City = reader.ReadInt32(),
                m_PosState = (POSITION_STATE)reader.ReadInt32(),
                CurCarId = reader.ReadInt32(),
                QuickSlot1 = reader.ReadUInt32(),
                QuickSlot2 = reader.ReadUInt32(),
                TeamJoinDate = reader.ReadInt32(),
                TeamCloseDate = reader.ReadInt32(),
                TeamLeaveDate = reader.ReadInt32(),
                HancoinInven = reader.ReadInt32(),
                HancoinGarage = reader.ReadInt32(),
                Flags = (FlagType)reader.ReadInt32(),
                Guild = reader.ReadInt32(),
                Mileage = reader.ReadInt64()
            };
        }
    }
}
