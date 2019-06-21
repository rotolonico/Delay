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
                                     && colliders[0].GetComponent<HexagonHandler>().team != (selectedPawnHandler.team == 1 ? 2 : 1));
                break;
            case "Swordsman":
                if (Physics2D.OverlapCircleNonAlloc(transform.position, 0.1f, colliders) >= 1)
                {
                    if (colliders[0].CompareTag("Hexagon")) gameObject.SetActive(true);
                    else if (!colliders[0].CompareTag("Bomb") &&
                             colliders[0].GetComponent<PawnHandler>().team != selectedPawnHandler.team &&
                             !colliders[0].GetComponent<PawnHandler>().isProtected)
                    {
                        gameObject.SetActive(true);
                        possibleCapture = colliders[0].GetComponent<PawnHandler>();
                    }
                    else gameObject.SetActive(false); 
                } else gameObject.SetActive(false);

                break;
            case "Protector":
                gameObject.SetActive(Physics2D.OverlapCircleNonAlloc(transform.position, 0.1f, colliders) == 1
                                     && colliders[0].GetComponent<HexagonHandler>().team != (selectedPawnHandler.team == 1 ? 2 : 1));
                break;
            case "King":
                if (Physics2D.OverlapCircleNonAlloc(transform.position, 0.1f, colliders) >= 1)
                {
                    if (colliders[0].CompareTag("Hexagon")) gameObject.SetActive(true);
                    else if (colliders[0].GetComponent<PawnHandler>().team != selectedPawnHandler.team &&
                             !colliders[0].GetComponent<PawnHandler>().isProtected)
                    {
                        gameObject.SetActive(true);
                        possibleCapture = colliders[0].GetComponent<PawnHandler>();
                    }
                    else gameObject.SetActive(false); 
                } else gameObject.SetActive(false);
                break;
        }
    }
    
    public void Activate()
    {
        gameObject.SetActive(false);
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

    public void DoubleClick()
    {
        selectedPawnHandler = TileSelector.reference.selectedPawn.GetComponent<PawnHandler>();
        
        switch (selectedPawnHandler.pawnType)
        {
            case "Bomb":
                if (Physics2D.OverlapCircleNonAlloc(transform.position, 0.1f, colliders) > 0)
                {
                    if (colliders[0].CompareTag("Hexagon")) colliders[0].GetComponent<HexagonHandler>().ChangeTeam(selectedPawnHandler.team);
                    else
                    {
                        if (!colliders[0].GetComponent<PawnHandler>().isProtected) Destroy(colliders[0].gameObject);
                        colliders[1].GetComponent<HexagonHandler>().ChangeTeam(selectedPawnHandler.team);
                    } 
                }
                break;
            case "Protector":
                if (Physics2D.OverlapCircleNonAlloc(transform.position, 0.1f, colliders) > 1)
                {
                    colliders[0].GetComponent<PawnHandler>().Protect();
                }

                break;
        }
        
        Deactivate();
    }
}
