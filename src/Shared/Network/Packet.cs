using System;
using System.IO;
using Shared.Util;

namespace Shared.Network
{
    public class Packet
    {
        public readonly byte[] Buffer;
        public readonly ushort Id;
        public readonly BinaryReaderExt Reader;

        public readonly BinaryWriterExt Writer;
        public readonly Client Sender;

        public Packet(ushort id)
        {
            Writer = new BinaryWriterExt(new MemoryStream());
            Id = id;

            Writer.Write(id);
        }

        public Packet(Client sender, ushort id, byte[] buffer)
        {
            Sender = sender;
            Buffer = buffer;
            Id = id;
            Reader = new BinaryReaderExt(new MemoryStream(Buffer));
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class PacketAttribute : Attribute
    {
        public PacketAttribute(ushort id)
        {
            Id = id;
        }

        public ushort Id { get; }
    }
}