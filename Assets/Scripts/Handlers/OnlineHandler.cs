using System.Collections;
using Globals;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Handlers
{
    public class OnlineHandler : MonoBehaviour
    {
        private void Start()
        {
            GeneratePlayerId();
            DatabaseHandler.EnterWaitingRoom(Global.PlayerId, "battlefield",
                response => { StartCoroutine(IsGameReady()); });
        }

        private void GeneratePlayerId()
        {
            Global.PlayerId = BasicFunctions.GetCurrentTimestamp().ToString();
        }

        private IEnumerator IsGameReady()
        {
            yield return new WaitForSecondsRealtime(3);

            DatabaseHandler.IsGameReady(Global.PlayerId, (gameId, ready) =>
            {
                if (ready)
                {
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