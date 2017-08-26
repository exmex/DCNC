using System;
using System.IO;
using NUnit.Framework;
using Shared;
using Shared.Network.LobbyServer;
using Shared.Objects;
using Shared.Util;

namespace SharedTests.Packets
{
    [TestFixture]
    public class LobbyServerTest
    {
        #region "CheckInLobbyExchange"
        [Test]
        public void CheckInLobbyPacketTest()
        {
            var packet = new CheckInLobbyPacket(Utilities.ConstructTestPacket("CheckInLobby.bin",
                Shared.Network.Packets.CmdCheckInLobby));
            
            Assert.AreEqual(ServerMain.ProtocolVersion, packet.ProtocolVersion);
            StringAssert.AreEqualIgnoringCase(string.Empty, packet.StringTicket);
            Assert.AreEqual(2225370335, packet.Ticket);
            Assert.AreEqual(6357106, packet.Time);
            StringAssert.AreEqualIgnoringCase("admin", packet.Username);
        }

        [Test]
        public void CheckInLobbyAnswerTest()
        {
            var packet = new CheckInLobbyAnswerPacket();
            var bytes = packet.GetBytes();

            using (var ms = new MemoryStream(bytes))
            {
                using (var bs = new BinaryReaderExt(ms))
                {
                    var result = bs.ReadInt32();
                    Assert.AreEqual(packet.Result, result);
                    
                    var permission = bs.ReadInt32();
                    Assert.AreEqual(packet.Permission, permission);
                }
            }
        }
        #endregion
        
        #region "CheckCharNameExchange"
        [Test]
        public void CheckCharNamePacketTest()
        {
            var packet =
                new CheckCharacterNamePacket(Utilities.ConstructTestPacket("CheckCharName.bin", Shared.Network.Packets.CmdCheckCharName));
            
            StringAssert.AreEqualIgnoringCase("fefewwefewf", packet.CharacterName);
        }
        

        [Test]
        public void CheckCharNameAnswerTest()
        {
            var packet = new CheckCharacterNameAnswerPacket()
            {
                CharacterName = "Test",
                Availability = true
            };
            var bytes = packet.GetBytes();

            using (var ms = new MemoryStream(bytes))
            {
                using (var bs = new BinaryReaderExt(ms))
                {
                    var characterName = bs.ReadUnicodeStatic(21);
                    Assert.AreEqual(packet.CharacterName, characterName);

                    var availability = bs.ReadBoolean();
                    Assert.AreEqual(packet.Availability, availability);
                }
            }
        }
        #endregion

        #region "CreateCharExchange"
        [Test]
        public void CreateCharPacketTest()
        {
            var packet = new CreateCharPacket(Utilities.ConstructTestPacket("CreateChar.bin", Shared.Network.Packets.CmdCreateChar));
            
            StringAssert.AreEqualIgnoringCase("fefewwefewf", packet.CharacterName);
            Assert.AreEqual(0, packet.Avatar);
            Assert.AreEqual(95, packet.CarType);
            Assert.AreEqual(16777218, packet.Color);
        }
        
        [Test]
        public void CreateCharAnswerTest()
        {
            var packet = new CreateCharAnswerPacket()
            {
                CharacterName = "Test"
            };
            var bytes = packet.GetBytes();

            using (var ms = new MemoryStream(bytes))
            {
                using (var bs = new BinaryReaderExt(ms))
                {
                    var characterName = bs.ReadUnicodeStatic(21);
                    Assert.AreEqual(packet.CharacterName, characterName);
                }
            }
        }
        #endregion
        
        #region "DeleteCharExchange"
        [Test]
        public void DeleteCharPacketTest()
        {
            var packet = new DeleteCharacterPacket(Utilities.ConstructTestPacket("DeleteChar.bin", Shared.Network.Packets.CmdDeleteChar));
            
            StringAssert.AreEqualIgnoringCase("Administrator", packet.CharacterName);
        }
        
        [Test]
        public void DeleteCharAnswerTest()
        {
            var packet = new DeleteCharacterAnswerPacket()
            {
                CharacterName = "Test"
            };
            var bytes = packet.GetBytes();

            using (var ms = new MemoryStream(bytes))
            {
                using (var bs = new BinaryReaderExt(ms))
                {
                    var characterName = bs.ReadUnicodeStatic(21);
                    Assert.AreEqual(packet.CharacterName, characterName);
                }
            }
        }
        #endregion

        #region "UserInfoExchange"
        [Test]
        public void UserInfoPacketTest()
        {
            var packet = new UserInfoPacket(Utilities.ConstructTestPacket("UserInfo.bin", Shared.Network.Packets.CmdUserInfo));
            
            StringAssert.AreEqualIgnoringCase("admin", packet.Username);
            Assert.AreEqual(2225370335, packet.Ticket);
        }
        
        [Test]
        public void UserInfoAnswerTest()
        {
            var packet = new UserInfoAnswerPacket()
            {
                Permissions = Utilities.Rand.Next(),
                CharacterCount = 1,
                Characters = new []
                {
                    new Character()
                    {
                        Name = "TestChar",
                        Cid = Utilities.Rand.NextUInt32(),
                        Avatar = 2,
                        Level = 10,
                        CurrentCarId = Utilities.Rand.Next(),
                        CreationDate = Utilities.Rand.Next(),
                        Tid = Utilities.Rand.NextUInt32(),
                        TeamMarkId = Utilities.Rand.NextUInt32(),
                        ActiveCar = new Vehicle()
                        {
                            CarType = Utilities.Rand.NextUInt32(),
                            BaseColor = Utilities.Rand.NextUInt32()
                        },
                        TeamName = "TestTeam"
                    }
                },
                Username = "Test"
            };
            var bytes = packet.GetBytes();

            using (var ms = new MemoryStream(bytes))
            {
                using (var bs = new BinaryReaderExt(ms))
                {
                    var permissions = bs.ReadInt32();
                    Assert.AreEqual(packet.Permissions, permissions);
                    
                    var characterCount = bs.ReadInt32();
                    Assert.AreEqual(packet.CharacterCount, characterCount);
                    
                    var username = bs.ReadUnicodeStatic(18);
                    Assert.AreEqual(packet.Username, username);
                    
                    var long1 = bs.ReadInt64(); // Always 0
                    Assert.AreEqual(0, long1);
                    
                    var long2 = bs.ReadInt64(); // Always 0
                    Assert.AreEqual(0, long2);
                    
                    var long3 = bs.ReadInt64(); // Always 0
                    Assert.AreEqual(0, long3);
                    
                    var int1 = bs.ReadInt32(); // Always 0
                    Assert.AreEqual(0, int1);

                    for (int i = 0; i < characterCount; i++)
                    {
                        var characterName = bs.ReadUnicodeStatic(21);
                        StringAssert.AreEqualIgnoringCase(packet.Characters[i].Name, characterName);

                        var charId = bs.ReadUInt64();
                        Assert.AreEqual(packet.Characters[i].Cid, charId);

                        var avatar = bs.ReadInt32();
                        Assert.AreEqual(packet.Characters[i].Avatar, avatar);

                        var level = bs.ReadInt32();
                        Assert.AreEqual(packet.Characters[i].Level, level);

                        var currentCarId = bs.ReadInt32();
                        Assert.AreEqual(packet.Characters[i].CurrentCarId, currentCarId);

                        var carType = bs.ReadUInt32();
                        Assert.AreEqual(packet.Characters[i].ActiveCar.CarType, carType);
                        
                        var baseColor = bs.ReadUInt32();
                        Assert.AreEqual(packet.Characters[i].ActiveCar.BaseColor, baseColor);

                        var creationDate = bs.ReadInt32();
                        Assert.AreEqual(packet.Characters[i].CreationDate, creationDate);

                        var tid = bs.ReadInt64(); // TeamId!!!
                        Assert.AreEqual(packet.Characters[i].Tid, tid);

                        var teamMarkId = bs.ReadInt64();
                        Assert.AreEqual(packet.Characters[i].TeamMarkId, teamMarkId);

                        var teamName = bs.ReadUnicodeStatic(13);
                        StringAssert.AreEqualIgnoringCase("TestTeam", teamName);
                    }
                }
            }
        }
        #endregion
    }
}