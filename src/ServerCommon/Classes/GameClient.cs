using System;
using System.IO;
using System.Net.Sockets;

namespace ServerCommon.Classes
{
    public class GameClient
    {
        private TcpClient _tcpClient;
        private NetworkStream _networkStream;
        private MittronMadnessServer _parent;
        private bool _exchangeRequired;

        private byte[] _buffer;
        private int _bytesToRead;
        private ushort _packetLength, _packetId;

        public bool Alive;

        public GameClient(TcpClient tcpClient, MittronMadnessServer parent, bool exchangeRequired)
        {
            _tcpClient = tcpClient;
            _parent = parent;
            _exchangeRequired = exchangeRequired;

            _networkStream = tcpClient.GetStream();
            Alive = true;

            try
            {
                if (exchangeRequired)
                {
                    _buffer = new byte[56];
                    _bytesToRead = _buffer.Length;
                    _networkStream.BeginRead(_buffer, 0, 56, OnExchange, null);
                }
                else
                {
                    _buffer = new byte[4];
                    _bytesToRead = _buffer.Length;
                    _networkStream.BeginRead(_buffer, 0, 4, OnHeader, null);
                }
            }
            catch (Exception ex) { Kill(ex); }
        }

        private void OnExchange(IAsyncResult result)
        {
            try
            {
                _bytesToRead -= _networkStream.EndRead(result);
                if (_bytesToRead > 0)
                {
                    _networkStream.BeginRead(_buffer, _buffer.Length - _bytesToRead, _bytesToRead, OnExchange, null);
                    return;
                }

                _networkStream.Write(new byte[56], 0, 56);

                _buffer = new byte[4];
                _bytesToRead = _buffer.Length;
                _networkStream.BeginRead(_buffer, 0, 4, OnHeader, null);
            }
            catch (Exception ex) { Kill(ex); }
        }

        private void OnHeader(IAsyncResult result)
        {
            try
            {
                _bytesToRead -= _networkStream.EndRead(result);
                if (_bytesToRead > 0)
                {
                    _networkStream.BeginRead(_buffer, _buffer.Length - _bytesToRead, _bytesToRead, OnHeader, null);
                    return;
                }

                _packetLength = BitConverter.ToUInt16(_buffer, 0);
                _packetId = BitConverter.ToUInt16(_buffer, 2);

                _bytesToRead = _packetLength - 4;
                _buffer = new byte[_bytesToRead];
                _networkStream.BeginRead(_buffer, 0, _bytesToRead, OnData, null);
            }
            catch (Exception ex) { Kill(ex); }
        }

        private void OnData(IAsyncResult result)
        {
            try
            {
                _bytesToRead -= _networkStream.EndRead(result);
                if (_bytesToRead > 0)
                {
                    _networkStream.BeginRead(_buffer, _buffer.Length - _bytesToRead, _bytesToRead, OnData, null);
                    return;
                }

                var packet = new Packet(this, _packetId, _buffer);
                _parent.Parse(packet);

                _buffer = new byte[4];
                _bytesToRead = _buffer.Length;
                _networkStream.BeginRead(_buffer, 0, 4, OnHeader, null);
            }
            catch (Exception ex) { Kill(ex); }
        }

        public void Send(Packet packet)
        {
            byte[] buffer = packet.Writer.GetBuffer();

            int bufferLength = buffer.Length;
            ushort length = (ushort)(bufferLength + 2); // Length includes itself

            try
            {
                _networkStream.Write(BitConverter.GetBytes(length), 0, 2);
                _networkStream.Write(buffer, 0, bufferLength);
            }
            catch (Exception ex) { Kill(ex); }
        }

        public void Error(string format, params object[] args)
        {
            var err = new Packet(1);
            err.Writer.WriteUnicode(String.Format(format, args));
            Send(err);
        }

        public void Kill(Exception ex)
        {
            if (ex is SocketException || ex is IOException)
            {
                Kill();
                return;
            }

            Kill(ex.Message + ": " + ex.StackTrace);
        }

        public void Kill(string reason = "")
        {
            if (!Alive) return;
            Alive = false;

            Logger.Warning("Killing off client. {0}", reason);
            _tcpClient.Close();
        }
    }
}