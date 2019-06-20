using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossibleTile : MonoBehaviour
{
    private Collider2D[] colliders = new Collider2D[2];
    private PawnHandler possibleCapture;

    private PawnHandler selectedPawnHandler;

    public void CheckVisibility()
    {
        selectedPawnHandler = TileSelector.reference.selectedPawn.GetComponent<PawnHandler>();

        switch (selectedPawnHandler.pawnType)
        {
            case "Bomb":
                gameObject.SetActive(Physics2D.OverlapCircleNonAlloc(transform.position, 0.1f, colliders) == 1
                                     && colliders[0].GetComponent<HexagonHandler>().hexagonColor != (selectedPawnHandler.team == 1 ? 2 : 1));
                break;
            case "Swordsman":
                if (Physics2D.OverlapCircleNonAlloc(transform.position, 0.1f, colliders) >= 1)
                {
                    if (colliders[0].CompareTag("Hexagon")) gameObject.SetActive(true);
                    else if (!colliders[0].CompareTag("Bomb") &&
                             colliders[0].GetComponent<PawnHandler>().team != selectedPawnHandler.team)
                    {
                        gameObject.SetActive(true);
                        possibleCapture = colliders[0].GetComponent<PawnHandler>();
                    }
                    else gameObject.SetActive(false); 
                } else gameObject.SetActive(false);

                break;
        }
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        possibleCapture = null;
    }

    public void OnClick()
    {
        if (possibleCapture != null) Destroy(possibleCapture.gameObject);
        
        Deactivate();
        
        selectedPawnHandler.UpdatePosition(transform.position);
        TileSelector.reference.DeactivateSelector();

        MainHandler.TurnHandler.ChangeTurn();
    }
}
