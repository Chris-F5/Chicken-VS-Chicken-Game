namespace SharedClassLibrary.Simulation
{
    public class EntityHandler
    {
        private readonly Entity entity;
        private readonly World world;
        internal EntityHandler(World _world, Entity _entity)
        {
            world = _world;
            entity = _entity;
        }
        public void AddComponent<ComponentType>(ComponentType _component) where ComponentType : struct
        {
            world.AttachComponent(entity, _component);
        }
        public void RemoveComponent<ComponentType>() where ComponentType : struct
        {
            world.RemoveComponent<ComponentType>(entity);
        }
    }
}
