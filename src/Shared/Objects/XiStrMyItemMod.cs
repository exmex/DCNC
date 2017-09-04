namespace Shared.Objects
{
    public class XiStrMyItemMod
    {
        public InventoryItem MyInventoryItem;

        public int State;

        public XiStrMyItemMod()
        {
            MyInventoryItem = new InventoryItem();
        }

        /*struct XiStrMyItemMod
        {
            XiStrMyItem MyItem;
            int State;
        };*/
    }
}