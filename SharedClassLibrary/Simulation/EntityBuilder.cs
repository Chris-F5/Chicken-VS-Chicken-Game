﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SharedClassLibrary.Simulation
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
