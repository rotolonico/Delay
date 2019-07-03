using FullSerializer;
using Serializables;
using UnityEngine;

namespace Globals
{
    public static class Global
    {
        public static fsSerializer Serializer = new fsSerializer();
        public static string SelectedMapJson;
        public static Map LoadedMap;
        public static bool IsOnlineMatch;
        public static bool IsPlayerTurn;
        public static string GameId;
        public static readonly string PlayerId = BasicFunctions.GetCurrentTimestamp().ToString();
        public static string OpponentNickname;
        public static int MaxId;
        public const float MaxTime = 30;
        public static float TimeLeft = MaxTime;

        public static Color lightBlue = new Color(0.1882353f, 0.5647059f, 0.5647059f);
        public static Color darkBlue = new Color(0.1411765f, 0.4196078f, 0.4196078f);
        public static Color lightRed = new Color(0.5647059f, 0.1882353f, 0.1882353f);
        public static Color darkRed = new Color(0.4196078f, 0.1411765f, 0.1411765f);

        public static void Reset()
        {
            SelectedMapJson = "";
            IsOnlineMatch = false;
            IsPlayerTurn = false;
            OpponentNickname = "";
            GameId = "";
            MaxId = 0;
            TimeLeft = MaxTime;
        }
    }
}
