using FullSerializer;
using UnityEngine;

namespace Globals
{
    public static class Global
    {
        public static fsSerializer Serializer = new fsSerializer();
        public static string SelectedMapJson;
        public static bool IsOnlineMatch;
        public static bool IsPlayerTurn;
        public static bool FlippedBoard;
        public static string GameId;
        public static string PlayerId = BasicFunctions.GetCurrentTimestamp().ToString();
        public static int MaxId;
        public const float MaxTime = 30;
        public static float TimeLeft = MaxTime;

        public static Color lightBlue = new Color(0.1882353f, 0.5647059f, 0.5647059f);
        public static Color darkBlue = new Color(0.1411765f, 0.4196078f, 0.4196078f);
        public static Color lightRed = new Color(0.5647059f, 0.1882353f, 0.1882353f);
        public static Color darkRed = new Color(0.4196078f, 0.1411765f, 0.1411765f);
    }
}
