using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCommon.Classes
{
    public class Packet
    {
        public GameClient Sender;

        public PacketWriter Writer;
        public PacketReader Reader;

        public byte[] Buffer;
        public ushort ID;

        public Packet(ushort id)
        {
            Writer = new PacketWriter(new MemoryStream());
            ID = id;

            Writer.Write(id);
        }

        public Packet(GameClient sender, ushort id, byte[] buffer)
        {
            Sender = sender;
            Buffer = buffer;
            ID = id;
            Reader = new PacketReader(new MemoryStream(Buffer));
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    internal sealed class PacketAttribute : Attribute
    {
        private readonly ushort _id;

        public PacketAttribute(ushort id)
        {
            this._id = id;
        }

        public ushort Id => _id;
    }
}
