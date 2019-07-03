using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Handlers
{
    public class MessageHandler : MonoBehaviour
    {
        public delegate void OnButtonClick();

        public static Canvas canvas;
        private static TextMeshProUGUI messageText;
        private static Button messageButton;
        private static Button exitButton;

        private void Start()
        {
            canvas = GetComponent<Canvas>();
            var background = transform.GetChild(0);
            messageText = background.GetChild(0).GetComponent<TextMeshProUGUI>();
            messageButton = background.GetChild(1).GetComponent<Button>();
            exitButton = background.GetChild(2).GetComponent<Button>();
            exitButton.onClick.AddListener(HideMessage);
        }

        public static void ShowMessage(string message, OnButtonClick onClick, OnButtonClick onExitClick)
        {
            canvas.enabled = true;
            messageText.text = message;
            messageButton.onClick.AddListener(() => { onClick(); });
            exitButton.onClick.AddListener(() => { onExitClick(); });
        }

        public static void HideMessage()
        {
            canvas.enabled = false;
            messageButton.onClick.RemoveAllListeners();
        }
    }
}
