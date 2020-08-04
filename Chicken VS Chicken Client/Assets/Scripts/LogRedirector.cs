using UnityEngine;

namespace Assets.Scripts
{
    class LogRedirector : MonoBehaviour
    {
        private void Update()
        {
            while (SharedClassLibrary.Logging.Logger.DebugMessages.Count > 0)
            {
                Debug.Log(SharedClassLibrary.Logging.Logger.DebugMessages.Dequeue());
            }
            while (SharedClassLibrary.Logging.Logger.WarningMessages.Count > 0)
            {
                Debug.LogWarning(SharedClassLibrary.Logging.Logger.WarningMessages.Dequeue());
            }
        }
    }
}
