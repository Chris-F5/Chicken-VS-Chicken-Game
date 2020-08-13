using System;
using System.Collections.Generic;

namespace SharedClassLibrary.Simulation
{
    public abstract class Component
    {
        public readonly byte indexId;

        protected readonly Component nextComponent;

        public Component(Component _nextComponent)
        {
            nextComponent = _nextComponent;

            if (nextComponent == null)
            {
                indexId = 0;
            }
            else
            {
                if (nextComponent.indexId + 1 > byte.MaxValue)
                {
                    throw new Exception($"Too many components added to one component chain (> {short.MaxValue})");
                }
                indexId = (byte)(nextComponent.indexId + 1);
            }

            CallStartupHandlers();
        }

        internal ComponentType GetComponent<ComponentType>() where ComponentType : Component
        {
            if (this.GetType() == typeof(ComponentType) || this.GetType().IsSubclassOf(typeof(ComponentType)))
            {
                return this as ComponentType;
            }
            else if (nextComponent == null)
            {
                return null;
            }
            else
            {
                return nextComponent.GetComponent<ComponentType>();
            }
        }

        internal List<ComponentType> GetComponents<ComponentType>() where ComponentType : Component
        {
            if (this.GetType() == typeof(ComponentType) || this.GetType().IsSubclassOf(typeof(ComponentType)))
            {
                if (nextComponent == null)
                {
                    return new List<ComponentType>() { this as ComponentType };
                }
                else
                {
                    List<ComponentType> currentComponents = nextComponent.GetComponents<ComponentType>();
                    currentComponents.Add(this as ComponentType);
                    return currentComponents;
                }
            }
            else
            {
                if (nextComponent == null)
                {
                    return new List<ComponentType>();
                }
                else
                {
                    return nextComponent.GetComponents<ComponentType>();
                }
            }
        }

        internal virtual void Update() 
        {
            if (nextComponent != null) {
                nextComponent.Update();
            }
        }

        internal virtual void Dispose() 
        {
            if (nextComponent != null)
            {
                nextComponent.Dispose();
            }
        }

        internal virtual void CallStartupHandlers()
        {
            if (nextComponent != null)
            {
                nextComponent.CallStartupHandlers();
            }
        }
    }
}
