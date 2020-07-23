using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    class KenimaticCollider : Component
    {
        public static List<NetworkObject> allKenimaticColliders = new List<NetworkObject>();
        public readonly Collider[] colliders;

        public KenimaticCollider(NetworkObject _networkObject) : base(_networkObject)
        {
            allKenimaticColliders.Add(networkObject);

            colliders = networkObject.GetComponents<Collider>();
        }

        public override void Dispose()
        {
            allKenimaticColliders.Remove(networkObject);
            base.Dispose();
        }
    }
}
