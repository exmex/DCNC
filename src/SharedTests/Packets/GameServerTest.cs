using System.IO;
using NUnit.Framework;
using Shared.Network.GameServer;
using Shared.Util;

namespace SharedTests.Packets
{
    [TestFixture]
    public class GameServerTest
    {
        [Test]
        public void WeatherAnswerTest()
        {
            var packet = new WeatherAnswer
            {
                CurrentWeather = WeatherAnswer.Weather.Foggy
            };
            var bytes = packet.GetBytes();

            using (var ms = new MemoryStream(bytes))
            {
                using (var bs = new BinaryReaderExt(ms))
                {
                    var weather = bs.ReadInt32();
                    Assert.AreEqual((int)packet.CurrentWeather, weather);
                }
            }
        }
    }
}