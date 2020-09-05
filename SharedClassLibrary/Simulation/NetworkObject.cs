using System;
using System.Collections.Generic;
using SharedClassLibrary.Logging;
using SharedClassLibrary.Simulation.Components;

namespace SharedClassLibrary.Simulation
{
    public class NetworkObject : RollbackComponent
    {
        private static List<NetworkObject> pendingCreationObject = new List<NetworkObject>();
        private static List<NetworkObject> allNetworkObjects = new List<NetworkObject>();

        public readonly short objectId;
        public readonly short objectTypeId;

        internal NetworkObject(Component _firstComponent, short _objectTypeId) 
            : base (_firstComponent, new NetworkObjectState())
        {
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

            AddObject(this);
        }

        internal void EnableObject()
        {
            (state as NetworkObjectState).Enabled = true;
            Enable();
        }
        internal void DisableObject()
        {
            (state as NetworkObjectState).Enabled = false;
            Disable();
        }

        private protected override void RollbackOneTickPastCreation()
        {
            if (enabled)
            {
                DisableObject();
            }
        }

        private protected override void RollforwardPastCreation()
        {
            EnableObject();
        }

        private protected override void Dispose()
        {
            base.Dispose();
            allNetworkObjects.Remove(this);
        }

        internal void Destroy()
        {
            DisableObject();
            (state as NetworkObjectState).destroyedTime = GameLogic.Instance.GameTick;
        }

        private void RollBackComponentTicks(int _ticks)
        {
            for (int i = 0; i < _ticks; i++) {
                RollBackOneTick();
            }
        }
        protected internal override void Update()
        {
            if ((state as NetworkObjectState).destroyedTime != -1 && (state as NetworkObjectState).destroyedTime < GameLogic.Instance.rollbackLimit)
            {
                Dispose();
            }
            base.Update();
        }

        static internal void RollBackTicks(int _ticks)
        {
            foreach (NetworkObject networkObject in allNetworkObjects)
            {
                networkObject.RollBackComponentTicks(_ticks);
            }
        }

        internal static void UpdateAll()
        {
            foreach (NetworkObject _networkObject in allNetworkObjects)
            {
                _networkObject.Update();
            }
            foreach (NetworkObject _networkObject in allNetworkObjects)
            {
                _networkObject.LateUpdate();
            }
            allNetworkObjects.AddRange(pendingCreationObject);
            pendingCreationObject.Clear();
        }

        private static void AddObject(NetworkObject _object)
        {
            if (_object == null)
                throw new ArgumentNullException("_object");
            pendingCreationObject.Add(_object);
        }
    }

    internal class NetworkObjectState : ComponentRollbackState
    {
        internal NetworkObjectState(bool _enabled = true)
        {
            enabled = _enabled;
        }
        public int destroyedTime = -1;
        private bool enabled;
        public bool Enabled 
        {
            get 
            {
                return (activeState as NetworkObjectState).enabled;
            }
            set
            {
                ChangeMade();
                (activeState as NetworkObjectState).enabled = value;
            }
        }

        protected override ComponentRollbackState Clone()
        {
            return new NetworkObjectState(enabled);
        }
    }
}
