using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using Shared.Objects;
using Shared.Util;

namespace Shared.Network
{
    public class Client
    {
        private readonly TcpClient _tcp;
        private readonly NetworkStream _ns;
        private readonly DefaultServer _parent;

        private byte[] _buffer;
        private int _bytesToRead;
        private ushort _packetLength, _packetId;

        private bool _connected;
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
                Kill(ex);
            }
        }

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
            catch (Exception ex) { Kill(ex); }
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
            catch (Exception ex) { Kill(ex); }
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
            catch (Exception ex) { Kill(ex); }
        }

        public void Send(Packet packet)
        {
            byte[] buffer = packet.Writer.GetBuffer();

            int bufferLength = buffer.Length;
            ushort length = (ushort)(bufferLength + 2); // Length includes itself

            int bytesPerLine = 16;
            string hexDump = "";
            int j = 0;
            foreach (var g in buffer.Select((c, i) => new { Char = c, Chunk = i / bytesPerLine }).GroupBy(c => c.Chunk))
            {
                var s1 = g.Select(c => $"{c.Char:X2} ").Aggregate((s, i) => s + i);
                string s2 = null;
                bool first = true;
                foreach (var c in g)
                {
                    var s = $"{(c.Char < 32 || c.Char > 122 ? '·' : (char)c.Char)} ";
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

            Log.Debug("Sending ({0}): {1}", packet.Id, hexDump);

            try
            {
                _ns.Write(BitConverter.GetBytes(length), 0, 2);
                _ns.Write(buffer, 0, bufferLength);
            }
            catch (Exception ex) { Kill(ex); }
        }

        public void Error(string format, params object[] args)
        {
            var err = new Packet(1);
            err.Writer.WriteUnicode(string.Format(format, args));
            Send(err);
        }

        private void Kill(Exception ex)
        {
            if (ex is SocketException || ex is IOException)
            {
                Kill("Socket or IO Exception");
                return;
            }

#if DEBUG
            Error(ex.Message + ": " + ex.StackTrace);
#endif

            Kill(ex.Message + ": " + ex.StackTrace);
        }

        public void Kill(string reason = "")
        {
            if (!_connected) return;
            _connected = false;

            Log.Info("Killing off client. {0}", reason);
            _tcp.Close();
        }
    }
}
