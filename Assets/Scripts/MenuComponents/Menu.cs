using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Extra
{
    public class Menu : MonoBehaviour
    {
        [Header("Buttons")] 
        public Button joinButton;
        public Button hostButton;
        public Button quit;

        private void Start()
        {
            joinButton.onClick.AddListener(Join);
            hostButton.onClick.AddListener(Host);
            quit.onClick.AddListener(Application.Quit);
        }

        private void Join()
        {
            NetworkManager.Singleton.StartClient();
        }

        private void Host()
        {
            NetworkManager.Singleton.StartHost();
            NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
            
        }
    }
}
