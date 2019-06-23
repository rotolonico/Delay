using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileId : MonoBehaviour
{
    public int id;

    public void OnClick()
    {
        EditorHandler.TileToSpawn = id;
    }
}
