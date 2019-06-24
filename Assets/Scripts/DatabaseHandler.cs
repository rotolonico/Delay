using System;
using System.Collections;
using System.Collections.Generic;
using Proyecto26;
using UnityEngine;

public class DatabaseHandler : MonoBehaviour
{
    public static string projectId;
    private static string databaseURL;

    private void Start()
    {
        databaseURL = $"https://{projectId}.firebaseio.com/";
    }

    public static void UploadMap(string mapName, string map)
    {
        RestClient.Put($"{databaseURL}maps/{mapName}.json", map);
    }
}
