using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        foreach (KeyValuePair<KeyButton, KeyCode> key in keys)
        {
            if (Input.GetKeyDown(key.Value))
            {
                ClientSend.ButtonDown(key.Key);
            }
            else if(Input.GetKeyUp(key.Value))
            {
                ClientSend.ButtonUp(key.Key);
            }
        }
    }
}
