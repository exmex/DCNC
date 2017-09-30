using System.IO;
using Shared.Models;
using Shared.Util;

namespace Shared.Network.GameServer
{
    /// <summary>
    /// sub_52E910
    /// </summary>
    public class MyQuestListAnswer : OutPacket
    {
        public Quest[] Quests;
        
        public override Packet CreatePacket()
        {
            return base.CreatePacket(Packets.MyQuestListAck);
        }
        
        public override int ExpectedSize() => (14 * Quests.Length-1)+20;

        public override byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bs = new BinaryWriterExt(ms))
                {
                    bs.Write(Quests.Length);
                    foreach (var quest in Quests)
                    {
                        bs.Write(quest.QuestId);
                        bs.Write(quest.State);
                        bs.Write(quest.PlaceIdx);
                        bs.Write(quest.FailNum);
                    }
                }
                return ms.ToArray();
            }
            
            /*
            ack.Writer.Write(quests.Count);
            foreach (var quest in quests)
            {
                ack.Writer.Write(quest.QuestId);
                ack.Writer.Write(quest.State);
                ack.Writer.Write(quest.PlaceIdx);
                ack.Writer.Write(quest.FailNum);
            }
            
            /*
            //ack.Writer.Write(0); // Quest num
            ack.Writer.Write(1); // Quest num


            ack.Writer.Write((uint)0);
            ack.Writer.Write((uint)0);
            ack.Writer.Write((uint)0);
            ack.Writer.Write((ushort)0);*/
            /*
            unsigned int TableIdx;
            unsigned int State;
            unsigned int PlaceIdx;
            unsigned __int16 FailNum;
            */
        }
    }
}