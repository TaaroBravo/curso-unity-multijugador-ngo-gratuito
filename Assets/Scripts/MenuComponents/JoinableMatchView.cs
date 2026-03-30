using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Extra
{
    public class JoinableMatchView : MonoBehaviour
    {
        public Button joinButton;
        public TextMeshPro matchNameText;
        public TextMeshPro playersText;
        public TextMeshPro pingText;
        
        private string _matchName;
        private int _currentPlayers;
        private int _maxPlayers;
        private int _ping;

        public void Init(string matchName, int currentPlayers, int maxPlayers, int ping, Action onJoinButton)
        {
            _matchName = matchName;
            _currentPlayers = currentPlayers;
            _maxPlayers = maxPlayers;
            _ping = ping;
            joinButton.onClick.AddListener(() => onJoinButton());
        }

        public void UpdateMatch()
        {
            matchNameText.text = _matchName;
            playersText.text = $"{_currentPlayers}/{_maxPlayers}";
            pingText.text = $"{_ping}";
            joinButton.interactable = _currentPlayers < _maxPlayers;
        }
    }
}