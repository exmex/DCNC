using Shared.Util;


namespace Shared.Objects
{
    public class XiStrMemberInfo : BinaryWriterExt.ISerializable
    {
        public XiStrCharInfo CharInfo;
        public XiCarAttr CarAttr;

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write((BinaryWriterExt.ISerializable)CharInfo);
            writer.Write((BinaryWriterExt.ISerializable)CarAttr);
            
        }
    }
}
