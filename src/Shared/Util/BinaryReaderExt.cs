using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Util
{
    public class BinaryReaderExt:BinaryReader
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
    }
}
