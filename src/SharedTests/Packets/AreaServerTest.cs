using System.IO;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Shared.Network.AreaServer;
using Shared.Util;

namespace SharedTests.Packets
{
    [TestFixture]
    public class AreaServerTest
    {
        #region "EnterAreaExchange"
        [Test]
        public void EnterAreaPacketTest()
        {
            var packet = new EnterAreaPacket(Utilities.ConstructTestPacket("EnterArea.bin", Shared.Network.Packets.CmdEnterArea));

            Assert.AreEqual(123, packet.SessionId);
            StringAssert.AreEqualIgnoringCase("Administrator", packet.Username);
            Assert.AreEqual(5, packet.AreaId);
            Assert.AreEqual(4294967295, packet.LocalTime);
            Assert.AreEqual(13762644, packet.GroupId);
        }

        [Test]
        public void EnterAreaPacketAnswerTest()
        {
            var packet = new EnterAreaAnswerOutPacket()
            {
                AreaId = Utilities.Rand.NextUInt32(),
                LocalTime = Utilities.Rand.NextUInt32()
            };
            var bytes = packet.GetBytes();

            using (var ms = new MemoryStream(bytes))
            {
                using (var bs = new BinaryReaderExt(ms))
                {
                    var areaId = bs.ReadUInt32();
                    Assert.AreEqual(packet.AreaId, areaId);
                    
                    var localTime = bs.ReadUInt32();
                    Assert.AreEqual(packet.LocalTime, localTime);
                }
            }
        }
        #endregion

        #region "TimeSync Exchange"
        [Test]
        public void TimeSyncPacketTest()
        {
            var packet = new TimeSyncPacket(Utilities.ConstructTestPacket("UDPTimeSync.bin", Shared.Network.Packets.CmdUdpTimeSync));
            
            Assert.AreEqual(1180111476, packet.LocalTime);
        }
        [Test]
        public void TimeSyncPacketAnswerTest()
        {
            var packet = new TimeSyncAnswerPacket()
            {
                GlobalTime = Utilities.Rand.NextUInt32(),
                SystemTick = Utilities.Rand.NextUInt32()
            };
            var bytes = packet.GetBytes();

            using (var ms = new MemoryStream(bytes))
            {
                using (var bs = new BinaryReaderExt(ms))
                {
                    var globalTime = bs.ReadUInt32();
                    Assert.AreEqual(packet.GlobalTime, globalTime);
                    
                    var systemTick = bs.ReadUInt32();
                    Assert.AreEqual(packet.SystemTick, systemTick);
                }
            }
        }
        #endregion

        #region "UdpCastExchange"
        [Test]
        public void UdpCastTcsSignalPacketTest()
        {
            var packet = new UdpCastTcsSignalPacket(Utilities.ConstructTestPacket("UdpCastTcsSignal.bin", Shared.Network.Packets.CmdUdpCastTcsSignal));

            Assert.AreEqual(4, packet.AreaId, 4);
            Assert.AreEqual(34, packet.Signal, 34);
            Assert.AreEqual(0, packet.State, 0);
            Assert.AreEqual(0, packet.Time, 0);
            Assert.AreEqual(-4345.89941f, packet.X);
            Assert.AreEqual(1026.86694f, packet.Y);
        }

        [Test]
        public void UdpCastTcsSignalAnswerTest()
        {
            var packet = new UdpCastTcsSignalAnswerPacket()
            {
                Signal = Utilities.Rand.Next(),
                State = Utilities.Rand.Next(),
                Time = Utilities.Rand.Next()
            };
            var bytes = packet.GetBytes();
            
            using (var ms = new MemoryStream(bytes))
            {
                using (var bs = new BinaryReaderExt(ms))
                {
                    var time = bs.ReadInt32();
                    Assert.AreEqual(packet.Time, time);
                    
                    var signal = bs.ReadInt32();
                    Assert.AreEqual(packet.Signal, signal);

                    var state = bs.ReadInt32();
                    Assert.AreEqual(packet.State, state);
                }
            }
        }
        #endregion
    }
}