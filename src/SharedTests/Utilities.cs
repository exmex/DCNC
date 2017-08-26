using System;
using System.IO;
using NUnit.Framework;
using Shared.Network;

namespace SharedTests
{
    public static class Utilities
    {
        public static Random Rand = new Random();
        private static byte[] ReadAllBytesNoLock(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException();
            
            byte[] oFileBytes;
            using (var fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                var numBytesToRead = Convert.ToInt32(fs.Length);
                oFileBytes = new byte[(numBytesToRead)];
                fs.Read(oFileBytes, 0, numBytesToRead);
            }
            return oFileBytes;
        }

        private static string GetTestFile(string filePath)
        {
            return Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)) + filePath;
        }

        public static Packet ConstructTestPacket(string testFile, ushort packetId)
        {
            var packet = ReadAllBytesNoLock(GetTestFile("/../../packetcaptures/"+testFile));
            return new Packet(null, packetId, packet);
        }
    }
}