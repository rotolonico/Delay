using System;
using System.Collections.Generic;

namespace Serializables
{
    [Serializable]
    public class Map
    {
        public List<Tile> TileSet;
        public string name;
    }
}
