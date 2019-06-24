using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public string firebaseProjectId;
    
    private void Start()
    {
        DatabaseHandler.projectId = firebaseProjectId;
    }
    

    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    
    public void Editor()
    {
        SceneManager.LoadScene(2);
    }
    
    public void Online()
    {
        SceneManager.LoadScene(3);
    }
    
    public void Exit()
    {
        Application.Quit();
    }
}
