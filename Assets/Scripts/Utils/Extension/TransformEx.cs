using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public static class TransformEx
    {

        /// <summary>
        /// 子に設定
        /// </summary>
        public static GameObject SetChild(this Transform transform, GameObject child)
        {
            child.transform.SetParent(transform);
            return child;
        }

        /// <summary>
        /// 全ての子を取得
        /// /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static GameObject[] GetAllChild(this Transform transform)
        {
            var list = new List<GameObject>();
            for (int i = 0; i < transform.childCount; i++)
            {
                list.Add(transform.GetChild(i).gameObject);
            }
            return list.ToArray();
        }

        /// <summary>
        /// position、rotation、scaleをリセットする
        /// </summary>
        public static void Reset(this Transform transform)
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = new Vector3(
              transform.localScale.x / transform.lossyScale.x,
              transform.localScale.y / transform.lossyScale.y,
              transform.localScale.z / transform.lossyScale.z
            );
        }

        /// <summary>
        /// ローカルのposition、rotation、scaleをリセットする
        /// </summary>
        public static void LocalReset(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
    }
}