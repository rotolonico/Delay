using FullSerializer;

namespace Globals
{
    public class Global
    {
        public static fsSerializer Serializer = new fsSerializer();
        public static string SelectedMapJson;
        public static bool IsOnlineMatch;
        public static bool IsPlayerTurn;
        public static string GameId;
        public static string PlayerId;
    }
}
