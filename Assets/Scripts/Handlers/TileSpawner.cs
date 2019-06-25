using System;
using System.Collections;
using UnityEngine;

namespace Handlers
{
    public class TileSpawner : MonoBehaviour
    {
        public Grid grid;
        public GameObject[] spawnables;

        private System.Random rnd = new System.Random();
        private Camera mainCamera;        

        private void Start()
        {
            mainCamera = Camera.main;
            StartCoroutine(SpawnTilesRandomly((float) rnd.NextDouble() / 4));
        }

        private IEnumerator SpawnTilesRandomly(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
        
            var spawnPosition = mainCamera.ScreenToWorldPoint(new Vector3(rnd.Next(Screen.width), rnd.Next(Screen.height), -1));
            spawnPosition.x = Mathf.Round(spawnPosition.x / 1.5f) * 1.5f;
            spawnPosition.y = Math.Abs(spawnPosition.x % 3) < 0.1f ? (int)spawnPosition.y % 2 == 1
                    ?
                    (int)spawnPosition.y + 1
                    : (int)spawnPosition.y :
                (int)spawnPosition.y % 2 == 0 ? (int)spawnPosition.y + 1 :
                (int)spawnPosition.y;
            spawnPosition.z = -1f;

            var occupiedTile = Physics2D.OverlapCircle(spawnPosition, 0.1f);
            if (occupiedTile != null) Destroy(occupiedTile.gameObject);
        
            Instantiate(spawnables[rnd.Next(spawnables.Length - 1)], spawnPosition, Quaternion.identity).transform
                .SetParent(grid.transform, true);

            StartCoroutine(SpawnTilesRandomly((float) rnd.NextDouble() / 4));
        }
    }
}
