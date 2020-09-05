using System;
using System.Collections.Generic;
using System.Text;

namespace SharedClassLibrary.Simulation
{
    struct WorldTickState
    {
        ComponentFamilyData[] componentData;

        public WorldTickState(ComponentManager[] _componentManagers)
        {
            componentData = new ComponentFamilyData[_componentManagers.Length];
            for (int i = 0; i < _componentManagers.Length; i++)
            {
                componentData[i] = _componentManagers[i].GetEmptyComponentFamilyData();
            }
        }

        public WorldTickState NextTickState()
        {
            WorldTickState newComponentData;
            newComponentData.componentData = new ComponentFamilyData[componentData.Length];
            for (int i = 0; i < componentData.Length; i++)
            {
                newComponentData.componentData[i] = componentData[i].Clone();
            }
            return newComponentData;
        }
        public interface ComponentFamilyData 
        {
            ComponentFamilyData Clone();
        }
        public struct ComponentFamilyData<ComponentType> : ComponentFamilyData where ComponentType : struct
        {
            public int setComponentCount;
            public ComponentType[] components;
            public Dictionary<Entity, int> componentIndexMap;
            public ComponentFamilyData(int _maxCount)
            {
                components = new ComponentType[_maxCount];
                setComponentCount = 0;
                componentIndexMap = new Dictionary<Entity, int>();
            }
            public ComponentFamilyData Clone()
            {
                ComponentFamilyData<ComponentType> newData;
                newData.setComponentCount = setComponentCount;
                newData.components = (ComponentType[])components.Clone();
                newData.componentIndexMap = new Dictionary<Entity, int>(componentIndexMap);
                return newData;
            }
        }
    }
}
