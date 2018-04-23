using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Util
{
    public class SceneLoader : SingletonMonoBehaviour<SceneLoader>
    {
        // コールバック
        public delegate void Callback();

        private AsyncOperation _async;

        /// <summary>
        /// シーンの読み込み
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IEnumerator LoadSceneWait(string sceneName, Callback callback = null)
        {
            // 非同期読み込み開始
            _async = SceneManager.LoadSceneAsync(sceneName);

            // 読み込み終わってもロードしない
            _async.allowSceneActivation = false;

            // 読み込み待ち
            while (_async.progress < 0.9f)
            {
                yield return null;
            }

            // ロード完了後
            if (callback != null)
            {
                // コールバック処理
                callback();
            }
        }

        /// <summary>
        /// シーンの変更
        /// </summary>
        /// <returns></returns>
        public bool ChangeScene()
        {
            if (_async != null)
            {
                if (0.9f <= _async.progress)
                {
                    _async.allowSceneActivation = true;
                }
            }

            return false;
        }
    }
}