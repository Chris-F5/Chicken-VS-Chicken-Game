using SharedClassLibrary.Simulation.Components;

namespace SharedClassLibrary.Simulation.Systems
{
    class ApplyVelocitySystem : System
    {
        private ComponentManager<TransformComponent> transformManager;
        private ComponentManager<VelocityComponent> velocityManager;

        public ApplyVelocitySystem(World _world)
            : base(new ComponentMask(_world, typeof(TransformComponent), typeof(VelocityComponent)))
        {
        }
        public override void Update()
        {
            foreach (Entity entity in activeEntities)
            {
                TransformComponent transform = transformManager[entity];
                VelocityComponent velocity = velocityManager[entity];
                transform.position += velocity.velocity;
                transformManager[entity] = transform;
            }
        }
    }
}
