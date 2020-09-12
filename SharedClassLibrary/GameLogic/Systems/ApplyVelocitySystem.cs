using SharedClassLibrary.GameLogic.Components;
using SharedClassLibrary.ECS;

namespace SharedClassLibrary.GameLogic.Systems
{
    class ApplyVelocitySystem : GameSystem
    {
        private ComponentManager<TransformComponent> transformManager;
        private ComponentManager<VelocityComponent> velocityManager;

        public ApplyVelocitySystem(World _world)
            : base(new ComponentMask(_world, typeof(TransformComponent), typeof(VelocityComponent)))
        {
            ComponentMask.GetComponentManager(out transformManager);
            ComponentMask.GetComponentManager(out velocityManager);
        }
        public override void Update()
        {
            foreach (Entity entity in activeEntities)
            {
                ref TransformComponent transform = ref transformManager.GetComponent(entity);
                ref VelocityComponent velocity = ref velocityManager.GetComponent(entity);
                transform.position += velocity.velocity;
            }
        }
    }
}
