using Globals;
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

        public void ChangeTurn()
        {
            turn = turn == 1 ? 2 : 1;
            if (!Global.IsOnlineMatch) return;
            Global.IsPlayerTurn = !Global.IsPlayerTurn;
            DatabaseHandler.ChangeTurn(Global.GameId, Global.PlayerId, () =>
            {
                StartCoroutine(GameHandler.reference.CheckTurn()); 
            });
        }
    }
}
