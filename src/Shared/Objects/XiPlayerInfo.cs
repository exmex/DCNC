namespace Shared.Objects
{
    public class XiPlayerInfo
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
        public XiVisualItem VisualItem;
        public float UseTime;
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
    }
}