using System;
using UnityEngine;

namespace Serializables
{
    [Serializable]
    public class Move
    {
        public string pawn;
        public bool doubleClick;
        public Vector2 newPosition;
        public bool resign;

        public Move(string pawn, bool doubleClick, Vector2 newPosition, bool resign)
        {
            this.pawn = pawn;
            this.doubleClick = doubleClick;
            this.newPosition = newPosition;
            this.resign = resign;
        }
    }
}
