using System;
using Globals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Handlers
{
    public class MapIconHandler : MonoBehaviour
    {
        public string mapId;

        private TextMeshProUGUI tmp;
        private Image img;
        public long iconId;

        private void Start()
        {
            tmp = GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = mapId.Split('-')[1];
            img = GetComponent<Image>();
        }

        public void OnClick()
        {
            OnlineHandler.selectedMap = mapId;
            
            foreach (var mapIconHandler in OnlineHandler.reference.mapIconHandlers)
            {
                mapIconHandler.Deselect();
            }
            
            tmp.color = Global.lightRed;
            img.sprite = OnlineHandler.reference.redSpawnablesIcons[iconId];
        }

        private void Deselect()
        {
            tmp.color = Global.lightBlue;
            img.sprite = OnlineHandler.reference.spawnablesIcons[iconId];
        }
    }
}
