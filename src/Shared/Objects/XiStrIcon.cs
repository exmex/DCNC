namespace Shared.Objects
{
    public class XiStrIcon
    {
        public class XiStrPos
        {
            public float X;
            public float Y;
            public float Z;
        }
        public char[] Index; // 255
        public char[] Name; // 255
        public XiStrPos Pos;
        public uint Type;
        public bool Check;
        public int MapId;
        public int Guild;
        public uint Idx;
        //public List<XiStrArbeit> ArbeitList;
    }
}