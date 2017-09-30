using System.Security;
using Shared.Util;

namespace Shared.Objects
{
    public class XiPlayerInfo : BinaryWriterExt.ISerializable
    {
        public string CharacterName;

        public ushort Serial;
        public ushort Age;
        public long CharacterId;
        public ushort Level;
        public uint Exp;
        public long TeamId;
        public long TeamMarkId;
        public string TeamName;
        public ushort TeamNLevel;
        public XiVisualItem VisualItem = new XiVisualItem();
        public float UseTime;

        public XiPlayerInfo()
        {
        }

        public XiPlayerInfo(Character character)
        {
            CharacterName = character.Name;
            //Serial
            Age = 0;
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
        public void Serialize(BinaryWriterExt writer)
        {
            writer.WriteUnicodeStatic(CharacterName, 13, true);
            writer.Write(Serial);
            writer.Write(Age);
            writer.Write(new byte[186]);
        }
    }
}