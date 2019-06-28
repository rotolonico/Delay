using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Handlers
{
    public class MessageHandler : MonoBehaviour
    {
        public delegate void OnButtonClick();

        private static Canvas canvas;
        private static TextMeshProUGUI messageText;
        private static Button messageButton;

        private void Start()
        {
            canvas = GetComponent<Canvas>();
            var background = transform.GetChild(1);
            messageText = background.GetChild(0).GetComponent<TextMeshProUGUI>();
            messageButton = background.GetChild(1).GetComponent<Button>();
        }

        public static void ShowMessage(string message, OnButtonClick onClick)
        {
            canvas.enabled = true;
            messageText.text = message;
            messageButton.onClick.AddListener(() => { onClick(); });
        }

        public static void HideMessage()
        {
            canvas.enabled = false;
            messageButton.onClick.RemoveAllListeners();
        }
    }
}
