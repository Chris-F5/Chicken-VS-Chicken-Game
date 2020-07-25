using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SharedGameLogic;

namespace GameClient
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        [SerializeField]
        GameObject startMenu;
        [SerializeField]
        public TMP_InputField usernameField;
        [SerializeField]
        private Client client;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Debug.LogWarning("UIManager instance already exists, destroying object!");
                Destroy(this);
            }
        }

        public void ConnectBtnClicked()
        {
            startMenu.SetActive(false);
            usernameField.interactable = false;
            client.ConnectToServer();
        }
    }
}
