using System;
using UnityEngine;

public class PawnHandler : MonoBehaviour
{
    public int team;
    public string pawnType;

    private Transform thisTransform;

    private void Start()
    {
        thisTransform = transform;
    }

    private void OnMouseDown()
    {
        if (MainHandler.TileSelector.selectedPawn == thisTransform)
        {
            MainHandler.TileSelector.DeactivateSelector();
        }
        else
        {
            MainHandler.TileSelector.ActivateSelector(thisTransform);
        }
    }

    public void UpdatePosition(Vector3 newPosition)
    {
        thisTransform.position = newPosition;
        
        var tile = Physics2D.OverlapCircle(newPosition, 0.1f);
        tile.GetComponent<HexagonHandler>().ChangeColor(team);
    }
}
