using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using Shared.Util;

namespace GameServer
{
    public class XiExpTable
    {
        public static Dictionary<int, KeyValuePair<ushort, long>> LoadFromTdf(TdfReader tdfReader)
        {
            var levelTable = new Dictionary<int, KeyValuePair<ushort, long>>();
            using (TextReader reader = File.OpenText("system/data/LevelServer.csv"))
            {
                string line;
                int i = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    var data = line.Split(',');
                    if (data.Length < 2) return levelTable;
                    levelTable.Add(i, new KeyValuePair<ushort, long>(Convert.ToUInt16(data[0]), Convert.ToInt64(data[1])));
                    i++;
                }
            }
            /*var levelTable = new Dictionary<int, KeyValuePair<ushort, long>>();
            using (var reader = new BinaryReaderExt(new MemoryStream(tdfReader.ResTable)))
            {
                for (var row = 0; row < tdfReader.Header.Row; row++)
                    levelTable.Add(row, new KeyValuePair<ushort, long>(Convert.ToUInt16(reader.ReadUnicode()), Convert.ToInt64(reader.ReadUnicode())));
            }*/

            return levelTable;
        }
    }
}