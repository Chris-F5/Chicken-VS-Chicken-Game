using System;
using System.Collections.Generic;
using System.Text;

namespace SharedClassLibrary.Simulation
{
    internal class EntityHandler
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
            world.AttachComponent<ComponentType>(entity, _component);
        }
        public void RemoveComponent<ComponentType>() where ComponentType : struct
        {
            world.RemoveComponent<ComponentType>(entity);
        }
    }
}
