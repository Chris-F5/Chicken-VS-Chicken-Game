using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance { get; private set; }
    public static Dictionary<short, NetworkObject> allNetworkObjects = new Dictionary<short, NetworkObject>();

    [SerializeField]
    private GameObject[] objectPrefabs;

    private void Awake()
    {
        if (instance == null )
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.LogWarning("ObjectManager instance already exists, destroying object!");
            Destroy(this);
        }
    }
    public void NewGameObject(Packet _packet)
    {
        byte _typeId = _packet.ReadByte();
        short _objectId = _packet.ReadShort();
        if (!allNetworkObjects.ContainsKey(_objectId)) {
            GameObject newObjectPrefab;
            newObjectPrefab = objectPrefabs[_typeId];

            GameObject newGameObject = Instantiate(newObjectPrefab);
            NetworkObject newNetworkObject = newGameObject.GetComponent<NetworkObject>();

            newNetworkObject.HandleNewObjectData(_packet);

            allNetworkObjects.Add(_objectId, newNetworkObject);
        }
    }
    public void HandleObjectUpdate(short _objectId, Packet _packet)
    {
        allNetworkObjects[_objectId].HandleObjectUpdate(_packet);
    }
}
