using SharedClassLibrary.Simulation.Components;

namespace SharedClassLibrary.Simulation.Systems
{
    class ApplyVelocitySystem : System
    {
        private ComponentManager<TransformComponent> transformManager;
        private ComponentManager<TransformComponent> velocityManager;

        public ApplyVelocitySystem(World _world)
            : base(new ComponentMask(_world, typeof(TransformComponent), typeof(VelocityComponent)))
        {
        }
        public override void Update()
        {

        }
    }
}
