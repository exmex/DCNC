using Shared.Util;

namespace Shared.Objects
{
    public class XiStrEnchantBonus : BinaryWriterExt.ISerializable
    {
        public int Speed;
        public int Crash;
        public int Accel;
        public int Boost;
        public int AddSpeed;
        public float Drop;
        public float Exp;
        public float MitronCapacity;
        public float MitronEfficiency;
        
        
        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(Speed);            
            writer.Write(Crash);            
            writer.Write(Accel);            
            writer.Write(Boost);            
            writer.Write(AddSpeed);            
            writer.Write(Drop);            
            writer.Write(Exp);            
            writer.Write(MitronCapacity);            
            writer.Write(MitronEfficiency);            
        }
    }
}