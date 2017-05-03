using System.IO;
using System.Text;

namespace TdfReader
{
    public class BinaryReaderExt : BinaryReader
    {
        public BinaryReaderExt(MemoryStream stream)
            : base(stream, Encoding.Unicode)
        {
        }

        public string ReadUnicode()
        {
            StringBuilder sb = new StringBuilder();
            char val;
            do
            {
                val = ReadChar();
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

        public string ReadAscii()
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

        public string ReadAsciiStatic(int maxLength)
        {
            byte[] buf = ReadBytes(maxLength);
            string str = Encoding.ASCII.GetString(buf);

            if (str.Contains("\0"))
                str = str.Substring(0, str.IndexOf('\0'));

            return str;
        }
    }
}