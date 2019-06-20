using System.Collections;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    public static TileSelector reference;
    
    public PossibleTile[] possibleTiles = new PossibleTile[6];
    public Transform selectedPawn;

    private Transform thisTransform;

    private bool isActivated;

    private void Start()
    {
        reference = this;
        thisTransform = transform;
    }

    private void Update()
    {
        if (!isActivated || !Input.GetMouseButtonDown(0)) return;
        var hitCollider = MainHandler.RaycastMouse().collider;
        if (hitCollider.CompareTag("Selector")) hitCollider.GetComponent<PossibleTile>().OnClick();
    }

    public void ActivateSelector(Transform newSelectedPawn)
    {
        isActivated = true;
        
        selectedPawn = newSelectedPawn;

        var thisTransformPosition = selectedPawn.position;
        thisTransformPosition.z = -2;
        thisTransform.position = thisTransformPosition;
        
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
        
        isActivated = false;
    }
}
