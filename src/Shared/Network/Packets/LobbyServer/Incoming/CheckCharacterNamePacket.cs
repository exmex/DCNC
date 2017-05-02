using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Network.LobbyServer
{
    public class CheckCharacterNamePacket
    {
        public readonly string CharacterName;

        public CheckCharacterNamePacket(Packet packet)
        {
            CharacterName = packet.Reader.ReadUnicode();
        }
    }
}