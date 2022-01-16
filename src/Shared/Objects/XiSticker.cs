using Shared.Util;


namespace Shared.Objects
{
    public class XiSticker : BinaryWriterExt.ISerializable
    {
        public long stickerId;
        public uint color;
        public ushort posX;
        public ushort posY;
        public ushort scaleX;
        public ushort scaleY;
        public ushort rotate;
        public char part;
        public char flip;

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(stickerId);
            writer.Write(color);
            writer.Write(posX);
            writer.Write(posY);
            writer.Write(scaleX);
            writer.Write(scaleY);
            writer.Write(rotate);
            writer.Write(part);
            writer.Write(flip);
        }

    }
}
