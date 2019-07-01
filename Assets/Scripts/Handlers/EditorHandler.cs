using System;
using System.IO;
using System.Linq;
using FullSerializer;
using Game;
using Serializables;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Handlers
{
    public class EditorHandler : MonoBehaviour
    {
        private fsSerializer serializer = new fsSerializer();
    
        public Grid grid;
    
        public GameObject[] spawnables;
        public static int TileToSpawn;

        private Camera mainCamera;
        private EventSystem mainEventSystem;

        public GameObject saveWindow;
        public GameObject loadWindow;
        public GameObject uploadWindow;

        public TMP_InputField saveInputField;
        public TMP_InputField loadInputField;
        public TMP_InputField uploadInputField;
        

        private void Start()
        {
            mainCamera = Camera.main;
            mainEventSystem = EventSystem.current;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !mainEventSystem.IsPointerOverGameObject())
            {
                SpawnTile();
            }
        }

        private void SpawnTile()
        {
            var spawnPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            spawnPosition.x = Mathf.Round(spawnPosition.x / 1.5f) * 1.5f;
            spawnPosition.y = Math.Abs(spawnPosition.x % 3) < 0.1f ? (int)spawnPosition.y % 2 == 1
                    ?
                    (int)spawnPosition.y + 1
                    : (int)spawnPosition.y :
                (int)spawnPosition.y % 2 == 0 ? (int)spawnPosition.y + 1 :
                (int)spawnPosition.y;
            spawnPosition.z = -1f;

            var occupiedTile = Physics2D.OverlapCircle(spawnPosition, 0.1f);
            if (occupiedTile != null) Destroy(occupiedTile.gameObject);
        
            if (TileToSpawn == -1) return;
            Instantiate(spawnables[TileToSpawn], spawnPosition, Quaternion.identity).transform
                .SetParent(grid.transform, true);
        }

        public string ConvertMapToJson(string mapName)
        {
            var tilesGameObjects = GameObject.FindGameObjectsWithTag("EditorTile");
            var tiles = tilesGameObjects.Select(tilesGameObject => tilesGameObject.GetComponent<TileSettings>()).Select(tileSettings => new Tile(tileSettings.team, tileSettings.type, tileSettings.id, tileSettings.position.x, tileSettings.position.y, tileSettings.tileId)).ToList();
            var map = new Map {TileSet = tiles, name = mapName};
            fsData data;
            serializer.TrySerialize(typeof(Map), map, out data).AssertSuccessWithoutWarnings();
            return fsJsonPrinter.CompressedJson(data);
        }

        public void SaveMap()
        {
            File.WriteAllText(Application.persistentDataPath + "/" + saveInputField.text + ".dmap", ConvertMapToJson(saveInputField.text));
            
            HideWindow();
        }
    
        public void LoadMap()
        {
            var reader = new StreamReader(Application.persistentDataPath + "/" + loadInputField.text + ".dmap");
            var data = fsJsonParser.Parse(reader.ReadToEnd());
            reader.Close();
        
            object deserialized = null;
            serializer.TryDeserialize(data, typeof(Map), ref deserialized).AssertSuccessWithoutWarnings();

            var map = deserialized as Map;
        
            ClearMap();

            foreach (var tile in map.TileSet)
            {
                var spawnPosition = new Vector3(tile.x, tile.y, -1);
                var newTile = Instantiate(spawnables[tile.id], spawnPosition, Quaternion.identity);
                newTile.transform.SetParent(grid.transform, true);
                var newTileSettings = newTile.GetComponent<TileSettings>();
                newTileSettings.loadedTile = true;
                newTileSettings.tileId = tile.tileId;

            }
            
            HideWindow();
        }

        public void UploadMap()
        {
            DatabaseHandler.UploadMap(ConvertMapToJson(uploadInputField.text), uploadInputField.text);
            HideWindow();
        }

        public void ClearMap()
        {
            var tilesGameObjects = GameObject.FindGameObjectsWithTag("EditorTile");
            foreach (var tileGameObject in tilesGameObjects)
            {
                Destroy(tileGameObject);
            }
        }

        public void ShowSaveWindow()
        {
            saveWindow.SetActive(true);
        }
        
        public void ShowLoadWindow()
        {
            loadWindow.SetActive(true);
        }
        
        public void ShowUploadWindow()
        {
            uploadWindow.SetActive(true);
        }

        public void HideWindow()
        {
            saveWindow.SetActive(false);
            saveInputField.text = "";
            loadWindow.SetActive(false);
            loadInputField.text = "";
            uploadWindow.SetActive(false);
            uploadInputField.text = "";
        }
    }
}
