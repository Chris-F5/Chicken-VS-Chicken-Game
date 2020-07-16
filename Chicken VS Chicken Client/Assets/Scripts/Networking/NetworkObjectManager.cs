using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameClient.NetworkSynchronisers;

namespace GameClient
{
    public class NetworkObjectManager : MonoBehaviour
    {
        public static NetworkObjectManager instance { get; private set; }


        [SerializeField]
        private UnityEngine.GameObject[] networkObjectPrefabs;

        private static Dictionary<short, NetworkObject> allNetworkObjects = new Dictionary<short, NetworkObject>();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Debug.LogWarning("ObjectManager instance already exists, destroying object!");
                Destroy(this);
            }
        }
        public static NetworkObject FindObject(short _id)
        {
            if (allNetworkObjects.ContainsKey(_id))
            {
                return allNetworkObjects[_id];
            }
            else
            {
                return null;
            }
        }

        public void HandleNewSynchroniser(short _synchroniserId, short _typeId, Packet _packet)
        {
            if (_typeId < networkObjectPrefabs.Length && networkObjectPrefabs[_typeId] != null)
            {
                short _firstEvent = _packet.ReadByte(false);
                if (_firstEvent != EventIds.startupEvents)
                {
                    // TODO: ask server for the startup infomation every set interval of frames until its retrieved.
                    Debug.Log("Object created without startup event. A packet must have been lost.");
                }

                GameObject _newGameObject = Instantiate(networkObjectPrefabs[_typeId]);
                _newGameObject.GetComponent<NetworkObject>().HandleSynchronise(_packet);
                allNetworkObjects.Add(_synchroniserId, _newGameObject.GetComponent<NetworkObject>());
            }
            else
            {
                Debug.LogWarning("Network object type ID given by server does not have corresponding prefab.");
            }
        }
    }
}
