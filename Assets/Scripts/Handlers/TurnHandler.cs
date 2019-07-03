using System.Collections;
using System.Linq;
using Globals;
using Serializables;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Handlers
{
    public class TurnHandler : MonoBehaviour
    {
        public int turn;

        private void Start()
        {
            turn = 1;
        }

        public void ChangeTurn(bool changeTurnInDatabase = false, Move move = null)
        {
            StartCoroutine(ChangeTurnCoroutine(changeTurnInDatabase, move));
        }

        private IEnumerator ChangeTurnCoroutine(bool changeTurnInDatabase = false, Move move = null)
        {
            yield return null;

            if (!CheckGame(changeTurnInDatabase, move)) yield break;

            Global.TimeLeft = Global.MaxTime;
            Global.IsPlayerTurn = !Global.IsPlayerTurn;
            GameHandler.reference.timerImage.color = turn == 1 ? Global.darkRed : Global.darkBlue;
            GameHandler.reference.timerForegroundImage.color = turn == 1 ? Global.lightRed : Global.lightBlue;
            turn = turn == 1 ? 2 : 1;
            if (!Global.IsOnlineMatch) yield break;
            if (!changeTurnInDatabase) yield break;

            DatabaseHandler.UploadMove(Global.GameId, move,
                () =>
                {
                    DatabaseHandler.ChangeTurn(Global.GameId, Global.PlayerId,
                        () => { StartCoroutine(GameHandler.reference.CheckTurn()); });
                });
        }

        private bool CheckGame(bool changeTurnInDatabase, Move move)
        {
            var blueKing = false;
            var redKing = false;

            var Kings = GameObject.FindGameObjectsWithTag("King");
            if (Kings.Any(king => king.GetComponent<PawnHandler>().team == 1))
            {
                blueKing = true;
            }

            if (Kings.Any(king => king.GetComponent<PawnHandler>().team == 2))
            {
                redKing = true;
            }

            if (blueKing && redKing) return true;

            string message;

            if (redKing || blueKing)
            {
                message = Global.IsOnlineMatch ? changeTurnInDatabase ? turn == 1 ? blueKing
                        ? "Congratulations, You won the match!"
                        : "You lost, better luck next time!" :
                    blueKing ? "You lost, better luck next time!" : "Congratulations, You won the match!" :
                    turn == 1 ? blueKing ? "You lost, better luck next time!" :
                    "Congratulations, You won the match!" :
                    blueKing ? "Congratulations, You won the match!" : "You lost, better luck next time!" :
                    blueKing ? "Blue won the match!" : "Red won the match!";
            }
            else
            {
                message = "It's a draw!";
            }

            MessageHandler.ShowMessage(message, () =>
            {
                SceneManager.LoadScene(0);
                MessageHandler.HideMessage();
            }, () =>
            {
                SceneManager.LoadScene(0);
                MessageHandler.HideMessage();
            });

            if (!changeTurnInDatabase) return false;
            DatabaseHandler.UploadMove(Global.GameId, move,
                () =>
                {
                    DatabaseHandler.ChangeTurn(Global.GameId, Global.PlayerId,
                        () => { StartCoroutine(GameHandler.reference.CheckTurn()); });
                });

            return false;
        }
    }
}