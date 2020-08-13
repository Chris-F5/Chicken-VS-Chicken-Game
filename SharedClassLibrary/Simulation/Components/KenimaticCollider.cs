using System.Collections.Generic;

namespace SharedClassLibrary.Simulation.Components
{
    public class KenimaticCollider : Component
    {
        public static List<KenimaticCollider> allKenimaticColliders = new List<KenimaticCollider>();
        public readonly Collider[] colliders;

        public KenimaticCollider(Component _nextComponent) : base(_nextComponent)
        {
            allKenimaticColliders.Add(this);

            colliders = GetComponents<Collider>().ToArray();
        }

        internal override void Dispose()
        {
            allKenimaticColliders.Remove(this);
            base.Dispose();
        }
    }
}
