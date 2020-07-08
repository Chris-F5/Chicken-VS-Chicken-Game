using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.NetworkSynchronisers
{
    class Item : RectObject
    {
        private static List<Item> allItems = new List<Item>();
        public Item(SynchroniserType _synchroniserType, Rect _rect, bool _constantSize = true) : base(_synchroniserType, _rect, _constantSize)
        {
            allItems.Add(this);
        }

        public override void Destroy()
        {
            allItems.Remove(this);
            base.Destroy();
        }
    }
}
