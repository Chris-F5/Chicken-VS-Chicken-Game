using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameClient.NetworkSynchronisers;

namespace GameClient
{
    public class SynchroniserManager : MonoBehaviour
    {
        public static SynchroniserManager instance { get; private set; }


        [SerializeField]
        private UnityEngine.GameObject[] synchroniserPrefabs;

        private static Dictionary<short, NetworkSynchroniser> allSynchronisers = new Dictionary<short, NetworkSynchroniser>();

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
        public static NetworkSynchroniser FindSynchroniser(short _id)
        {
            if (allSynchronisers.ContainsKey(_id))
            {
                return allSynchronisers[_id];
            }
            else
            {
                return null;
            }
        }

        public void HandleNewSynchroniser(short _synchroniserId, short _typeId, Packet _packet)
        {
            if (_typeId < synchroniserPrefabs.Length)
            {
                short _firstEvent = _packet.ReadByte(false);
                if (_firstEvent != EventIds.startupEvents)
                {
                    // TODO: ask server for the startup infomation every set interval of frames until its retrieved.
                    Debug.Log("Object created without startup event. A packet must have been lost.");
                }

                UnityEngine.GameObject _synchroniserGameObject = Instantiate(synchroniserPrefabs[_typeId]);
                _synchroniserGameObject.GetComponent<NetworkSynchroniser>().HandleSynchronise(_packet);
                allSynchronisers.Add(_synchroniserId, _synchroniserGameObject.GetComponent<NetworkSynchroniser>());
            }
            else
            {
                Debug.LogWarning("Synchroniser type ID given by server does not have corresponding prefab.");
            }
        }
    }
}
