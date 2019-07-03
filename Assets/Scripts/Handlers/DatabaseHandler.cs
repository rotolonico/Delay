using System;
using System.Collections.Generic;
using System.Linq;
using FullSerializer;
using Globals;
using Proyecto26;
using Serializables;
using UnityEngine;

namespace Handlers
{
    public class DatabaseHandler : MonoBehaviour
    {
        public delegate void EnterWaitingRoomCallback(ResponseHelper response);
        public delegate void IsGameReadyCallback(string gameId, bool ready);
        public delegate void DownloadMapCallback(string map);
        public delegate void DownloadMapsListCallback(List<string> maps);
        public delegate void DownloadOpponentNicknameCallback(string nickname);
        public delegate void CheckTurnCallback(string playerId);
        public delegate void UploadTurnCallback();
        public delegate void ChangeTurnCallback();
        public delegate void DownloadTurnCallback(Move move);

        private static readonly fsSerializer Serializer = new fsSerializer();
        
        public static string projectId;
        private static string databaseURL;

        private void Start()
        {
            databaseURL = $"https://{projectId}.firebaseio.com/";
        }

        public static void UploadMap(string map, string mapName)
        {
            RestClient.Put($"{databaseURL}maps/{BasicFunctions.GetCurrentTimestamp()}-{mapName}.json", map);
        }
        
        public static void DownloadMap(string gameId, DownloadMapCallback callback)
        {
            RestClient.Get($"{databaseURL}games/{gameId}/map.json").Then(response =>
            {
                callback(response.Text);
            });
        }

        public static void DownloadMapsList(DownloadMapsListCallback callback)
        {
            RestClient.Get($"{databaseURL}maps.json?shallow=true").Then(response =>
            {
                var data = fsJsonParser.Parse(response.Text);
                object deserialized = null;
                Serializer.TryDeserialize(data, typeof(Dictionary<string, bool>), ref deserialized);

                callback((deserialized as Dictionary<string, bool>).Keys.ToList());
            });
        }

        public static void DownloadOpponentNickname(string gameId, string playerId, DownloadOpponentNicknameCallback callback)
        {
            var opponentPlayerId = gameId.Remove(gameId.IndexOf(playerId, StringComparison.Ordinal), playerId.Length);
            RestClient.Get($"{databaseURL}games/{gameId}/{opponentPlayerId}.json").Then(response =>
            {
                callback(response.Text.Trim('"'));
            });
        }

        public static void EnterWaitingRoom(string id, string mapName, string nickname, EnterWaitingRoomCallback callback)
        {
            if (nickname == "") nickname = "Unnamed Player";
            var payLoad = "{\"map\":\"" + mapName + "\",\"nickname\":\"" + nickname + "\"}";
            RestClient.Put($"{databaseURL}waitingroom/{id}.json", payLoad).Then(response => { callback(response); });
        }

        public static void ExitWaitingRoom(string id)
        {
            RestClient.Delete($"{databaseURL}waitingroom/{id}.json");
        }
        
        public static void IsGameReady(string id, IsGameReadyCallback callback)
        {
            RestClient.Get($"{databaseURL}waitingroom/{id}/gameid.json").Then(response =>
            {
                callback(response.Text.Trim('"'), response.Text.Trim('"') != "null");
            });
        }

        public static void CheckTurn(string gameId, CheckTurnCallback callback)
        {
            RestClient.Get($"{databaseURL}games/{gameId}/turn.json").Then(response =>
            {
                callback(response.Text.Trim('"'));
            });
        }
        
        public static void ChangeTurn(string gameId, string playerId, ChangeTurnCallback callback)
        {
            var opponentPlayerId = gameId.Remove(gameId.IndexOf(playerId, StringComparison.Ordinal), playerId.Length);
            var payLoad = "\"" + opponentPlayerId + "\"";
            RestClient.Put($"{databaseURL}games/{gameId}/turn.json", payLoad).Then(response => { callback(); });
        }

        public static void UploadMove(string gameId, Move move, UploadTurnCallback callback)
        {
            RestClient.Put<Move>($"{databaseURL}games/{gameId}/move.json", move).Then(response => { callback(); });
        }

        public static void DownloadMove(string gameId, DownloadTurnCallback callback)
        {
            RestClient.Get<Move>($"{databaseURL}games/{gameId}/move.json").Then(response =>
            { 
                callback(response);
            });
        }
    }
}