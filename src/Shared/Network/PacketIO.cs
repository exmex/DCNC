using System;
using System.IO;
using System.Text;

namespace Shared.Network
{
    public interface ISerializable
    {
        void Serialize(PacketWriter writer);
    }

    /// <summary>
    ///     Class PacketWriter.
    ///     Size reference:
    ///     sizeof(sbyte)	1
    ///     sizeof(byte)	1
    ///     sizeof(short)	2
    ///     sizeof(ushort)	2
    ///     sizeof(int)	4
    ///     sizeof(uint)	4
    ///     sizeof(long)	8
    ///     sizeof(ulong)	8
    ///     sizeof(char)	2 (Unicode)
    ///     sizeof(float)	4
    ///     sizeof(double)	8
    ///     sizeof(decimal)	16
    ///     sizeof(bool)	1
    /// </summary>
    /// <seealso cref="System.IO.BinaryWriter" />
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

            var buf = Encoding.Unicode.GetBytes(str + "\0");

            if (lengthPrefix)
                Write((ushort) str.Length);

            Write(buf);
        }

        public void WriteUnicodeStatic(string str, int maxLength)
        {
            if (str == null)
                str = "";

            if (str.Length > maxLength)
                str = str.Substring(0, maxLength);

            var stringBuf = Encoding.Unicode.GetBytes(str);

            var buf = new byte[maxLength * 2];
            Array.Copy(stringBuf, 0, buf, 0, stringBuf.Length);

            Write(buf);
        }

        public void WriteAsciiStatic(string str, int maxLength)
        {
            if (str == null)
                str = "";

            if (str.Length > maxLength)
                str = str.Substring(0, maxLength);

            var stringBuf = Encoding.ASCII.GetBytes(str);

            var buf = new byte[maxLength];
            Array.Copy(stringBuf, 0, buf, 0, stringBuf.Length);

            Write(buf);
        }

        public void Write(ISerializable structure)
        {
            structure.Serialize(this);
        }
    }

    public class PacketReader : BinaryReader
    {
        public PacketReader(MemoryStream stream)
            : base(stream, Encoding.Unicode)
        {
        }

        public string ReadUnicode()
        {
            var sb = new StringBuilder();
            short val;
            do
            {
                val = ReadInt16();
                if (val > 0)
                    sb.Append((char) val);
            } while (val > 0);
            return sb.ToString();
        }

        public string ReadUnicodeStatic(int maxLength)
        {
            var buf = ReadBytes(maxLength * 2);
            var str = Encoding.Unicode.GetString(buf);

            if (str.Contains("\0"))
                str = str.Substring(0, str.IndexOf('\0'));

            return str;
        }

        public string ReadUnicodePrefixed()
        {
            var length = ReadUInt16();
            return ReadUnicodeStatic(length);
        }

        public string ReadAscii()
        {
            var sb = new StringBuilder();
            byte val;
            do
            {
                val = ReadByte();
                sb.Append((char) val);
            } while (val > 0);
            return sb.ToString();
        }

        public string ReadAsciiStatic(int maxLength)
        {
            var buf = ReadBytes(maxLength);
            var str = Encoding.ASCII.GetString(buf);

            if (str.Contains("\0"))
                str = str.Substring(0, str.IndexOf('\0'));

            return str;
        }
    }
}