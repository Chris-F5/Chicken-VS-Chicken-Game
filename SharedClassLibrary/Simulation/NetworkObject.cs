using System;
using System.Collections.Generic;
using SharedClassLibrary.Logging;

namespace SharedClassLibrary.Simulation
{
    public sealed class NetworkObject : Component
    {
        private static List<NetworkObject> allNetworkObjects = new List<NetworkObject>();

        public readonly short objectId;
        public readonly short objectTypeId;

        private INetworkObjectHandler handler;

        internal NetworkObject(Component _firstComponent, INetworkObjectHandler _handler, short _objectTypeId) : base (_firstComponent)
        {
            if (_handler == null)
                throw new ArgumentNullException("_handler");

            handler = _handler;
            objectTypeId = _objectTypeId;

            // Sellect object id that is not in use.

            List<short> _takenObjectIds = new List<short>();

            foreach (NetworkObject _object in allNetworkObjects)
            {
                _takenObjectIds.Add(_object.objectId);
            }

            for (short i = 0; i <= short.MaxValue; i++)
            {
                if (!_takenObjectIds.Contains(i))
                {
                    objectId = i;
                    break;
                }
            }

            if (objectId >= short.MaxValue)
            {
                Logger.LogWarning($"Max NetworkSynchroniser count reached ({short.MaxValue}) reached - ignoring this NetworkSynchroniser");
                return;
            }

            allNetworkObjects.Add(this);
        }
        internal override void CallStartupHandlers()
        {
            handler.Created();
            base.CallStartupHandlers();
        }

        internal override void Dispose()
        {
            base.Dispose();
            allNetworkObjects.Remove(this);
        }

        public void Destroy()
        {
            handler.Destroyed();
            Dispose();
        }

        public static void UpdateAll()
        {
            foreach (NetworkObject _networkObject in allNetworkObjects)
            {
                _networkObject.Update();
            }
        }
    }

    public interface INetworkObjectHandler
    {
        void Created();
        void Destroyed();
    }
}
