using Shared.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Objects
{
    public class XiStrItemUnit : BinaryWriterExt.ISerializable
    {
        public int StackNum;
        public int Random;
        public uint AssistA;
        public uint AssistB;
        public uint AssistC;
        public uint AssistD;
        public uint AssistE;
        public uint AssistF;
        public uint AssistG;
        public uint AssistH;
        public uint AssistI;
        public uint AssistJ;
        public uint Box;
        public uint Belonging;
        public int Upgrade;
        public int UpgradePoint;
        public uint ExpireTick;

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(StackNum);
            writer.Write(Random);
            writer.Write(AssistA);
            writer.Write(AssistB);
            writer.Write(AssistC);
            writer.Write(AssistD);
            writer.Write(AssistE);
            writer.Write(AssistF);
            writer.Write(AssistG);
            writer.Write(AssistH);
            writer.Write(AssistI);
            writer.Write(AssistJ);
            writer.Write(Box);
            writer.Write(Belonging);
            writer.Write(Upgrade);
            writer.Write(UpgradePoint);
            writer.Write(ExpireTick);
        }

    }
}
