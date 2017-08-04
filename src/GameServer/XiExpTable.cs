using System;
using System.Collections.Generic;
using System.IO;
using Shared.Util;

namespace GameServer
{
    public class XiExpTable
    {
        public static Dictionary<long, long> LoadFromTdf(TdfReader tdfReader)
        {
            var levelTable = new Dictionary<long, long>();
            using (var reader = new BinaryReaderExt(new MemoryStream(tdfReader.ResTable)))
            {
                for (var row = 0; row < tdfReader.Header.Row; row++)
                    levelTable.Add(Convert.ToInt64(reader.ReadUnicode()), Convert.ToInt64(reader.ReadUnicode()));
            }

            return levelTable;
        }
    }
}