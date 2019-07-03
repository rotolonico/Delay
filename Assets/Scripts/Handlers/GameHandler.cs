using System;
using System.Collections;
using FullSerializer;
using Game;
using Globals;
using Serializables;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Handlers
{
    public class GameHandler : MonoBehaviour
    {
        public static GameHandler reference;
        public static GameResources GameResources;
        public static TileSelector TileSelector;
        public static TurnHandler TurnHandler;
        private static Camera Camera;

        public GameObject[] spawnables;
        public Grid grid;

        public GameObject timer;
        public RectTransform timerForeground;
        public Image timerImage;
        public Image timerForegroundImage;

        private bool resigned;
        private bool timerRunning;

        public bool tutorialMode;

        private void Start()
        {
            tutorialMode = !Global.IsOnlineMatch;
            timerRunning = true;
            reference = this;
            GameResources = GetComponent<GameResources>();
            TileSelector = GameObject.Find("TileSelector").GetComponent<TileSelector>();
            TurnHandler = GetComponent<TurnHandler>();
            Camera = Camera.main;

            Global.MaxId = 0;
            LoadMap(Global.SelectedMapJson);

            if (tutorialMode)
                MessageHandler.ShowMessage(
                    "You can use this room to play around with the game. You can also double click pawns to trigger their ability",
                    MessageHandler.HideMessage, MessageHandler.HideMessage);
            if (Global.IsOnlineMatch) 
                MessageHandler.ShowMessage(
                    $"You are fighting against \"{Global.OpponentNickname}\", on the map \"{Global.LoadedMap.name}\". You are {(Global.IsPlayerTurn ? "blue" : "red")}. Good luck!",
                    MessageHandler.HideMessage, MessageHandler.HideMessage);

            if (!Global.IsOnlineMatch) return;
            StartCoroutine(CheckTurn());
            timer.SetActive(true);
        }

        private void Update()
        {
            if (Global.IsOnlineMatch && timerRunning) UpdateTimer();
        }

        private void UpdateTimer()
        {
            if (resigned) return;
            Global.TimeLeft -= Time.deltaTime;
            var newPosition = timerForeground.offsetMin;
            newPosition.x = Global.TimeLeft / 30 * 931;
            timerForeground.offsetMin = newPosition;

            if (Global.TimeLeft > 0) return;
            timerRunning = false;
            if (!Global.IsPlayerTurn) return;
            ResignGame();
            resigned = true;
        }

        private void ClearMap()
        {
            for (var i = 1; i < grid.transform.childCount; i++)
            {
                Destroy(grid.transform.GetChild(i));
            }
        }

        private void LoadMap(string mapJson)
        {
            ClearMap();

            var data = fsJsonParser.Parse(mapJson);

            object deserialized = null;
            Global.Serializer.TryDeserialize(data, typeof(Map), ref deserialized).AssertSuccessWithoutWarnings();

            var map = deserialized as Map;
            Global.LoadedMap = map;

            foreach (var tile in map.TileSet)
            {
                var spawnPosition = new Vector3(tile.x, tile.y, 0);
                var newTile = Instantiate(spawnables[tile.id], spawnPosition, Quaternion.identity);
                newTile.transform.SetParent(grid.transform, true);
                newTile.name = tile.tileId;
            }
        }

        public IEnumerator CheckTurn()
        {
            if (Global.IsPlayerTurn) yield break;
            yield return new WaitForSecondsRealtime(3);
            DatabaseHandler.CheckTurn(Global.GameId, playerTurnId =>
            {
                if (playerTurnId == Global.PlayerId)
                {
                    DatabaseHandler.DownloadMove(Global.GameId, move =>
                    {
                        if (move.resign)
                        {
                            MessageHandler.ShowMessage("Congratulations, You won the match!", () =>
                            {
                                SceneManager.LoadScene(0);
                                MessageHandler.HideMessage();
                            }, () =>
                            {
                                SceneManager.LoadScene(0);
                                MessageHandler.HideMessage();
                            });
                            return;
                        }

                        timerRunning = true;

                        if (move.doubleClick)
                        {
                            GameObject.Find(move.pawn).GetComponent<PawnHandler>().DoubleClick(false);
                        }
                        else
                        {
                            GameObject.Find(move.pawn).GetComponent<PawnHandler>()
                                .UpdatePosition(move.newPosition, true);
                        }

                        TurnHandler.ChangeTurn();
                    });
                }
                else
                {
                    StartCoroutine(CheckTurn());
                }
            });
        }

        public static RaycastHit2D RaycastMouse()
        {
            Vector3 worldPoint = Camera.ScreenToWorldPoint(Input.mousePosition);
            worldPoint.z = Camera.transform.position.z;
            Ray ray = new Ray(worldPoint, new Vector3(0, 0, 1));
            return Physics2D.GetRayIntersection(ray);
        }

        private static void ResignGame()
        {
            MessageHandler.ShowMessage("You lost, better luck next time!", () =>
            {
                SceneManager.LoadScene(0);
                MessageHandler.HideMessage();
            }, () =>
            {
                SceneManager.LoadScene(0);
                MessageHandler.HideMessage();
            });
            
            TurnHandler.ChangeTurn(true, new Move("null", false, Vector2.zero, true));
        }

        public void Back()
        {
            if (Global.IsOnlineMatch)
            {
                MessageHandler.ShowMessage("By quitting you will resign. Are you sure?", () =>
                {
                    MessageHandler.HideMessage();
                    SceneManager.LoadScene(0);
                }, MessageHandler.HideMessage);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}