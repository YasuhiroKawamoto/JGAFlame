using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    /// <summary>
    /// Vector3の拡張
    /// </summary>
    public static class Vector3Ex
    {
        /// <summary>
        /// 一番近くの座標を取得
        /// </summary>
        /// <param name="position">World position.</param>
        /// <param name="otherPositions">Other world positions.</param>
        /// <returns>Closest position.</returns>
        public static Vector3 GetClosest(this Vector3 position, IEnumerable<Vector3> otherPositions)
        {
            // 座標
            var closest = Vector3.zero;

            // 距離
            var shortestDistance = Mathf.Infinity;

            foreach (var otherPosition in otherPositions)
            {
                var distance = (position - otherPosition).sqrMagnitude;

                if (distance < shortestDistance)
                {
                    closest = otherPosition;
                    shortestDistance = distance;
                }
            }

            return closest;
        }
    }
}