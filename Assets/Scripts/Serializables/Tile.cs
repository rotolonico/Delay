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

        public Tile(int newTeam, string newType, int newId, float newX, float newY)
        {
            team = newTeam;
            type = newType;
            id = newId;
        
            x = newX;
            y = newY;
        }
    }
}
