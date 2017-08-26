using System;
using System.IO;
using System.Text;

namespace Shared.Util
{
    /// <summary>
    ///     Extension to the BinaryWriterClass.
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
    public class BinaryWriterExt : BinaryWriter
    {
        public interface ISerializable
        {
            void Serialize(BinaryWriterExt writer);
        }
        
        public BinaryWriterExt(Stream stream)
            : base(stream, Encoding.Unicode)
        {
        }

        public BinaryWriterExt(Stream output, Encoding encoding) : base(output, encoding)
        {
        }

        public byte[] GetBuffer()
        {
            return (BaseStream as MemoryStream)?.ToArray();
        }

        public void Write(ISerializable structure)
        {
            structure.Serialize(this);
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
    }
}