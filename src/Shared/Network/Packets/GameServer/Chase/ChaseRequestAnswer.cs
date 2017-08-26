using System.IO;
using System.Threading;
using Shared.Network.AreaServer;
using Shared.Util;

namespace Shared.Network.GameServer
{
    public class ChaseRequestAnswer : OutPacket
    {
        /*
        unsigned __int16 m_Serial;
        XiVec4 m_StartPos;
        XiVec4 m_EndPos;
        int m_Type;
        int m_courseId;
        int m_firstHuvLevel;
        int m_firstHuvId;
        wchar_t m_PosName[100];
        */
        public ushort Unknown1 = 0; // Serial?
        public float StartPosX;
        public float StartPosY;
        public float StartPosZ;
        public float StartRot;
        
        public float EndPosX;
        public float EndPosY;
        public float EndPosZ;
        public float EndRot;

        public int CourseId;
        public int Type;
        public string PosName;
        public int FirstHuvLevel;
        public int FirstHuvId = 10001;
        public byte[] Unknown2 = new byte[2];
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.ChasePropose);
        }

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(Unknown1);
                    bs.Write(StartPosX); // Start X
                    bs.Write(StartPosY); // Start Y
                    bs.Write(StartPosZ); // Start Z
                    bs.Write(StartRot); // Start W

                    bs.Write(EndPosX); // End X
                    bs.Write(EndPosY); // End Y
                    bs.Write(EndPosZ); // End Z
                    bs.Write(EndRot); // End Rot

                    bs.Write(CourseId); // CourseId
                    bs.Write(Type); // Type?
                    bs.WriteUnicodeStatic(PosName, 100);

                    bs.Write(FirstHuvLevel); // HUV first level
                    bs.Write(FirstHuvId); // HUV first Id
                    bs.Write(Unknown2); // Not sure.
                }
                return ms.GetBuffer();
            }
        }
    }
}