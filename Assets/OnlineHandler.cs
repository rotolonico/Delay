using System;
using System.Collections;
using System.Collections.Generic;
using Globals;
using Handlers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnlineHandler : MonoBehaviour
{
    private string playerId;
    
    private void Start()
    {
        GeneratePlayerId();
        DatabaseHandler.EnterWaitingRoom(playerId, "battlefield", response => { StartCoroutine(IsGameReady()); });
    }

    private void GeneratePlayerId()
    {
        playerId = BasicFunctions.GetCurrentTimestamp().ToString();
    }

    private IEnumerator IsGameReady()
    {
        yield return new WaitForSecondsRealtime(5);
        
        DatabaseHandler.IsGameReady(playerId, (gameId, ready) =>
        {
            if (ready)
            {
                DatabaseHandler.GetMapToDownload(gameId, mapName =>
                {
                    DatabaseHandler.DownloadMap("battlefield", map =>
                    {
                        Global.selectedMapJson = map;
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
