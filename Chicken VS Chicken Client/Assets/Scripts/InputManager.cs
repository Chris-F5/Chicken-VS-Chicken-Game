using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameClient
{
    public enum KeyButton
    {
        up = 1,
        down,
        left,
        right
    }

    public class InputManager : MonoBehaviour
    {
        [SerializeField]
        KeyCode up, down, left, right;
        private Dictionary<KeyButton, KeyCode> keys;
        private void Awake()
        {
            keys = new Dictionary<KeyButton, KeyCode>()
        {
            { KeyButton.up, up },
            { KeyButton.down, down },
            { KeyButton.left, left },
            { KeyButton.right, right }
        };
        }

        void Update()
        {
            foreach (KeyValuePair<KeyButton, KeyCode> _key in keys)
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