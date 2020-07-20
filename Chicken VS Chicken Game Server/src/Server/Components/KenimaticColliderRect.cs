using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    class KenimaticColliderRect : RectCollider
    {
        public static List<KenimaticColliderRect> allKenimaticColliderRects = new List<KenimaticColliderRect>();
        public KenimaticColliderRect(NetworkObject _networkObject, Rect _rect) : base(_networkObject, _rect)
        {
            allKenimaticColliderRects.Add(this);
        }

        public override void Dispose()
        {
            allKenimaticColliderRects.Remove(this);
            base.Dispose();
        }
    }
}
