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
#if DEBUG
        private static Dictionary<ushort, string> _debugNameDatabase;
#endif
        
        private readonly List<Client> _clients;
        private readonly bool _exchangeRequired;
        private readonly TcpListener _listener;

        private readonly Dictionary<ushort, Action<Packet>> _parsers;

        private readonly int _port;

        public DefaultServer(int port, bool exchangeRequired = true)
        {
#if DEBUG
            if (_debugNameDatabase == null)
            {
                _debugNameDatabase = new Dictionary<ushort, string>();
                if (File.Exists("system/parsers.txt"))
                {
                    //string src = new WebClient().DownloadString("http://u.rtag.me/p/parsers.txt");
                    var src = File.ReadAllText("system/parsers.txt");

                    foreach (var line in src.Split('\n'))
                    {
                        if (line.Length <= 3) continue;
                        var lineSplit = line.Split(':');

                        var id = ushort.Parse(lineSplit[0]);

                        _debugNameDatabase[id] = lineSplit[1].Trim().Split('_')[1];
                    }
                }
            }
#endif

            _parsers = new Dictionary<ushort, Action<Packet>>();
            _clients = new List<Client>();
            _port = port;
            _listener = new TcpListener(IPAddress.Any, port);
            _exchangeRequired = exchangeRequired;

            var i = 0;
            foreach (var type in Assembly.GetEntryAssembly().GetTypes())
            foreach (var method in type.GetMethods())
            foreach (var boxedAttrib in method.GetCustomAttributes(typeof(PacketAttribute), false))
            {
                var attrib = boxedAttrib as PacketAttribute;

                if (attrib == null) continue;
                var id = attrib.Id;
                var parser = (Action<Packet>) Delegate.CreateDelegate(typeof(Action<Packet>), method);

                SetParser(id, parser);
                i++;
            }
            
#if DEBUG
            Log.Info("Added {0} packets", i);
#endif
        }

        public void Start()
        {
            Log.Info("Starting server on port {0}", _port);

            _listener.Start();
            _listener.BeginAcceptTcpClient(OnAccept, _listener);

            Log.Info("Server started on port {0}", _port);
        }

        private void OnAccept(IAsyncResult result)
        {
            var tcpClient = _listener.EndAcceptTcpClient(result);
            var riceClient = new Client(tcpClient, this, _exchangeRequired);

#if DEBUG
            Log.Info("Accepted client from {0} on {1}", tcpClient.Client.RemoteEndPoint, _port);
#endif

            _clients.Add(riceClient);
            _listener.BeginAcceptTcpClient(OnAccept, _listener);
        }

        private void SetParser(ushort id, Action<Packet> parser)
        {
            Packets.GetName(id);

#if DEBUG
            Log.Debug("Added parser for packet {0} ({1} : 0x{1:X}).", Packets.GetName(id), id);
#endif
            _parsers[id] = parser;
        }

        public void Parse(Packet packet)
        {
            const int bytesPerLine = 16;
            var hexDump = "";
            var j = 0;
            foreach (var g in packet.Buffer.Select((c, i) => new {Char = c, Chunk = i / bytesPerLine})
                .GroupBy(c => c.Chunk))
            {
                var s1 = g.Select(c => $"{c.Char:X2} ").Aggregate((s, i) => s + i);
                string s2 = null;
                var first = true;
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
                var s3 = $"{j++ * bytesPerLine:d6}: {s1} {s2}";
                hexDump = hexDump + s3 + Environment.NewLine;
            }
#if DEBUG
            // Hide frequent sync packets from console log.
            /*if (packet.Id != Packets.CmdUnknownSync && packet.Id != Packets.CmdNullPing &&
                packet.Id != Packets.UdpCastTcsSignalAck && packet.Id != Packets.CmdUdpCastTcsSignal &&
                packet.Id != Packets.CmdUdpCastTcs)
            {
                if (!_debugNameDatabase.ContainsKey(packet.Id))
                    Log.Debug("{0}: {1}", packet.Id, hexDump);
                else
                    Log.Debug("{0}: {1}", _debugNameDatabase[packet.Id], hexDump);
            }*/

            // Make sure the packetcaptures directory exists.
            Directory.CreateDirectory("packetcaptures\\");
            
            // Dump the received data in hex
            if (_debugNameDatabase.ContainsKey(packet.Id))
            {
                if (!File.Exists("packetcaptures\\" + _debugNameDatabase[packet.Id] + ".txt"))
                    File.WriteAllText("packetcaptures\\" + _debugNameDatabase[packet.Id] + ".txt", hexDump);
            }
            else if (!File.Exists("packetcaptures\\" + packet.Id + ".txt"))
                File.WriteAllText("packetcaptures\\" + packet.Id + ".txt", hexDump);

            // Dump the received data into a binary file
            if (_debugNameDatabase.ContainsKey(packet.Id))
            {
                if (!File.Exists("packetcaptures\\" + _debugNameDatabase[packet.Id] + ".bin"))
                    File.WriteAllBytes("packetcaptures\\" + _debugNameDatabase[packet.Id] + ".bin", packet.Buffer);
            }
            else if (!File.Exists("packetcaptures\\" + packet.Id + ".bin"))
                File.WriteAllBytes("packetcaptures\\" + packet.Id + ".bin", packet.Buffer);
#endif
            
            // Handle the packet.
            if (_parsers.ContainsKey(packet.Id))
            {
#if DEBUG
                if (_debugNameDatabase.ContainsKey(packet.Id))
                {
                    // Stop frequent packets from spamming the console.
                    if (packet.Id != Packets.CmdUnknownSync || packet.Id != Packets.CmdNullPing ||
                        packet.Id != Packets.UdpCastTcsSignalAck || packet.Id != Packets.CmdUdpCastTcsSignal)
                        Log.Info("Handling packet {2} (id {0}, 0x{0:X}) on {1}.", packet.Id, _port,
                            _debugNameDatabase[packet.Id]);
                }
#endif
                _parsers[packet.Id](packet);
            }
            else
            {
#if DEBUG
                if (_debugNameDatabase.ContainsKey(packet.Id))
                {
                    Log.Info("Received unhandled packet {2} (id {0}, 0x{0:X}) on {1}.", packet.Id, _port,
                        _debugNameDatabase[packet.Id]);
                    Log.Debug("HexDump:{0}{1}", Environment.NewLine, hexDump);
                    return;
                }
#endif
                    Log.Error("Received unhandled packet (id {0}, 0x{0:X}) on {1}.", packet.Id, _port);
            }
        }

        private IEnumerable<Client> GetClients() => _clients.ToArray();

        public void Broadcast(Packet packet, Client exclude = null)
        {
            foreach (var client in GetClients())
                if (exclude == null || client != exclude)
                    client.Send(packet);
        }
    }
}