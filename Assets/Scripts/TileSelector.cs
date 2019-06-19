using System.Collections;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    public static TileSelector reference;
    
    public PossibleTile[] possibleTiles = new PossibleTile[6];
    public Transform selectedPawn;

    private Transform thisTransform;

    private void Start()
    {
        reference = this;
        thisTransform = transform;
    }

    public void ActivateSelector(Transform newSelectedPawn)
    {
        selectedPawn = newSelectedPawn;
        thisTransform.position = selectedPawn.position;
        
        thisTransform.eulerAngles = Vector3.zero;
        
        foreach (var possibleTile in possibleTiles)
        {
            possibleTile.Deactivate();
        }

        foreach (var possibleTile in possibleTiles)
        {
            possibleTile.CheckVisibility();
        }
    }

    public void DeactivateSelector()
    {
        transform.eulerAngles = new Vector3(90, 0, 0);
        selectedPawn = null;
    }
}
