using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Shared.Objects;
using Shared.Util;
using Shared.Util.Configuration.Files;

namespace Shared.Network
{
    public class Client
    {
        private readonly NetworkStream _ns;
        private readonly DefaultServer _parent;
        private readonly TcpClient _tcp;

        private byte[] _buffer;
        private int _bytesToRead;

        private bool _connected;
        private ushort _packetLength, _packetId;

        public User User;

        public Client(TcpClient tcp, DefaultServer parent, bool exchangeRequired)
        {
            _tcp = tcp;
            _parent = parent;

            _ns = tcp.GetStream();
            _connected = true;

            try
            {
                if (exchangeRequired)
                {
                    _buffer = new byte[56];
                    _bytesToRead = _buffer.Length;
                    _ns.BeginRead(_buffer, 0, 56, OnExchange, null);
                }
                else
                {
                    _buffer = new byte[4];
                    _bytesToRead = _buffer.Length;
                    _ns.BeginRead(_buffer, 0, 4, OnHeader, null);
                }
            }
            catch (Exception ex)
            {
                KillConnection(ex);
            }
        }

        public IPEndPoint EndPoint => _tcp.Client.RemoteEndPoint as IPEndPoint;

        private void OnExchange(IAsyncResult result)
        {
            try
            {
                _bytesToRead -= _ns.EndRead(result);
                if (_bytesToRead > 0)
                {
                    _ns.BeginRead(_buffer, _buffer.Length - _bytesToRead, _bytesToRead, OnExchange, null);
                    return;
                }

                _ns.Write(new byte[56], 0, 56);

                _buffer = new byte[4];
                _bytesToRead = _buffer.Length;
                _ns.BeginRead(_buffer, 0, 4, OnHeader, null);
            }
            catch (Exception ex)
            {
                KillConnection(ex);
            }
        }

        private void OnHeader(IAsyncResult result)
        {
            try
            {
                _bytesToRead -= _ns.EndRead(result);
                if (_bytesToRead > 0)
                {
                    _ns.BeginRead(_buffer, _buffer.Length - _bytesToRead, _bytesToRead, OnHeader, null);
                    return;
                }

                _packetLength = BitConverter.ToUInt16(_buffer, 0);
                _packetId = BitConverter.ToUInt16(_buffer, 2);

                _bytesToRead = _packetLength - 4;
                _buffer = new byte[_bytesToRead];
                _ns.BeginRead(_buffer, 0, _bytesToRead, OnData, null);
            }
            catch (Exception ex)
            {
                KillConnection(ex);
            }
        }

        private void OnData(IAsyncResult result)
        {
            try
            {
                _bytesToRead -= _ns.EndRead(result);
                if (_bytesToRead > 0)
                {
                    _ns.BeginRead(_buffer, _buffer.Length - _bytesToRead, _bytesToRead, OnData, null);
                    return;
                }

                var packet = new Packet(this, _packetId, _buffer);
                _parent.Parse(packet);

                _buffer = new byte[4];
                _bytesToRead = _buffer.Length;
                _ns.BeginRead(_buffer, 0, 4, OnHeader, null);
            }
            catch (Exception ex)
            {
                KillConnection(ex);
            }
        }

        public void Send(Packet packet)
        {
#if DEBUG
            var buffer = packet.Writer.GetBuffer();

            var bufferLength = buffer.Length;
            var length = (ushort) (bufferLength + 2); // Length includes itself

            var hexDump = BinaryWriterExt.HexDump(buffer);
            
            // Stop frequent packets from spamming the console.
            if (packet.Id != Packets.UdpCastTcsSignalAck)
            {
                if(DefaultServer.PacketNameDatabase.ContainsKey(packet.Id))
                    Log.Info("Sending packet {0} ({1} id {2}, 0x{2:X}).", DefaultServer.PacketNameDatabase[packet.Id],
                        Packets.GetName(packet.Id), packet.Id);
                else
                    Log.Info("Sending unnamed packet ({0} id {1}, 0x{1:X}).",
                        Packets.GetName(packet.Id), packet.Id);
            }

            if (DefaultServer.DumpOutgoing)
            {
                // Make sure the packetcaptures directory exists.
                Directory.CreateDirectory("packetcaptures\\outgoing\\");

                // Dump the received data in hex
                if (DefaultServer.PacketNameDatabase.ContainsKey(packet.Id))
                {
                    if (!File.Exists(
                        "packetcaptures\\outgoing\\" + DefaultServer.PacketNameDatabase[packet.Id] + ".txt"))
                        File.WriteAllText(
                            "packetcaptures\\outgoing\\" + DefaultServer.PacketNameDatabase[packet.Id] + ".txt",
                            hexDump);
                }
                else if (!File.Exists("packetcaptures\\outgoing\\" + packet.Id + ".txt"))
                    File.WriteAllText("packetcaptures\\outgoing\\" + packet.Id + ".txt", hexDump);

                // Dump the received data into a binary file
                if (DefaultServer.PacketNameDatabase.ContainsKey(packet.Id))
                {
                    if (!File.Exists(
                        "packetcaptures\\outgoing\\" + DefaultServer.PacketNameDatabase[packet.Id] + ".bin"))
                        File.WriteAllBytes(
                            "packetcaptures\\outgoing\\" + DefaultServer.PacketNameDatabase[packet.Id] + ".bin",
                            buffer);
                }
                else if (!File.Exists("packetcaptures\\outgoing\\" + packet.Id + ".bin"))
                    File.WriteAllBytes("packetcaptures\\outgoing\\" + packet.Id + ".bin", buffer);
            }
#endif

            try
            {
                _ns.Write(BitConverter.GetBytes(length), 0, 2);
                _ns.Write(buffer, 0, bufferLength);
            }
            catch (Exception ex)
            {
                KillConnection(ex);
            }
        }

        public void SendError(string format, params object[] args)
        {
            var err = new Packet(1);
            err.Writer.WriteUnicode(string.Format(format, args));
            Send(err);
        }

        private void KillConnection(Exception ex)
        {
            if (ex is SocketException || ex is IOException)
            {
                KillConnection("Socket or IO Exception");
                return;
            }

            KillConnection(ex.Message + ": " + ex.StackTrace);
        }

        public void KillConnection(string reason = "")
        {
            if (!_connected) return;
            _connected = false;

            Log.Info("Killing off client. {0}", reason);
            _tcp.Close();
        }
    }
}