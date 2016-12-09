using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Network;
using Shared.Util;

namespace Shared.Objects
{
    public class IUnit
    {
        /*
        struct XiStrItemUnit
        {
          int StackNum;
          int Random;
          unsigned int AssistA;
          unsigned int AssistB;
          unsigned int Box;
          unsigned int Belonging;
          int Upgrade;
          int UpgradePoint;
          unsigned int ExpireTick;
        };
        */
        public int StackNum;
        public int Random;

        public uint AssistA;
        public uint AssistB;
        public uint Box;
        public uint Belonging;

        public int Upgrade;
        public int UpgradePoint;

        public uint ExpireTick;
    }

    public class ItemData
    {
        /*
        struct $46506E0D494CF120A19388EB37177777
        {
          unsigned __int16 State;
          unsigned __int16 Slot;
        };
        union $90E2572CC35A071924DAD0BC1A98978B
        {
          $46506E0D494CF120A19388EB37177777 __s0;
                unsigned int StateVar;
            };
        */

        public ushort State;
        public ushort Slot;

        public uint StateVar;
    }

    public class Item : ISerializable
    {
        /*
        struct XiStrMyItem
        {
          unsigned int CarID;
          $90E2572CC35A071924DAD0BC1A98978B Itm;
          XiStrItemUnit ItemUnit;
          unsigned int TableIdx;
          unsigned int InvenIdx;
        };
        */
        public uint CarID;

        public ItemData Itm;
        public IUnit iunit;

        public uint TableIdx;
        public uint InvenIdx;

        public void Serialize(PacketWriter writer)
        {
            writer.Write(CarID);

            writer.Write(Itm.State);
            writer.Write(Itm.Slot);
            writer.Write(Itm.StateVar);

            writer.Write(iunit.StackNum);
            writer.Write(iunit.Random);

            writer.Write(iunit.AssistA);
            writer.Write(iunit.AssistB);

            writer.Write(iunit.Box);
            writer.Write(iunit.Belonging);

            writer.Write(iunit.Upgrade);
            writer.Write(iunit.UpgradePoint);

            writer.Write(iunit.ExpireTick);


            writer.Write(TableIdx);
            writer.Write(InvenIdx);

            Log.Debug("Bufferlength: " + writer.GetBuffer().Length);
        }
    }
}
