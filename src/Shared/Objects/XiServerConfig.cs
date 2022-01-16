using Shared.Util;


namespace Shared.Objects
{
    public class XiServerConfig : BinaryWriterExt.ISerializable
    {
        public int Auth2Pass;
        public int CBattleDay;
        public int CBattleHour;
        public int DormantEvent;
        public int LevelMitoEvent;
        public int CBUserEvent;
        public int MitoDrinkBonus;
        public int SpeedHackThreshold;
        public int DisconnectHack;
        public int WarnHack;
        public int MaxMainCh;
        public int MaxAllCh;
        public int InviteFriendEvent;
        public int CoinCashExpireTime;

        public void Serialize(BinaryWriterExt writer)
        {
            writer.Write(Auth2Pass);
            writer.Write(CBattleDay);
            writer.Write(CBattleHour);
            writer.Write(DormantEvent);
            writer.Write(LevelMitoEvent);
            writer.Write(CBUserEvent);
            writer.Write(MitoDrinkBonus);
            writer.Write(SpeedHackThreshold);
            writer.Write(DisconnectHack);
            writer.Write(WarnHack);
            writer.Write(MaxMainCh);
            writer.Write(MaxAllCh);
            writer.Write(InviteFriendEvent);
            writer.Write(CoinCashExpireTime);
            
        }

    }
}
