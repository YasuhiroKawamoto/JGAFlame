using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    /// <summary>
    /// MonoBehaviourを継承したシングルトン
    /// </summary>
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// インスタンス
        /// </summary>
        private static volatile T _instance;

        /// <summary>
        /// 同期オブジェクト
        /// </summary>
        private static object _syncObj = new object();

        /// <summary>
        /// インスタンスのgetter/setter
        /// </summary>
        public static T Instance
        {
            get
            {
                // インスタンスがない場合に探す
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>() as T;

                    // 複数のインスタンスがあった場合
                    if (FindObjectsOfType<T>().Length > 1)
                    {
                        return _instance;
                    }

                    // Findで見つからなかった場合、新しくオブジェクトを生成
                    if (_instance == null)
                    {
                        // 同時にインスタンス生成を呼ばないためにlockする
                        lock (_syncObj)
                        {
                            var singleton = new GameObject();

                            // シングルトンオブジェクトだと分かりやすいように名前を設定
                            singleton.name = typeof(T).ToString() + " (Singleton)";

                            _instance = singleton.AddComponent<T>();

                            // シーン変更時に破棄させない
                            DontDestroyOnLoad(singleton);
                        }
                    }
                }
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }

        /// <summary>
        /// インスタンスの破棄
        /// </summary>
        void OnDestroy()
        {
            Instance = null;
        }

        // インスタンスを生成出来なくする
        protected SingletonMonoBehaviour() { }
    }
}