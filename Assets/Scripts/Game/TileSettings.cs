using Globals;
using UnityEngine;

namespace Game
{
    public class TileSettings : MonoBehaviour
    {
        public int team;
        public string type;
        public int id;

        public Vector2 position;

        public string tileId;

        public bool loadedTile;

        private void Start()
        {
            position = transform.position;
            if (!loadedTile) tileId = BasicFunctions.GetCurrentTimestamp().ToString();
        }
    }
}
