using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using Shared.Util;

namespace Shared.Network
{
    public class DefaultServer
    {
        private static Dictionary<ushort, string> _debugNameDatabase;

        private readonly Dictionary<ushort, Action<Packet>> _parsers;
        private readonly List<Client> _clients;

        private readonly int _port;
        private readonly TcpListener _listener;
        private readonly bool _exchangeRequired;

        public DefaultServer(int port, bool exchangeRequired = true)
        {
#if DEBUG
            if (_debugNameDatabase == null && File.Exists("system/parsers.txt"))
            {
                _debugNameDatabase = new Dictionary<ushort, string>();
                //string src = new WebClient().DownloadString("http://u.rtag.me/p/parsers.txt");
                string src = File.ReadAllText("system/parsers.txt");

                foreach (var line in src.Split('\n'))
                {
                    if (line.Length <= 3) continue;
                    string[] lineSplit = line.Split(':');

                    ushort id = ushort.Parse(lineSplit[0]);

                    _debugNameDatabase[id] = lineSplit[1].Trim().Split('_')[1];
                }
            }
#endif

            _parsers = new Dictionary<ushort, Action<Packet>>();
            _clients = new List<Client>();
            _port = port;
            _listener = new TcpListener(IPAddress.Any, port);
            _exchangeRequired = exchangeRequired;

            int i = 0;
            foreach (var type in Assembly.GetEntryAssembly().GetTypes())
            {
                foreach (var method in type.GetMethods())
                {
                    foreach (var boxedAttrib in method.GetCustomAttributes(typeof(PacketAttribute), false))
                    {
                        var attrib = boxedAttrib as PacketAttribute;

                        if (attrib == null) continue;
                        var id = attrib.Id;
                        var parser = (Action<Packet>)Delegate.CreateDelegate(typeof(Action<Packet>), method);

                        SetParser(id, parser);
                        i++;
                    }
                }
            }
            Log.Info("Added {0} packets", i);
        }

        public void Start()
        {
            _listener.Start();
            _listener.BeginAcceptTcpClient(OnAccept, _listener);

            Log.Info("Server started on port {0}", _port);
        }

        private void OnAccept(IAsyncResult result)
        {
            var tcpClient = _listener.EndAcceptTcpClient(result);
            var riceClient = new Client(tcpClient, this, _exchangeRequired);

            Log.Info("Accepted client from {0} on {1}", tcpClient.Client.RemoteEndPoint, _port);

            _clients.Add(riceClient);
            _listener.BeginAcceptTcpClient(OnAccept, _listener);
        }

        private void SetParser(ushort id, Action<Packet> parser)
        {
            Packets.GetName(id);

            Log.Debug("Added parser for packet {0} ({1} : 0x{1:X}).", Packets.GetName(id), id);
            _parsers[id] = parser;
        }

        public void Parse(Packet packet)
        {
            int bytesPerLine = 16;
            string hexDump = "";
            int j = 0;
            foreach (var g in packet.Buffer.Select((c, i) => new { Char = c, Chunk = i / bytesPerLine }).GroupBy(c => c.Chunk))
            {
                var s1 = g.Select(c => $"{c.Char:X2} ").Aggregate((s, i) => s + i);
                string s2 = null;
                bool first = true;
                foreach (var c in g)
                {
                    var s = $"{(c.Char < 32 || c.Char > 122 ? '·' : (char) c.Char)} ";
                    if (first)
                    {
                        first = false;
                        s2 = s;
                        continue;
                    }
                    s2 = s2 + s;
                }
                var s3 = $"{j++*bytesPerLine:d6}: {s1} {s2}";
                hexDump = hexDump + s3 + Environment.NewLine;
            }

#if DEBUG
            if (packet.Id != 3917 && packet.Id != 7)
            {
                if (!_debugNameDatabase.ContainsKey(packet.Id))
                    Log.Debug("{0}: {1}", packet.Id, hexDump);
                else
                    Log.Debug("{0}: {1}", _debugNameDatabase[packet.Id], hexDump);
            }
#else
            if (!_parsers.ContainsKey(packet.Id))
            {
                if (!_debugNameDatabase.ContainsKey(packet.Id))
                    Log.Debug("{0}: {1}", packet.Id, hexDump);
                else
                    Log.Debug("{0}: {1}", _debugNameDatabase[packet.Id], hexDump);
            }
#endif

            System.IO.Directory.CreateDirectory("packetcaptures\\");

            if (_debugNameDatabase.ContainsKey(packet.Id))
                if (!File.Exists("packetcaptures\\" + _debugNameDatabase[packet.Id] + ".txt"))
                    System.IO.File.WriteAllText("packetcaptures\\" + _debugNameDatabase[packet.Id] + ".txt", hexDump);
            else
                if (!File.Exists("packetcaptures\\" + packet.Id + ".txt"))
                    System.IO.File.WriteAllText("packetcaptures\\" + packet.Id + ".txt", hexDump);

            if (packet.Id != 3917 && packet.Id != 7)
            {
                if (_parsers.ContainsKey(packet.Id))
                    _parsers[packet.Id](packet);
                else
                {
#if DEBUG
                    if (_debugNameDatabase.ContainsKey(packet.Id))
                        Log.Info("Received {2} (id {0}, 0x{0:X}) on {1}.", packet.Id, _port,
                            _debugNameDatabase[packet.Id]);
                    else
#endif
                        Log.Error("Received unknown packet (id {0}, 0x{0:X}) on {1}.", packet.Id, _port);
                }
            }
        }

        public Client[] GetClients()
        {
            return _clients.ToArray();
        }

        public void Broadcast(Packet packet, Client exclude = null)
        {
            foreach (var client in GetClients())
            {
                if (exclude == null || client != exclude)
                    client.Send(packet);
            }
        }
    }
}
