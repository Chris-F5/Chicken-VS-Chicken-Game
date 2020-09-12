using System.Runtime.CompilerServices;

namespace SharedClassLibrary.ECS
{
    public class EntityHandler
    {
        public readonly Entity entity;
        private readonly World world;
        internal EntityHandler(World _world, Entity _entity)
        {
            world = _world;
            entity = _entity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddComponent<ComponentType>(ComponentType _component) where ComponentType : struct
        {
            world.AttachComponent(entity, _component);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveComponent<ComponentType>() where ComponentType : struct
        {
            world.RemoveComponent<ComponentType>(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ComponentType GetComponent<ComponentType>() where ComponentType : struct
        {
            return world.GetComponentManager<ComponentType>().GetComponent(entity);
        }
    }
}
