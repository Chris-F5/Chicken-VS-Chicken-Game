using SharedClassLibrary.Networking;

namespace SharedClassLibrary.Simulation
{
    public abstract class ObjectTemplate
    {
        public readonly short typeId;
        protected ObjectTemplate(NetworkObjectTemplateIds _typeId)
        {
            typeId = (short)_typeId;
        }
        public abstract void AddComponentsToArray(NetworkObject _objectReference, ref Component[] _componentArray);
        public NetworkObject CreateObjectInstance()
        {
            return new NetworkObject(this);
        }
    }
}
