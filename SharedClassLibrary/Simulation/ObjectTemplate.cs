using SharedClassLibrary.Networking;

namespace SharedClassLibrary.Simulation
{
    abstract class ObjectTemplate
    {
        public readonly short typeId;
        public ObjectTemplate(NetworkObjectTemplateIds _typeId)
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
