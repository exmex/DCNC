using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCommon.Classes
{
    public class PacketReader : BinaryReader
    {
        public PacketReader(MemoryStream stream)
            : base(stream, Encoding.Unicode)
        {
        }

        public string ReadUnicode()
        {
            StringBuilder sb = new StringBuilder();
            short val;
            do
            {
                val = ReadInt16();
                if (val > 0)
                    sb.Append((char)val);
            }
            while (val > 0);
            return sb.ToString();
        }

        public string ReadUnicodeStatic(int maxLength)
        {
            byte[] buf = ReadBytes(maxLength * 2);
            string str = Encoding.Unicode.GetString(buf);

            if (str.Contains("\0"))
                str = str.Substring(0, str.IndexOf('\0'));

            return str;
        }

        public string ReadUnicodePrefixed()
        {
            ushort length = ReadUInt16();
            return ReadUnicodeStatic(length);
        }

        public string ReadASCII()
        {
            StringBuilder sb = new StringBuilder();
            byte val;
            do
            {
                val = ReadByte();
                sb.Append((char)val);
            }
            while (val > 0);
            return sb.ToString();
        }

        public string ReadASCIIStatic(int maxLength)
        {
            byte[] buf = ReadBytes(maxLength);
            string str = Encoding.ASCII.GetString(buf);

            if (str.Contains("\0"))
                str = str.Substring(0, str.IndexOf('\0'));

            return str;
        }
    }
}
