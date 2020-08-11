using System;

namespace SharedClassLibrary.Simulation
{
    public abstract class Component
    {
        protected readonly NetworkObject networkObject;

        public Component(NetworkObject _networkObject)
        {
            if (_networkObject == null)
                throw new ArgumentNullException("_networkObject is null.");

            networkObject = _networkObject;
            CallStartupHandlers();
        }
        public virtual void Update() { }

        public virtual void Dispose()
        {
        }

        public virtual void CallStartupHandlers() {}
    }
}
