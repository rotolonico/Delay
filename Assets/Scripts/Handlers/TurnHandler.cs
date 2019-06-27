using Globals;
using Serializables;
using UnityEngine;

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
            turn = turn == 1 ? 2 : 1;
            if (!Global.IsOnlineMatch) return;
            Global.IsPlayerTurn = !Global.IsPlayerTurn;
            if (!changeTurnInDatabase) return;
            
            DatabaseHandler.UploadMove(Global.GameId, move, () =>
            {
                DatabaseHandler.ChangeTurn(Global.GameId, Global.PlayerId, () =>
                {
                    StartCoroutine(GameHandler.reference.CheckTurn()); 
                }); 
            });

        }
    }
}
