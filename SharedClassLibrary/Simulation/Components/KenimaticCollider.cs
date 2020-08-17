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

        private protected override void Dispose()
        {
            allKenimaticColliders.Remove(this);
            base.Dispose();
        }

        private protected override void Enable()
        {
            if (!allKenimaticColliders.Contains(this))
            {
                allKenimaticColliders.Add(this);
            }
            base.Enable();
        }

        private protected override void Disable()
        {
            if (allKenimaticColliders.Contains(this)) {
                allKenimaticColliders.Remove(this);
            }
            base.Disable();
        }
    }
}
