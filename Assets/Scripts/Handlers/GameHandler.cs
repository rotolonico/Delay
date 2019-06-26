using System.Collections;
using System.IO;
using System.Linq;
using FullSerializer;
using Game;
using Globals;
using Serializables;
using UnityEngine;

namespace Handlers
{
    public class GameHandler : MonoBehaviour
    {
        public static GameHandler reference;
        public static GameResources GameResources;
        public static TileSelector TileSelector;
        public static TurnHandler TurnHandler;
        public static Camera Camera;

        public GameObject[] spawnables;
        public Grid grid;

        private void Start()
        {
            reference = this;
            GameResources = GetComponent<GameResources>();
            TileSelector = GameObject.Find("TileSelector").GetComponent<TileSelector>();
            TurnHandler = GetComponent<TurnHandler>();
            Camera = Camera.main;
            LoadMap(Global.SelectedMapJson);
            if (Global.IsOnlineMatch) StartCoroutine(CheckTurn());
        }

        private void ClearMap()
        {
            for (var i = 1; i < grid.transform.childCount; i++)
            {
                Destroy(grid.transform.GetChild(i));
            }
        }

        private void LoadMap(string mapJson)
        {
            ClearMap();
            
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

        public IEnumerator CheckTurn()
        {
            if (Global.IsPlayerTurn) yield break;
            yield return new WaitForSecondsRealtime(3);
            DatabaseHandler.CheckTurn(Global.GameId, playerTurnId =>
            {
                if (playerTurnId == Global.PlayerId)
                {
                    //DatabaseHandler.DownloadMap(Global.GameId, map =>
                    //{
                        //LoadMap(map);
                        TurnHandler.ChangeTurn();
                    //});
                }
                else
                {
                    StartCoroutine(CheckTurn());
                }
            });
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
