using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Extra
{
    public class JoinView : MonoBehaviour
    {
        public TMP_InputField codeInputField;
        public Button joinWithCodeButton;
        public JoinableMatchView joinableMatchViewPrefab;
        public GameObject matchContainer;
        
        private List<JoinableMatchView> joinableMatchViews = new List<JoinableMatchView>();
    }
}