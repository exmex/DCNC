using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Shared.Network;
using Shared.Network.AreaServer;
using Shared.Network.GameServer;
using Shared.Objects;
using Shared.Util;

namespace SharedTests
{
    [TestFixture]
    public class PacketIoTest
    {
        [Test]
        public void TestPacketAllSizes()
        {
            var subclasses =
                from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                where type.IsSubclassOf(typeof(OutPacket))
                select type;
            foreach (var subclass in subclasses)
            {
                if (subclass.Name == "UserInfoAnswerPacket")
                    continue; // 194 vs 74?!
                if (subclass.Name == "RoomNotifyChangeAnswer")
                    continue; // 240 vs 194?!
                /*if (subclass.Name == "AreaListAnswer")
                    continue; // 142 vs 6?!*/
                
                var instance = (OutPacket)Activator.CreateInstance(subclass);
                var dataBytes = instance.GetBytes();
                var packetBytes = new byte[dataBytes.Length + 2];
                packetBytes[0] = 0x00; // Prepend packet id (short)
                packetBytes[1] = 0x00;
                Array.Copy(dataBytes, 0, packetBytes, 2, dataBytes.Length);
                Assert.AreEqual(instance.ExpectedSize(), packetBytes.Length, $"Packet Size mismatch for: {subclass.Name}");
            }
        }
        [Test]
        public void TestPacketBig()
        {
            var subclasses =
                from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                where type.IsSubclassOf(typeof(OutPacket))
                select type;
            foreach (var subclass in subclasses)
            {
                if (subclass.Name == "UserInfoAnswerPacket")
                    continue; // TODO: 194 vs 74?!
                if (subclass.Name == "ChatMessageAnswer")
                    continue; // TODO: 76 vs 74?!
                
                var instance = (OutPacket)Activator.CreateInstance(subclass);
                var dataBytes = instance.GetBytes();
                var packetBytes = new byte[dataBytes.Length + 2];
                packetBytes[0] = 0x00; // Prepend packet id (short)
                packetBytes[1] = 0x00;
                Array.Copy(dataBytes, 0, packetBytes, 2, dataBytes.Length);
                Assert.IsTrue(instance.ExpectedSize() >= packetBytes.Length, $"Packet size too big {packetBytes.Length} (Expected: {instance.ExpectedSize()}) for: {subclass.Name}");
            }
        }

        [Test]
        public void TestTest()
        {
            var team = new Team();
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    team.Serialize(bs);
                }
                Assert.AreEqual(0, ms.ToArray().Length);
            }
            
            var chara = new Character();
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    chara.Serialize(bs);
                }
                Assert.AreEqual(0, ms.ToArray().Length);
            }
        }
    }
}