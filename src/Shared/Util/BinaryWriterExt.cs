using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        
        public void WriteUnicodeStatic(string str, int maxLength, bool nullTerminated = false)
        {
            if (str == null)
                str = "";
	
            if (str.Length > maxLength) 
                str = str.Substring(0, maxLength);
	
            if (nullTerminated)
            {
                if (str.Length > maxLength - 1)
                    str = str.Substring(0, maxLength - 1);
                str += '\0';
            }
	
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

        public static string HexDump(IEnumerable<byte> buffer)
        {
            const int bytesPerLine = 16;
            var hexDump = "";
            var j = 0;
            foreach (var g in buffer.Select((c, i) => new {Char = c, Chunk = i / bytesPerLine}).GroupBy(c => c.Chunk))
            {
                var s1 = g.Select(c => $"{c.Char:X2} ").Aggregate((s, i) => s + i);
                string s2 = null;
                var first = true;
                foreach (var c in g)
                {
                    var s = $"{(c.Char < 32 || c.Char > 122 ? '·' : (char) c.Char)} ";
                    if (first)
                    {
                        first = false;
                        s2 = s;
                        continue;
                    }
                    s2 = s2 + s;
                }
                var s3 = $"{j++ * bytesPerLine:d6}: {s1} {s2}";
                hexDump = hexDump + s3 + Environment.NewLine;
            }
            return hexDump;
        }
    }
}