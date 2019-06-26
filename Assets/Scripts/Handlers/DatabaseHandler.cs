using System;
using Proyecto26;
using UnityEngine;

namespace Handlers
{
    public class DatabaseHandler : MonoBehaviour
    {
        public delegate void EnterWaitingRoomCallback(ResponseHelper response);
        public delegate void IsGameReadyCallback(string gameId, bool ready);
        public delegate void DownloadMapCallback(string map);
        public delegate void CheckTurnCallback(string playerId);
        public delegate void ChangeTurnCallback();
    
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
                callback(response.Text.Trim('"'), response.Text.Trim('"') != "null");
            });
        }

        public static void DownloadMap(string gameId, DownloadMapCallback callback)
        {
            RestClient.Get($"{databaseURL}games/{gameId}/map.json").Then(response =>
            {
                callback(response.Text);
            });
        }
        
        public static void CheckTurn(string gameId, CheckTurnCallback callback)
        {
            RestClient.Get($"{databaseURL}games/{gameId}/turn.json").Then(response =>
            {
                Debug.Log(gameId);
                Debug.Log(response.Text);
                Debug.Log(response.Text.Trim('"'));
                callback(response.Text.Trim('"'));
            });
        }

        public static void ChangeTurn(string gameId, string playerId, ChangeTurnCallback callback)
        {
            var opponentPlayerId = gameId.Remove(gameId.IndexOf(playerId, StringComparison.Ordinal), playerId.Length);
            var payLoad = "\"" + opponentPlayerId + "\"";
            RestClient.Put($"{databaseURL}games/{gameId}/turn.json", payLoad).Then(response => { callback(); });
        }
    }
}