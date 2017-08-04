namespace Shared.Objects
{
    public class XiStrIcon
    {
        public bool Check;
        public int Guild;
        public uint Idx;
        public char[] Index; // 255
        public int MapId;
        public char[] Name; // 255
        public XiStrPos Pos;
        public uint Type;

        public class XiStrPos
        {
            public float X;
            public float Y;
            public float Z;
        }

        //public List<XiStrArbeit> ArbeitList;
    }
}