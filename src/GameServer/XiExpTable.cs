using System;
using System.Collections.Generic;
using System.IO;
using Shared.Util;
using TdfReader = Shared.Util.TdfReader;

namespace GameServer
{
    public class XiExpTable
    {
        public static Dictionary<long, long> LoadFromTdf(TdfReader tdfReader)
        {
            Dictionary<long, long> levelTable = new Dictionary<long, long>();
            using (BinaryReaderExt reader = new BinaryReaderExt(new MemoryStream(tdfReader.ResTable)))
            {
                for (int row = 0; row < tdfReader.Header.Row; row++)
                {
                    levelTable.Add(Convert.ToInt64(reader.ReadUnicode()), Convert.ToInt64(reader.ReadUnicode()));
                }
            }

            return levelTable;
        }
    }
}