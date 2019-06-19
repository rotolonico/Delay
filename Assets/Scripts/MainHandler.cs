using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class MainHandler : MonoBehaviour
{
    public static Resources Resources;
    public static TileSelector TileSelector;

    private void Start()
    {
        Resources = GetComponent<Resources>();
        TileSelector = GameObject.Find("TileSelector").GetComponent<TileSelector>();
    }
}
