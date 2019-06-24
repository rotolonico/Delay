using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;

public class DatabaseHandler : MonoBehaviour
{
    private static DatabaseReference reference;
    
    public string databaseURL;
    
    private void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(databaseURL);
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public static void UploadMap(string map)
    {
        reference.Child("maps").SetRawJsonValueAsync(map);
    }
}
