using System.Collections.Generic;
using UnityEngine;
using SharedClassLibrary.Networking;

namespace GameClient
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField]
        KeyCode up, down, left, right;
        private Dictionary<ClientInputIds, KeyCode> keys;
        private void Awake()
        {
            keys = new Dictionary<ClientInputIds, KeyCode>()
        {
            { ClientInputIds.up, up },
            { ClientInputIds.down, down },
            { ClientInputIds.left, left },
            { ClientInputIds.right, right }
        };
        }

        void Update()
        {
            foreach (KeyValuePair<ClientInputIds, KeyCode> _key in keys)
            {
                if (Input.GetKeyDown(_key.Value))
                {
                    ClientSend.ButtonDown(_key.Key);
                }
                else if (Input.GetKeyUp(_key.Value))
                {
                    ClientSend.ButtonUp(_key.Key);
                }
            }
        }
    }
}