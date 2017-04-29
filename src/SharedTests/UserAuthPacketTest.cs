using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            byte[] packet = System.IO.File.ReadAllBytes(Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)) + "/../../packetcaptures/UserAuth.bin");
            Packet p = new Packet(null, Packets.CmdUserAuth, packet);
            UserAuthPacket authPacket = new UserAuthPacket(p);


            Assert.AreEqual(authPacket.ProtocolVersion, ServerMain.ProtocolVersion);
            Assert.AreEqual(authPacket.Username, "admin");
            Assert.AreEqual(authPacket.Password, "admin");
        }
    }
}
