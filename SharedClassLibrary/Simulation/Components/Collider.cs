namespace SharedClassLibrary.Simulation.Components
{
    public abstract class Collider : Component
    {
        public Collider (Component _nextComponent) : base(_nextComponent) { }

        /// <returns>A vector that reprosents how this collider needs to be translated to exit the bounds of the other collider. Null if the colliders dont intersect.</returns>
        public abstract Vector2? CollideWith(Collider _collider);
    }
}
