using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.GameObjects
{
    abstract class KenimaticColliderObject : RectObject
    {
        public static List<KenimaticColliderObject> allKenimaticColliders = new List<KenimaticColliderObject>();
        public KenimaticColliderObject(byte _typId, Rect _collider) : base(_typId, _collider)
        {
            allKenimaticColliders.Add(this);
        }

        public override void Destroy()
        {
            allKenimaticColliders.Remove(this);
            base.Destroy();
        }
    }
}
