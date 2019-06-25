using Handlers;
using UnityEngine;

namespace Game
{
    public class TileId : MonoBehaviour
    {
        public int id;

        public void OnClick()
        {
            EditorHandler.TileToSpawn = id;
        }
    }
}
