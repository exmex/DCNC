using System.IO;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class WeatherAnswer : OutPacket
    {
        public enum Weather
        {
            Fine = 0x0,
            Cloudy = 0x1,
            Foggy = 0x2,
            Rain = 0x3,
            Sunset = 0x4,
        }
        
        /*
            enum $38636D0EA7AD20B267BDBB95270A9F80
            {
              FINE = 0x0,
              CLOUDY = 0x1,
              FOGGY = 0x2,
              RAIN = 0x3,
              SUNSET = 0x4,
            };
            */
        
        public Weather CurrentWeather;
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.WeatherAck);
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write((int)CurrentWeather);
                }
                return ms.GetBuffer();
            }
            /*
            ack = new Packet(Packets.WeatherAck);
            ack.Writer.Write(3); // RAIN
            packet.Sender.Send(ack);
            */
        }
    }
}