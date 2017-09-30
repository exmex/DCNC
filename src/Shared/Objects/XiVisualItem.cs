using Shared.Util;

namespace Shared.Objects
{
    public class XiVisualItem : BinaryWriterExt.ISerializable
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
        
        /// <summary>
        /// Short array of 6
        /// </summary>
        public short[] Reserve = new short[6];
        
        /// <summary>
        /// Unicode 9 Chars
        /// </summary>
        public string PlateString; // 9
        
        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(Neon);
            writer.Write(Plate);
            writer.Write(Decal);
            writer.Write(DecalColor);
            writer.Write(AeroBumper);
            writer.Write(AeroIntercooler);
            writer.Write(AeroSet);
            writer.Write(MufflerFlame);
            writer.Write(Wheel);
            writer.Write(Spoiler);
            foreach (short r in Reserve)
                writer.Write(r);
            writer.WriteUnicodeStatic(PlateString, 9);
        }
        
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