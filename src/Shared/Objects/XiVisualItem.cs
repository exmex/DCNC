namespace Shared.Objects
{
    public class XiVisualItem
    {
        public short Neon;
        public short Plate;
        public short Decal;
        public short DecalColor;
        public short AeroBumper;
        public short AeroIntercooler;
        public short AeroSet;
        public short MufflerFlame;
        public short Wheel;
        public short Spoiler;
        public short[] Reserve; // 6
        public string PlateString; // 9
        
        /*
        struct XiVisualItem
{
  __int16 Neon;
  __int16 Plate;
  __int16 Decal;
  __int16 DecalColor;
  __int16 AeroBumper;
  __int16 AeroIntercooler;
  __int16 AeroSet;
  __int16 MufflerFlame;
  __int16 Wheel;
  __int16 Spoiler;
  __int16 Reserve[6];
  wchar_t PlateString[9];
};
        */
    }
}