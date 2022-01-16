using Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Objects
{
    public class XiStrUserInfo : BinaryWriterExt.ISerializable
    {
        public XiStrUserPermission Permission;
        public uint LastPlayTime;
        public uint TotalPlayTime;
        public XiStrCharName LastChar;

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(Permission);
            writer.Write(LastPlayTime);
            writer.Write(TotalPlayTime);
            writer.Write((BinaryWriterExt.ISerializable)LastChar);


        }

    }
}
