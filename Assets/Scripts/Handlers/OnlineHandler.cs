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
            DatabaseHandler.EnterWaitingRoom(Global.PlayerId, "battlefield",
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