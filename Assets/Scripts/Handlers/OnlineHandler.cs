using System.Collections;
using System.Collections.Generic;
using Globals;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Handlers
{
    public class OnlineHandler : MonoBehaviour
    {
        public static OnlineHandler reference;
        
        public GameObject MapIcon;
        
        public Transform mapIconsContainer;
        public Sprite[] spawnablesIcons;
        public Sprite[] redSpawnablesIcons;
        public List<MapIconHandler> mapIconHandlers = new List<MapIconHandler>(); 

        public static string selectedMap = "1561997251647-battlefield";
        
        private void Start()
        {
            reference = this;
            DownloadMapsList();
        }
        
        private void DownloadMapsList()
        {
            DatabaseHandler.DownloadMapsList(mapsList =>
            {
                foreach (var mapInfo in mapsList)
                {
                    var newIcon = Instantiate(MapIcon, transform.position, Quaternion.identity);
                    newIcon.transform.SetParent(mapIconsContainer);
                    var newIconHandler = newIcon.GetComponent<MapIconHandler>();
                    mapIconHandlers.Add(newIconHandler);
                    newIconHandler.mapId = mapInfo;
                    var iconId = long.Parse(mapInfo.Split('-')[0]) % spawnablesIcons.Length;
                    newIconHandler.iconId = iconId;
                    newIcon.GetComponent<Image>().sprite = spawnablesIcons[iconId];
                }
            });
        }

        public void StartSearch()
        {
            DatabaseHandler.EnterWaitingRoom(Global.PlayerId, selectedMap,
                response => { StartCoroutine(IsGameReady()); });
        }

        public void Back()
        {
            DatabaseHandler.ExitWaitingRoom(Global.PlayerId);
            SceneManager.LoadScene(0);
        }

        private IEnumerator IsGameReady()
        {
            yield return new WaitForSecondsRealtime(3);

            DatabaseHandler.IsGameReady(Global.PlayerId, (gameId, ready) =>
            {
                if (ready)
                {
                    DatabaseHandler.ExitWaitingRoom(Global.PlayerId);
                    
                    Global.GameId = gameId;

                    DatabaseHandler.DownloadMap(gameId, map =>
                    {
                        Global.SelectedMapJson = map;
                        DatabaseHandler.CheckTurn(gameId, playerTurnId =>
                        {
                            Global.IsOnlineMatch = true;
                            Global.IsPlayerTurn = playerTurnId == Global.PlayerId;
                            SceneManager.LoadScene(1);
                        });
                    });
                }
                else
                {
                    StartCoroutine(IsGameReady());
                }
            });
        }
    }
}