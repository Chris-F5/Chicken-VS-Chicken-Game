using UnityEngine;
using TMPro;
using SharedClassLibrary;

namespace GameClient
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        [SerializeField]
        GameObject startMenu;
        [SerializeField]
        public TMP_InputField usernameField;

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
            NetworkManager.instance.ConnectToServer();
        }
    }
}
