namespace SharedClassLibrary.ECS
{
    abstract class EntityBuilder
    {
        EntityHandler CreateEntity(World _world)
        {
            EntityHandler entity = _world.CreateEntity();
            AddComponents(entity);
            return entity;
        }
        protected abstract void AddComponents(EntityHandler _entity);
    }
}
