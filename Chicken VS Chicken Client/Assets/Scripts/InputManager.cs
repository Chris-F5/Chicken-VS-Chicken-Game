using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyButton
{
    up,
    down,
    left,
    right
}

public class InputManager : MonoBehaviour
{
    [SerializeField]
    KeyCode up, down, left, right;

    void Update()
    {
        if (Input.GetKeyDown(right))
        {
            ClientSend.ButtonDown(KeyButton.right);
        }
        if (Input.GetKeyDown(left))
        {
            ClientSend.ButtonDown(KeyButton.left);
        }
    }
}
