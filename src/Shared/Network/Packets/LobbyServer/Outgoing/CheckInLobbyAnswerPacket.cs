using System.IO;
using Shared.Util;

namespace Shared.Network.LobbyServer
{
    public class CheckInLobbyAnswerPacket : OutPacket
    {
        /// <summary>
        ///     The permission flags
        ///     Valid values:
        ///     0x8000 => Administrator
        ///     0x4000 => Power User
        ///     0x2000 => Remote Client User
        ///     0x1000 => Developer
        ///     0x0 => User
        /// </summary>
        public int Permission = 0;

        /// <summary>
        ///     The result of the operation
        /// </summary>
        public int Result = 1;

        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.CheckInLobbyAck);
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(Result);
                    bs.Write(Permission);
                }
                return ms.ToArray();
            }
        }
    }
}