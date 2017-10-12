using System.Security;
using Shared.Util;

namespace Shared.Objects
{
    /// <summary>
    /// 216 Bytes
    /// </summary>
    public class XiPlayerInfo : BinaryWriterExt.ISerializable
    {
        public Character Character;
        
        public ushort Serial;
        public ushort Age;
        public XiVisualItem VisualItem;
        public float UseTime;

        public XiPlayerInfo()
        {
            Character = new Character();
            Serial = 0;
            Age = 0;
            VisualItem = new XiVisualItem();
        }

        public XiPlayerInfo(ushort vehicleSerial, Character character)
        {
            Character = character;
            Serial = vehicleSerial;
            Age = 0;
            VisualItem = new XiVisualItem();
        }

        /*
        struct XiPlayerInfo
{
  wchar_t Cname[13];
  unsigned __int16 Serial;
  unsigned __int16 Age;
  __unaligned __declspec(align(1)) __int64 Cid;
  unsigned __int16 Level;
  unsigned int Exp;
  __unaligned __declspec(align(1)) __int64 TeamId;
  __unaligned __declspec(align(1)) __int64 TeamMarkId;
  wchar_t TeamName[14];
  unsigned __int16 TeamNLevel;
  XiVisualItem VisualItem;
  float UseTime;
};
        */
        
        /// <summary>
        /// 216 length but 150 bytes used!
        /// Leaked files has 156 bytes :/
        /// </summary>
        /// <param name="writer"></param>
        public void Serialize(BinaryWriterExt writer)
        {
            writer.WriteUnicodeStatic(Character.Name, 13, true); // 26
            writer.Write(Serial); // 2
            writer.Write(Age); // 2
            
            writer.Write(Character.Id); // 8
            writer.Write(Character.Level); // 2
            writer.Write(Character.ExperienceInfo.BaseExp); // 4
            
            //writer.Write(Character.TeamId); // 8
            if (Character.Team == null)
                new Team().SerializeShort(writer);
            else
            {
                Character.Team.SerializeShort(writer);
                /*
                writer.Write(Character.Team.MarkId); // 8
                writer.WriteUnicodeStatic(Character.Team.Name, 14, true);
                writer.Write(TeamNLevel); // 2
                */
            }
            writer.Write(VisualItem); // 50?
            writer.Write(UseTime); // 4
            
            writer.Write(new byte[60]);
            
            //writer.Write(new byte[186]);
        }
    }
}