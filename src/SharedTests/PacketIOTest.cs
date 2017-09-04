using System.IO;
using NUnit.Framework;
using Shared.Network.AreaServer;
using Shared.Network.GameServer;
using Shared.Util;

namespace SharedTests
{
    [TestFixture]
    public class PacketIoTest
    {
        [Test]
        public void TestPacketSize()
        {
            Assert.AreEqual(sizeof(uint)*2, new TimeSyncAnswerPacket().GetBytes().Length);
            
            Assert.AreEqual(sizeof(int)*4 + sizeof(byte)*6, new EnterAreaAnswer().GetBytes().Length);
            
            Assert.AreEqual(sizeof(int)*3, new BuyItemAnswer().GetBytes().Length);
            
            Assert.AreEqual(827, new GameCharInfoAnswer().GetBytes().Length);
        }
    }
}