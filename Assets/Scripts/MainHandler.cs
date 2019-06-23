using UnityEngine;

public class MainHandler : MonoBehaviour
{
    public static GameResources GameResources;
    public static TileSelector TileSelector;
    public static TurnHandler TurnHandler;
    public static Camera Camera;

    private void Start()
    {
        GameResources = GetComponent<GameResources>();
        TileSelector = GameObject.Find("TileSelector").GetComponent<TileSelector>();
        TurnHandler = GetComponent<TurnHandler>();
        Camera = Camera.main;
    }

    public static RaycastHit2D RaycastMouse()
    {
        Vector3 worldPoint = Camera.ScreenToWorldPoint(Input.mousePosition);
        worldPoint.z = Camera.transform.position.z;
        Ray ray = new Ray(worldPoint, new Vector3(0, 0, 1));
        return Physics2D.GetRayIntersection(ray);
    }
}
