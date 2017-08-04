using System.IO;
using System.Text;

namespace Shared.Util
{
    public class BinaryReaderExt : BinaryReader
    {
        public BinaryReaderExt(MemoryStream stream)
            : base(stream, Encoding.Unicode)
        {
        }

        public string ReadUnicode()
        {
            var sb = new StringBuilder();
            char val;
            do
            {
                val = ReadChar();
                if (val > 0)
                    sb.Append(val);
            } while (val > 0);
            return sb.ToString();
        }
    }
}