using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    /// <summary>
    /// Extension methods for UnityEngine.Vector3Int.
    /// </summary>
    public static class Vector3IntExtensions
    {
        /// <summary>
        /// Vector3に変換
        /// </summary>
        /// <param name="vector">Vector.</param>
        /// <returns>Vector3 struct.</returns>
        public static Vector3 ToVector3(this Vector3Int vector)
        {
            return new Vector3(
                System.Convert.ToSingle(vector.x),
                System.Convert.ToSingle(vector.y),
                System.Convert.ToSingle(vector.z)
            );
        }
    }
}