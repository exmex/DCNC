using System.IO;
using NUnit.Framework;
using Shared;
using Shared.Network;
using Shared.Network.AuthServer;

namespace SharedTests
{
    [TestFixture]
    public class UserAuthPacketTest
    {
        [Test]
        public void TestReceive()
        {
            var packet =
                File.ReadAllBytes(
                    Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)) +
                    "/../../packetcaptures/UserAuth.bin");
            var p = new Packet(null, Packets.CmdUserAuth, packet);
            var authPacket = new UserAuthPacket(p);


            Assert.AreEqual(authPacket.ProtocolVersion, ServerMain.ProtocolVersion);
            Assert.AreEqual(authPacket.Username, "admin");
            Assert.AreEqual(authPacket.Password, "admin");
        }
    }
}