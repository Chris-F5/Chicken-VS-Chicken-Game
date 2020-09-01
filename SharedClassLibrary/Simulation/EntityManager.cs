using System;
using System.Collections.Generic;

namespace SharedClassLibrary.Simulation
{
    class EntityManager
    {
        private List<Entity> entities = new List<Entity>();

        internal Entity CreateEntity()
        {
            for (uint id = 0; id < uint.MaxValue; id++)
            {
                bool idTaken = false;
                foreach (Entity entity in entities)
                {
                    if (entity.id == id)
                    {
                        idTaken = true;
                        break;
                    }
                }

                if (!idTaken)
                {
                    Entity newEntity = new Entity(id);
                    entities.Add(newEntity);
                    return newEntity;
                }
            }

            throw new Exception($"All {uint.MaxValue} entity ids are taken. Cant create new entity.");
        }

        internal void RemoveEntity(Entity _entity)
        {
            entities.Remove(_entity);
        }
    }
}
