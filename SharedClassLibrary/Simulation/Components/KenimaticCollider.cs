using System.Collections.Generic;

namespace SharedClassLibrary.Simulation.Components
{
    public class KenimaticCollider : Component
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
