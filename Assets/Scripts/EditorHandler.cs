using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FullSerializer;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditorHandler : MonoBehaviour
{
    private fsSerializer serializer = new fsSerializer();
    
    public Grid grid;
    
    public GameObject[] spawnables;
    public static int TileToSpawn;

    private Camera mainCamera;
    private EventSystem mainEventSystem;

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
        var tiles = tilesGameObjects.Select(tilesGameObject => tilesGameObject.GetComponent<TileSettings>()).Select(tileSettings => new Tile(tileSettings.team, tileSettings.type, tileSettings.id, tileSettings.position.x, tileSettings.position.y)).ToList();
        var map = new Map {TileSet = tiles, name = mapName};
        fsData data;
        serializer.TrySerialize(typeof(Map), map, out data).AssertSuccessWithoutWarnings();
        return fsJsonPrinter.CompressedJson(data);
    }

    public void SaveMap(string mapName)
    {
        File.WriteAllText(Application.persistentDataPath + "/" + mapName + ".dmap", ConvertMapToJson(mapName));
    }
    
    public void LoadMap(string mapName)
    {
        var reader = new StreamReader(Application.persistentDataPath + "/" + mapName + ".dmap");
        var data = fsJsonParser.Parse(reader.ReadToEnd());
        reader.Close();
        
        object deserialized = null;
        serializer.TryDeserialize(data, typeof(Map), ref deserialized).AssertSuccessWithoutWarnings();

        var map = deserialized as Map;
        
        ClearMap();

        foreach (var tile in map.TileSet)
        {
            var spawnPosition = new Vector3(tile.x, tile.y, -1);
            Instantiate(spawnables[tile.id], spawnPosition, Quaternion.identity).transform.SetParent(grid.transform, true);

        }
    }

    public void UploadMap(string mapName)
    {
        DatabaseHandler.UploadMap(mapName, ConvertMapToJson(mapName));
    }

    public void ClearMap()
    {
        var tilesGameObjects = GameObject.FindGameObjectsWithTag("EditorTile");
        foreach (var tileGameObject in tilesGameObjects)
        {
            Destroy(tileGameObject);
        }
    }
}
