using System;
using System.IO;
using System.Text;

namespace ServerCommon.Classes
{
    public class PacketWriter : BinaryWriter
    {
        public PacketWriter(MemoryStream stream)
            : base(stream, Encoding.Unicode)
        {
        }

        public byte[] GetBuffer()
        {
            return (BaseStream as MemoryStream)?.ToArray();
        }

        public void WriteUnicode(string str, bool lengthPrefix = true)
        {
            if (str == null)
                str = "";

            byte[] buf = Encoding.Unicode.GetBytes(str + "\0");

            if (lengthPrefix)
                Write((ushort)str.Length);

            Write(buf);
        }

        public void WriteUnicodeStatic(string str, int maxLength)
        {
            if (str == null)
                str = "";

            if (str.Length > maxLength)
                str = str.Substring(0, maxLength);

            byte[] stringBuf = Encoding.Unicode.GetBytes(str);

            byte[] buf = new byte[maxLength * 2];
            Array.Copy(stringBuf, 0, buf, 0, stringBuf.Length);

            Write(buf);
        }

        public void WriteASCIIStatic(string str, int maxLength)
        {
            if (str == null)
                str = "";

            if (str.Length > maxLength)
                str = str.Substring(0, maxLength);

            byte[] stringBuf = Encoding.ASCII.GetBytes(str);

            byte[] buf = new byte[maxLength];
            Array.Copy(stringBuf, 0, buf, 0, stringBuf.Length);

            Write(buf);
        }

        public void Write(ISerializable structure)
        {
            structure.Serialize(this);
        }
    }
}
