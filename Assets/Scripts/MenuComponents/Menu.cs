using UnityEngine;
using UnityEngine.UI;

namespace Extra
{
    public class Menu : MonoBehaviour
    {
        [Header("Buttons")] public Button quickPlayButton;
        public Button joinButton;
        public Button hostButton;
        public Button quit;

        [Header("Views")] public HostView hostView;
        public JoinView joinView;
        public GameObject loading;

        private void Start()
        {
            quickPlayButton.onClick.AddListener(SearchQuickPlay);
            joinButton.onClick.AddListener(Join);
            hostButton.onClick.AddListener(Host);
            quit.onClick.AddListener(Application.Quit);
        }

        private void SearchQuickPlay()
        {

        }

        private void Join()
        {

        }

        private void Host()
        {

        }
    }
}
