using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCommon.Classes
{
    public interface ISerializable
    {
        void Serialize(PacketWriter writer);
    }
}
