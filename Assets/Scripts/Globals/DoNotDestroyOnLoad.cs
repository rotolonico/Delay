using UnityEngine;

namespace Globals
{
    public class DoNotDestroyOnLoad : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
