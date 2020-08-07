﻿using System;
using System.Collections.Generic;
using SharedClassLibrary.Networking;

namespace SharedClassLibrary.Simulation
{
    public sealed class NetworkObject
    {
        private static List<NetworkObject> allNetworkObjects = new List<NetworkObject>();

        public readonly short objectId;
        public readonly short objectTypeId;

        private readonly Component[] components;

        internal NetworkObject(ObjectTemplate _template)
        {
            objectTypeId = _template.typeId;
            _template.AddComponentsToArray(this, ref components);

            // Shift component array allong one space and add the network object component.
            Component[] _componentsWithObjectComponent = new Component[components.Length + 1];
            Array.Copy(components, 0, _componentsWithObjectComponent,  1, components.Length);
            _componentsWithObjectComponent[0] = new ObjectComponent(this);
            components = _componentsWithObjectComponent;

            if (components.Length >= 255)
            {
                throw new Exception("Too many components attached to network object.");
            }

            if (components == null)
                throw new Exception($"InitComponents returned null on object type {objectTypeId}.");

            allNetworkObjects.Add(this);

            if (components.Length > byte.MaxValue)
            {
                throw new Exception("Too many components attached to a single object.");
            }

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
            if (objectId == short.MaxValue)
            {
                Console.WriteLine($"CRITICAL WARNING : max NetworkSynchroniser count reached ({short.MaxValue}) reached - ignoring this NetworkSynchroniser");
                return;
            }
        }

        internal void AddComponentEventsToPacket(Packet _packet)
        {
            for (byte i = 0; i < components.Length; i++)
            {
                if (components[i].pendingEventCount != 0) {
                    _packet.WriteByte(i);
                    components[i].AddEventsToPacket(_packet);
                    _packet.WriteByte(EventIds.EventEnd);
                }
            }
            _packet.WriteByte(255);
        }

        public void AddStartupEventsToPacket(Packet _packet)
        {
            for (byte i = 0; i < components.Length; i++)
            {
                _packet.WriteByte(i);
                components[i].AddStartupEventsToPacket(_packet);
                _packet.WriteByte(EventIds.EventEnd);
            }
            _packet.WriteByte(255);
        }

        private void UpdateComponents()
        {
            foreach (Component _component in components)
            {
                _component.Update();
            }
        }

        public ComponentType GetComponent<ComponentType>() where ComponentType : Component
        {
            foreach (Component _component in components)
            {
                if (_component == null)
                {
                    break;
                }
                if (_component.GetType() == typeof(ComponentType) || _component.GetType().IsSubclassOf(typeof(ComponentType)))
                {
                    return _component as ComponentType;
                }
            }
            throw new Exception($"Object {this} does not have attached component of type {typeof(ComponentType)}.");
        }

        public ComponentType[] GetComponents<ComponentType>() where ComponentType : Component
        {
            List<ComponentType> _attachedComponentsOfTypeList = new List<ComponentType>();
            foreach (Component _component in components)
            {
                if (_component == null)
                {
                    break;
                }
                if (_component.GetType() == typeof(ComponentType) || _component.GetType().IsSubclassOf(typeof(ComponentType)))
                {
                    _attachedComponentsOfTypeList.Add(_component as ComponentType);
                }
            }
            return _attachedComponentsOfTypeList.ToArray();
            throw new Exception($"Object {this} does not have attached component of type {typeof(ComponentType)}.");
        }

        public void Destroy()
        {
            allNetworkObjects.Remove(this);

            foreach (Component _component in components)
            {
                _component.Dispose();
            }

            // TODO: Destroy the object.
        }

        public static void UpdateAll()
        {
            foreach (NetworkObject _networkObject in allNetworkObjects)
            {
                _networkObject.UpdateComponents();
            }
        }

        public static Packet GenerateSynchronisationPacket()
        {
            Packet _packet = new Packet((byte)ServerPacketIds.synchronise);
            for (int i = 0; i < allNetworkObjects.Count; i++)
            {
                _packet.WriteShort(allNetworkObjects[i].objectId);
                _packet.WriteShort(allNetworkObjects[i].objectTypeId);
                allNetworkObjects[i].AddComponentEventsToPacket(_packet);
            }
            return _packet;
        }
        public static Packet GenerateStartupPacket()
        {
            Packet _packet = new Packet((byte)ServerPacketIds.synchronise);
            for (int i = 0; i < allNetworkObjects.Count; i++)
            {
                _packet.WriteShort(allNetworkObjects[i].objectId);
                _packet.WriteShort(allNetworkObjects[i].objectTypeId);
                allNetworkObjects[i].AddStartupEventsToPacket(_packet);
            }
            return _packet;
        }

        private sealed class ObjectComponent : Component
        {
            public ObjectComponent(NetworkObject _networkObject) : base(_networkObject) { }
            public override void AddStartupEventsToPacket(Packet _packet)
            {
                base.AddStartupEventsToPacket(_packet);
                new ObjectCreatedEvent().AddEventToPacket(_packet);
            }

            // If this event is sent, it means the startup events for components in this netowrk object are present in the packet.
            private class ObjectCreatedEvent : Event
            {
                public ObjectCreatedEvent() : base(EventIds.ObjectCreated) { }
            }
        }
    }
}