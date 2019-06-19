using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossibleTile : MonoBehaviour
{
    private Collider2D[] colliders = new Collider2D[2];

    private PawnHandler selectedPawnHandler;

    public void CheckVisibility()
    {
        selectedPawnHandler = TileSelector.reference.selectedPawn.GetComponent<PawnHandler>();
        if (selectedPawnHandler.pawnType == "Bomb")
        {
            gameObject.SetActive(Physics2D.OverlapCircleNonAlloc(transform.position, 0.1f, colliders) == 1
                                 && colliders[0].GetComponent<HexagonHandler>().hexagonColor != (selectedPawnHandler.team == 1 ? 2 : 1));
        }
        else
        {
            gameObject.SetActive(Physics2D.OverlapCircleNonAlloc(transform.position, 0.1f, colliders) == 1);
        }
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        Deactivate();
        selectedPawnHandler.UpdatePosition(transform.position);
        TileSelector.reference.DeactivateSelector();
    }
}
