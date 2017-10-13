using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using Shared.Network.GameServer;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network
{
    public class DefaultServer
    {
#if DEBUG
        /// <summary>
        /// A Dictionary containing packets and their friendly name
        /// </summary>
        public static Dictionary<ushort, string> PacketNameDatabase;
#endif
        
        /// <summary>
        /// A list of all connected clients
        /// </summary>
        private readonly List<Client> _clients;
        
        /// <summary>
        /// Does this server require an exchange?
        /// </summary>
        private readonly bool _exchangeRequired;
        
        /// <summary>
        /// The underlaying TCP Listener
        /// </summary>
        private readonly TcpListener _listener;

        /// <summary>
        /// Contains all parsers and their function
        /// </summary>
        private readonly Dictionary<ushort, Action<Packet>> _parsers;

        /// <summary>
        /// The port the server runs on
        /// </summary>
        private readonly int _port;
        
        /// <summary>
        /// Specifies wether or not we should dump incoming packets to a file!
        /// </summary>
        public static bool DumpIncoming { get; set; }
        
        /// <summary>
        /// Specifies wether or not we should dump outgoing packets to a file!
        /// </summary>
        public static bool DumpOutgoing { get; set; }

        public ushort LastSerial = 0;
        public static Dictionary<ushort, User> ActiveSerials = new Dictionary<ushort, User>();

#if DEBUG
        public static readonly List<ushort> PacketDumpBlacklist = new List<ushort>()
        {
            Packets.CmdUnknownSync,
            Packets.CmdNullPing,
            Packets.CmdUdpCastTcsSignal,
            Packets.UdpCastTcsSignalAck,
            Packets.CmdMoveVehicle
        };
#endif

        public DefaultServer(int port, bool exchangeRequired = true)
        {
#if DEBUG
            if (PacketNameDatabase == null)
            {
                PacketNameDatabase = new Dictionary<ushort, string>();
                if (File.Exists("system/parsers.txt"))
                {
                    //string src = new WebClient().DownloadString("http://u.rtag.me/p/parsers.txt");
                    var src = File.ReadAllText("system/parsers.txt");

                    foreach (var line in src.Split('\n'))
                    {
                        if (line.Length <= 3) continue;
                        var lineSplit = line.Split(':');

                        var id = ushort.Parse(lineSplit[0]);

                        PacketNameDatabase[id] = lineSplit[1].Trim().Split('_')[1];
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
            i += AddAllMethodsFromType(Assembly.GetEntryAssembly().GetTypes());
            i += AddAllMethodsFromType(Assembly.GetExecutingAssembly().GetTypes());
#if DEBUG
            Log.Info("Added {0} packets", i);
#endif
        }

        private int AddAllMethodsFromType(IEnumerable<Type> types)
        {
            var i = 0;
            foreach (var type in types)
            foreach (var method in type.GetMethods())
            foreach (var boxedAttrib in method.GetCustomAttributes(typeof(PacketAttribute), false))
            {
                var attrib = boxedAttrib as PacketAttribute;

                if (attrib == null) continue;
                var id = attrib.Id;
                var parser = (Action<Packet>) Delegate.CreateDelegate(typeof(Action<Packet>), method);

                SetParser(id, parser);
                
#if DEBUG
                i++;
#endif
            }
            return i;
        }

        public void Start()
        {
            Log.Info("Starting network", _port);

            _listener.Start();
            _listener.BeginAcceptTcpClient(OnAccept, _listener);

            Log.Info("Network started on port {0}", _port);
        }

        private void OnAccept(IAsyncResult result)
        {
            var tcpClient = _listener.EndAcceptTcpClient(result);
            var client = new Client(tcpClient, this, _exchangeRequired);

#if DEBUG
            Log.Info("Accepted client from {0} on {1}", tcpClient.Client.RemoteEndPoint, _port);
#endif

            _clients.Add(client);
            _listener.BeginAcceptTcpClient(OnAccept, _listener);
        }

        private void SetParser(ushort id, Action<Packet> parser)
        {
            if (_parsers.ContainsKey(id))
            {
#if DEBUG
                if(PacketNameDatabase.ContainsKey(id))
                    Log.Error("Duplicated parser for packet {0} ({1} {2} : 0x{2:X}).", PacketNameDatabase[id], Packets.GetName(id), id);
                else
#endif
                    Log.Error("Duplicated parser for packet ({0} {1} : 0x{1:X}).", Packets.GetName(id), id);
            }
#if DEBUG
            if(PacketNameDatabase.ContainsKey(id))
                Log.Debug("Added parser for packet {0} ({1} {2} : 0x{2:X}).", PacketNameDatabase[id], Packets.GetName(id), id);
            else
                Log.Debug("Added parser for packet ({0} {1} : 0x{1:X}).", Packets.GetName(id), id);
#endif
            _parsers[id] = parser;
        }

        public void Parse(Packet packet)
        {
#if DEBUG
            var hexDump = BinaryWriterExt.HexDump(packet.Buffer);

            if (DumpIncoming)
            {
                // Make sure the packetcaptures directory exists.
                Directory.CreateDirectory("packetcaptures\\incoming\\");

                // Dump the received data in hex
                if (PacketNameDatabase.ContainsKey(packet.Id))
                {
                    if (!File.Exists("packetcaptures\\incoming\\" + PacketNameDatabase[packet.Id] + ".txt"))
                        File.WriteAllText("packetcaptures\\incoming\\" + PacketNameDatabase[packet.Id] + ".txt", hexDump);
                }
                else if (!File.Exists("packetcaptures\\incoming\\" + packet.Id + ".txt"))
                    File.WriteAllText("packetcaptures\\incoming\\" + packet.Id + ".txt", hexDump);

                // Dump the received data into a binary file
                if (PacketNameDatabase.ContainsKey(packet.Id))
                {
                    if (!File.Exists("packetcaptures\\incoming\\" + PacketNameDatabase[packet.Id] + ".bin"))
                        File.WriteAllBytes("packetcaptures\\incoming\\" + PacketNameDatabase[packet.Id] + ".bin", packet.Buffer);
                }
                else if (!File.Exists("packetcaptures\\incoming\\" + packet.Id + ".bin"))
                    File.WriteAllBytes("packetcaptures\\incoming\\" + packet.Id + ".bin", packet.Buffer);
            }
#endif
            
            // Handle the packet.
            if (_parsers.ContainsKey(packet.Id))
            {
#if DEBUG
                // Stop frequent packets from spamming the console.
                if (!PacketDumpBlacklist.Contains(packet.Id))
                {
                    if (PacketNameDatabase.ContainsKey(packet.Id))
                    {
                        Log.Info("Handling packet {0} ({1} id {2}, 0x{2:X}) on {3}.", PacketNameDatabase[packet.Id],
                            Packets.GetName(packet.Id), packet.Id, _port);
                    }
                    else
                    {
                        Log.Info("Handling unnamed packet ({0} id {1}, 0x{1:X}) on {2}.", Packets.GetName(packet.Id), packet.Id, _port);
                    }

                    Log.Debug("HexDump {0}:{1}{2}", packet.Id, Environment.NewLine, hexDump);
                }
#endif
                _parsers[packet.Id](packet);
            }
            else
            {
#if DEBUG
                if (PacketNameDatabase.ContainsKey(packet.Id))
                {
                    Log.Info("Received unhandled packet {0} ({1} id {2}, 0x{2:X}) on {3}.", PacketNameDatabase[packet.Id],
                        Packets.GetName(packet.Id), packet.Id, _port);
                    Log.Debug("HexDump  {0} ({1} id {2}, 0x{2:X}):{3}{4}", PacketNameDatabase[packet.Id],
                        Packets.GetName(packet.Id), packet.Id, Environment.NewLine, hexDump);
                    return;
                }
#endif
                Log.Warning("Received unhandled packet {0} (id {1}, 0x{1:X}) on {2}.", Packets.GetName(packet.Id), packet.Id, _port);
#if DEBUG
                Log.Debug("HexDump {0} (id {1}, 0x{1:X}):{2}{3}", Packets.GetName(packet.Id), packet.Id, Environment.NewLine, hexDump);
#endif
            }
        }

        /// <summary>
        /// Gets a client from it's char name
        /// </summary>
        /// <param name="characterName">The character name to fetch</param>
        /// <returns>A client or null</returns>
        public Client GetClient(string characterName)
        {
            return _clients.FirstOrDefault(client => client?.User?.ActiveCharacter?.Name == characterName);
        }

        /// <summary>
        /// Gets a client from it's vehicle serial
        /// </summary>
        /// <param name="vehicleSerial">The vehicle serial to fetch</param>
        /// <returns>A client or null</returns>
        public Client GetClient(ushort vehicleSerial)
        {
            return _clients.FirstOrDefault(client => client?.User?.VehicleSerial == vehicleSerial);
        }

        /// <summary>
        /// Gets multiple clients based on it's vehicle serials
        /// </summary>
        /// <param name="vehicleSerial">An array of vehicle serials to fetch</param>
        /// <returns>A collection of clients or possbly null?</returns>
        public IEnumerable<Client> GetClients(ushort[] vehicleSerial)
        {
            return _clients.FindAll(client => client.User != null && vehicleSerial.Contains(client.User.VehicleSerial));
        }

        /// <summary>
        /// Gets all clients connected to the server
        /// </summary>
        /// <returns>A collection of all clients connected to the server</returns>
        public IEnumerable<Client> GetClients() => _clients.ToArray();

        /// <summary>
        /// Broadcasts a packet to all clients on the server, excluding exclude if specified
        /// </summary>
        /// <param name="packet">The packet to broadcast</param>
        /// <param name="exclude">The client to exclude</param>
        public void Broadcast(Packet packet, Client exclude = null)
        {
            foreach (var client in GetClients())
                if (exclude == null || client != exclude)
                    client.Send(packet);
        }
        
        /// <summary>
        /// Broadcasts a packet to all clients on the server, excluding exclude if specified
        /// </summary>
        /// <param name="packet">The packet to broadcast</param>
        /// <param name="exclude">An array of clients that should be exluded</param>
        public void Broadcast(Packet packet, Client[] exclude)
        {
            foreach (var client in GetClients())
                if (!exclude.Contains(client))
                    client.Send(packet);
        }

        /// <summary>
        /// Broadcasts a packet to all clients in the specified team/crew
        /// </summary>
        /// <param name="team">The team/crew to broadcast to</param>
        /// <param name="packet">The packet to broadcast</param>
        public void Broadcast(Team team, Packet packet)
        {
            foreach (var client in GetClients())
            {
                if(client.User?.ActiveCharacter?.TeamId == team.Id)
                    client.Send(packet);
            }
        }
        
        /*
        /// <summary>
        /// Broadcasts a packet to all clients in the specified party
        /// </summary>
        /// <param name="party"></param>
        /// <param name="packet"></param>
        public void Broadcast(Party party, Packet packet)
        {
            
        }
        */

        /// <summary>
        /// Broadcasts a packet to all clients in the same area as areaId
        /// TODO: Currently broadcasts everywhere.
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="packet"></param>
        public void BroadcastArea(int areaId, Packet packet)
        {
            foreach (var client in GetClients())
            {
                client.Send(packet);
            }
        }
        
        /// <summary>
        /// Broadcasts a packet to all clients in the same channel as channelId
        /// TODO: Currently broadcasts everywhere.
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="packet"></param>
        public void BroadcastChannel(int channelId, Packet packet)
        {
            foreach (var client in GetClients())
            {
                if(client.User?.ActiveCharacter?.LastChannel == channelId)
                    client.Send(packet);
            }
        }
    }
}