using System;
using System.Collections.Generic;
using System.IO;
using Shared.Util;

namespace GameServer
{
    public class XiExpTable
    {
        public static Dictionary<int, KeyValuePair<ushort, long>> LoadFromTdf(TdfReader tdfReader)
        {
            var levelTable = new Dictionary<int, KeyValuePair<ushort, long>>();
            using (var reader = new BinaryReaderExt(new MemoryStream(tdfReader.ResTable)))
            {
                for (var row = 0; row < tdfReader.Header.Row; row++)
                    levelTable.Add(row, new KeyValuePair<ushort, long>(Convert.ToUInt16(reader.ReadUnicode()), Convert.ToInt64(reader.ReadUnicode())));
            }

            return levelTable;
        }
    }
}