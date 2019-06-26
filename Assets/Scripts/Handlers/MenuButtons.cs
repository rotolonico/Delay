using Globals;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Handlers
{
    public class MenuButtons : MonoBehaviour
    {
        public string firebaseProjectId;

        public string battlefieldMap;
    
        private void Start()
        {
            DatabaseHandler.projectId = firebaseProjectId;
        }
        
        public void Play()
        {
            Global.SelectedMapJson = battlefieldMap;
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
}
