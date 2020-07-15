﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    enum NetworkObjectType
    {
        player,
        wall,
        testObject
    }
    sealed class NetworkObject
    {
        private static List<NetworkObject> allNetworkObjects = new List<NetworkObject>();

        public readonly short objectId;
        public readonly short objectTypeId;

        private readonly Component[] components;

        public NetworkObject(ObjectTemplate _template)
        {
            objectTypeId = _template.typeId;
            components = _template.GenerateCompoentSet(this);
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

        public void AddComponentEventsToPacket(Packet _packet)
        {
            for (byte i = 0; i < components.Length; i++)
            {
                if (components[i].pegingEventCount != 0) {
                    _packet.WriteByte(i);
                    components[i].AddEventsToPacket(_packet);
                }
            }
            // 0 is the end events constant id.
            _packet.WriteShort(0);
        }

        public void AddStartupEventsToPacket(Packet _packet)
        {
            for (byte i = 0; i < components.Length; i++)
            {
                if (components[i].pegingEventCount != 0)
                {
                    _packet.WriteByte(i);
                    components[i].AddStartupEventsToPacket(_packet);
                }
            }
            // 0 is the end events constant id.
            _packet.WriteShort(0);
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
                if (_component.GetType() == typeof(ComponentType))
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
                if (_component.GetType() == typeof(ComponentType))
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
            Packet _packet = new Packet(ServerPackets.synchronise);
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
            Packet _packet = new Packet(ServerPackets.synchronise);
            for (int i = 0; i < allNetworkObjects.Count; i++)
            {
                _packet.WriteShort(allNetworkObjects[i].objectId);
                _packet.WriteShort(allNetworkObjects[i].objectTypeId);
                allNetworkObjects[i].AddStartupEventsToPacket(_packet);
            }
            return _packet;
        }
    }
}
