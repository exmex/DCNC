using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ServerCommon.Classes;

namespace ServerCommon
{
    public class MittronMadnessServer
    {
        private List<GameClient> _clients;
        private TcpListener _listener;
        private Dictionary<ushort, Action<Packet>> _parsers;
        private bool _exchangeRequired;

        public MittronMadnessServer(string prefixName, int port, bool exchangeRequired = true)
        {
            Logger.SetLogFile(".\\logs", prefix: prefixName + "_", writeText: true);

            _exchangeRequired = exchangeRequired;
            _parsers = new Dictionary<ushort, Action<Packet>>();
            _clients = new List<GameClient>();
            _listener = new TcpListener(IPAddress.Any, port);
        }

        public void Listen(int port)
        {
            _listener.Start();
            _listener.BeginAcceptTcpClient(OnAccept, _listener);

            Logger.Info("Started TCPListener on port {0}", port);
        }

        private void OnAccept(IAsyncResult result)
        {
            var tcpClient = _listener.EndAcceptTcpClient(result);
            var riceClient = new GameClient(tcpClient, this, _exchangeRequired);

            Logger.Info("Accepted client from {0}", tcpClient.Client.RemoteEndPoint);

            _clients.Add(riceClient);
            _listener.BeginAcceptTcpClient(OnAccept, _listener);
        }

        public void SetParser(ushort id, Action<Packet> parser)
        {
            _parsers[id] = parser;
        }

        public void Parse(Packet packet)
        {
            if (_parsers.ContainsKey(packet.ID))
                _parsers[packet.ID](packet);
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;

                Logger.Error("Received unknown packet (id {0}, {0:X})", packet.ID);

                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

    }
}
