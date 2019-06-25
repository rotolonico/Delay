using UnityEngine;

namespace Game
{
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
}
