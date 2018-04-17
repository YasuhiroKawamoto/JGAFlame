using System;
using UnityEngine;

namespace Extensions
{
    /// <summary>
    /// 死亡時のコールバッククラス
    /// </summary>
    public class OnDestroyCallback : MonoBehaviour
    {

        Action onDestroy;

        /// <summary>
        /// 死亡時の処理を追加
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="callback"></param>
        public static void AddOnDestroyCallback(GameObject gameObject, Action callback)
        {
            OnDestroyCallback onDestroyCallback = gameObject.GetComponent<OnDestroyCallback>();
            if (!onDestroyCallback)
            {
                onDestroyCallback = gameObject.AddComponent<OnDestroyCallback>();
                onDestroyCallback.hideFlags = HideFlags.HideAndDontSave;
            }
            onDestroyCallback.onDestroy += callback;
        }

        private void OnDestroy()
        {
            if (onDestroy != null)
            {
                onDestroy();
            }
        }
    }

    /// <summary>
    /// 拡張
    /// </summary>
    public static class OnDestroyCallbackExtensions
    {
        public static void AddOnDestroyCallback(this GameObject gameObject, Action callback)
        {
            OnDestroyCallback.AddOnDestroyCallback(gameObject, callback);
        }
    }
}