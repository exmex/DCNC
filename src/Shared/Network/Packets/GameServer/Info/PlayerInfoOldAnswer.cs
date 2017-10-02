using System.IO;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class PlayerInfoOldAnswer : OutPacket
    {
        public XiPlayerInfo PlayerInfo = new XiPlayerInfo();
        public XiPlayerInfo[] PlayerInfos = new XiPlayerInfo[0];
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.PlayerInfoOldAck);
        }

        public override int ExpectedSize() => (216 * PlayerInfos.Length) + 222;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(PlayerInfos.Length);
                    bs.Write(PlayerInfo);
                    foreach (var playerInfo in PlayerInfos)
                    {
                        bs.Write(playerInfo);
                    }
                }
                return ms.ToArray();
            }
        }
    }
}