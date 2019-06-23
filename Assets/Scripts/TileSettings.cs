using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSettings : MonoBehaviour
{
    public int team;
    public string type;
    public int id;

    public Vector2 position;

    private void Start()
    {
        position = transform.position;
    }
}
