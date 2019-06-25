using Proyecto26;
using UnityEngine;

namespace Handlers
{
    public class DatabaseHandler : MonoBehaviour
    {
        public delegate void EnterWaitingRoomCallback(ResponseHelper response);
        public delegate void IsGameReadyCallback(string gameId, bool ready);
        public delegate void DownloadMapCallback(string map);
        public delegate void GetMapToDownloadCallback(string mapName);
    
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

        public static void EnterWaitingRoom(string id, string mapName, EnterWaitingRoomCallback callback)
        {
            var payLoad = "{\"map\":\"" + mapName + "\"}";
            RestClient.Put($"{databaseURL}waitingroom/{id}.json", payLoad).Then(response => { callback(response); });
        }

        public static void IsGameReady(string id, IsGameReadyCallback callback)
        {
            RestClient.Get($"{databaseURL}waitingroom/{id}/gameid.json").Then(response =>
            {
                callback(response.Text, response.Text != "null");
            });
        }

        public static void GetMapToDownload(string gameId, GetMapToDownloadCallback callback)
        {
            RestClient.Get($"{databaseURL}games/{gameId}/map.json").Then(response =>
            {
                callback(response.Text);
            });
        }

        public static void DownloadMap(string mapName, DownloadMapCallback callback)
        {
            RestClient.Get($"{databaseURL}maps/{mapName}.json").Then(response =>
            {
                Debug.Log(response.Text);
                callback(response.Text);
            });
        }
    }
}