using System;
using UnityEngine;
using SharedClassLibrary.Simulation;

namespace GameClient
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance { get; private set; }

        public InputState inputState;
        public InputState InputState { get { return inputState; } }

        InputManager()
        {
            if (instance != null)
            {
                throw new Exception("Only one instance of InputManger can exist.");
            }
            instance = this;
        }

        [SerializeField]
        KeyCode up, down, left, right;

        void Update()
        {
            inputState.upKey = Input.GetKey(up);
            inputState.leftKey = Input.GetKey(left);
            inputState.rightKey = Input.GetKey(right);
            inputState.downKey = Input.GetKey(down);
        }
    }
}