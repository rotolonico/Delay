using System;
using UnityEngine;

namespace Globals
{
    public class BasicFunctions : MonoBehaviour
    {
        /// <summary>
        /// Returns the current timestamp in milliseconds
        /// </summary>
        public static long GetCurrentTimestamp()
        {
            return (long) (DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }
    }
}
