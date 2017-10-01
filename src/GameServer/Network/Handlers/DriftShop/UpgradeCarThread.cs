using Shared.Network;

namespace GameServer.Network.Handlers
{
    /*
[Info] - Received unhandled packet UpgradeCar (CmdUpgradeCarThread id 91, 0x5B) on 11021.
[Debug] - HexDump  UpgradeCar (CmdUpgradeCarThread id 91, 0x5B):
000000: 15 00 00 00 10 27 00 00 00 00 00 00 00 00 00 00  · · · · · ' · · · · · · · · · ·
000016: 00  ·
*/
    public class UpgradeCarThread
    {
        //[Packet(Packets.CmdUpgradeCarThread)]
        public static void Handle(Packet packet)
        {
            // TODO: Implement Uprgade Car.
        }
    }
}