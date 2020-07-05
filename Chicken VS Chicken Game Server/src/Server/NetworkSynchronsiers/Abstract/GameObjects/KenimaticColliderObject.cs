using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.NetworkSynchronisers
{
    abstract class KenimaticColliderObject : RectObject
    {
        public static List<KenimaticColliderObject> allKenimaticColliders = new List<KenimaticColliderObject>();
        public KenimaticColliderObject(SynchroniserType _synchroniseType, Rect _collider) : base(_synchroniseType, _collider)
        {
            allKenimaticColliders.Add(this);
        }

        public override void Update(){}

        public override void Destroy()
        {
            allKenimaticColliders.Remove(this);
            base.Destroy();
        }
    }
}
