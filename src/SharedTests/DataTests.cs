using System.Net;
using NUnit.Framework;
using Shared.Objects;
using Shared.Objects.GameDatas;

namespace SharedTests
{
    [TestFixture]
    public class DataTests
    {
        // TODO: This still fails.
        [Test]
        public static void TestItems()
        {
            FileAssert.Exists(Utilities.GetTestFile("/Items.xml"));
            FileAssert.Exists(Utilities.GetTestFile("/UseItems.xml"));
            
            var items = GameData.LoadItems(Utilities.GetTestFile("/Items.xml"), Utilities.GetTestFile("/UseItems.xml"));
            
            //Challenge Coupon
            Assert.AreEqual(1593, items.FindIndex(item => item.Id == "i_n_00037"));
            
            var challengeCoupon = items[1593];
            StringAssert.AreEqualIgnoringCase("i_n_00037", challengeCoupon.Id);
            StringAssert.AreEqualIgnoringCase("ch_attack", challengeCoupon.Category);
            StringAssert.AreEqualIgnoringCase("Challenge Coupon", challengeCoupon.Name);
            //151,151,i_n_00037,ch_attack,Challenge Coupon,n/a,n/a,n/a,Allows you to issue a challenge for a channel battle.  The coupon will be redeemed when the battle is started and not at the channel entry. ,5000,250,n/a,n/a,n/a,n/a,True,False,False,
            //1593 -> ch_attack
        }
    }
}