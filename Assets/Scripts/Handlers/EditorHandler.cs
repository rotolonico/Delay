using System;
using System.IO;
using System.Linq;
using FullSerializer;
using Game;
using Globals;
using Serializables;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Handlers
{
    public class EditorHandler : MonoBehaviour
    {
        private readonly fsSerializer serializer = new fsSerializer();
    
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

        private string ConvertMapToJson(string mapName)
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
            if (File.Exists(Application.persistentDataPath + "/" + saveInputField.text + ".dmap"))
            {
                MessageHandler.ShowMessage("A saved map with that name already exists, do you want to replace it?", () =>
                {
                    File.WriteAllText(Application.persistentDataPath + "/" + saveInputField.text + ".dmap", ConvertMapToJson(saveInputField.text));
                    HideWindow();
                    MessageHandler.HideMessage();
                }, MessageHandler.HideMessage);
            }
            else
            {
                File.WriteAllText(Application.persistentDataPath + "/" + saveInputField.text + ".dmap", ConvertMapToJson(saveInputField.text));
                HideWindow();
            }
        }
    
        public void LoadMap()
        {
            if (!File.Exists(Application.persistentDataPath + "/" + loadInputField.text + ".dmap"))
            {
                MessageHandler.ShowMessage("There is no saved map with that name!", MessageHandler.HideMessage, MessageHandler.HideMessage);
                return;
            }
            
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
            MessageHandler.ShowMessage("By continuing, this map will be uploaded and visible to everyone in the map selection.", MessageHandler.HideMessage,
                () =>
                {
                    DatabaseHandler.UploadMap(ConvertMapToJson(uploadInputField.text), uploadInputField.text);
                    HideWindow();
                });
        }

        private void ClearMap()
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

        public void Back()
        {
            if (Global.IsOnlineMatch)
            {
                MessageHandler.ShowMessage("Any unsaved changes will be discarded. Are you sure?", () =>
                {
                    MessageHandler.HideMessage();
                    SceneManager.LoadScene(0);
                }, MessageHandler.HideMessage);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
