namespace Shared.Objects
{
    
    public class _42B970CC5B25DBF678833D7A2C3F7BDA
    {
        public ushort Sort;
        public ushort Body;
        public char[] Color; // 4

        public _42B970CC5B25DBF678833D7A2C3F7BDA()
        {
            Color = new char[4];
        }
    };
        
    public class _A1194D3E500092C208560D6327859824
    {
        public int lvalSortBody;
        public int lvalColor;
    };
    
    public class _566EC0F75D06BC4759069039DB5ABCE0
    {
        public _42B970CC5B25DBF678833D7A2C3F7BDA __s0;
        public _A1194D3E500092C208560D6327859824 __s1;
        public long llval;

        public _566EC0F75D06BC4759069039DB5ABCE0()
        {
            __s0 = new _42B970CC5B25DBF678833D7A2C3F7BDA();
            __s1 = new _A1194D3E500092C208560D6327859824();
        }
    }
    public class XiCarAttr
    {
        public _566EC0F75D06BC4759069039DB5ABCE0 ___u0;

        public XiCarAttr()
        {
            ___u0 = new _566EC0F75D06BC4759069039DB5ABCE0();
        }

        /*
        struct $42B970CC5B25DBF678833D7A2C3F7BDA
        {
            unsigned __int16 Sort;
            unsigned __int16 Body;
            char Color[4];
        };
        
        struct $A1194D3E500092C208560D6327859824
        {
            int lvalSortBody;
            int lvalColor;
        };
        union $566EC0F75D06BC4759069039DB5ABCE0
        {
            $42B970CC5B25DBF678833D7A2C3F7BDA __s0;
                $A1194D3E500092C208560D6327859824 __s1;
            __int64 llval;
        };
        struct XiCarAttr
        {
            $566EC0F75D06BC4759069039DB5ABCE0 ___u0;
    };
        */
    }
}