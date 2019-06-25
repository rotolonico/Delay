using System.IO;
using FullSerializer;
using Game;
using Globals;
using Serializables;
using UnityEngine;

namespace Handlers
{
    public class MainHandler : MonoBehaviour
    {
        public static GameResources GameResources;
        public static TileSelector TileSelector;
        public static TurnHandler TurnHandler;
        public static Camera Camera;

        public GameObject[] spawnables;
        public Grid grid;

        private void Start()
        {
            GameResources = GetComponent<GameResources>();
            TileSelector = GameObject.Find("TileSelector").GetComponent<TileSelector>();
            TurnHandler = GetComponent<TurnHandler>();
            Camera = Camera.main;
            LoadMap(Global.selectedMapJson);
        }

        private void LoadMap(string mapJson)
        {
            var data = fsJsonParser.Parse(mapJson);
            
            object deserialized = null;
            Global.Serializer.TryDeserialize(data, typeof(Map), ref deserialized).AssertSuccessWithoutWarnings();

            var map = deserialized as Map;

            foreach (var tile in map.TileSet)
            {
                var spawnPosition = new Vector3(tile.x, tile.y, 0);
                Instantiate(spawnables[tile.id], spawnPosition, Quaternion.identity).transform.SetParent(grid.transform, true);
            }
        }

        public static RaycastHit2D RaycastMouse()
        {
            Vector3 worldPoint = Camera.ScreenToWorldPoint(Input.mousePosition);
            worldPoint.z = Camera.transform.position.z;
            Ray ray = new Ray(worldPoint, new Vector3(0, 0, 1));
            return Physics2D.GetRayIntersection(ray);
        }
    }
}
