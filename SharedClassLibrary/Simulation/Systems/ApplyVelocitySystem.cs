using SharedClassLibrary.Simulation.Components;

namespace SharedClassLibrary.Simulation.Systems
{
    class ApplyVelocitySystem : System
    {
        private ComponentManager<TransformComponent> transformManager;
        private ComponentManager<TransformComponent> velocityManager;

        public ApplyVelocitySystem(World _world)
        {
            ComponentMask componentMask = new ComponentMask(_world, 2);
            transformManager = componentMask.AddComponentType<TransformComponent>();
            velocityManager = componentMask.AddComponentType<TransformComponent>();
            SetComponentMask(componentMask);
        }
    }
}
