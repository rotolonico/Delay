using UnityEngine;

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
    }
}
