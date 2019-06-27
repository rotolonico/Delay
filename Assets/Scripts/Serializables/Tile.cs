using System;

namespace Serializables
{
    [Serializable]
    public class Tile
    {
        public int team;
        public string type;
        public int id;

        public float x;
        public float y;

        public string tileId; 

        public Tile(int newTeam, string newType, int newId, float newX, float newY, string newTileId)
        {
            team = newTeam;
            type = newType;
            id = newId;
            tileId = newTileId;
        
            x = newX;
            y = newY;
        }
    }
}
