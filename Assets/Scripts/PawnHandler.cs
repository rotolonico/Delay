using System;
using UnityEngine;

public class PawnHandler : MonoBehaviour
{
    public bool deactivated;
    
    public int team;
    public string pawnType;

    private Transform thisTransform;
    private SpriteRenderer sr;

    private void Start()
    {
        thisTransform = transform;
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (!deactivated && MainHandler.TurnHandler.turn != team) Deactivate();
        else if (deactivated && MainHandler.TurnHandler.turn == team) Activate();
    }
    
    private void Activate()
    {
        sr.color = new Color(1,1,1);
        deactivated = false;
    }

    private void Deactivate()
    {
        sr.color = new Color(0.5f,0.5f,0.5f);
        deactivated = true;
    }

    private void OnMouseDown()
    {
        if (deactivated) return;
        
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
