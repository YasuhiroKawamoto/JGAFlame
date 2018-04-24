using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Util.Scene
{
    public class SceneLoader : MonoBehaviour
    {
        // コールバック
        public delegate void Callback();

        // 非同期ロードのシーン
        private AsyncOperation _async;

        // 読み込み状況(読み込み時true)
        public bool IsLoading { get; private set; }

        /// <summary>
        /// 読み込み開始
        /// </summary>
        /// <param name="sceneName"></param>
        public void LoadStart(string sceneName)
        {
            // 読み込み開始
            this.IsLoading = true;

            // 読み込み後待機
            StartCoroutine(LoadSceneWait(sceneName));
        }

        /// <summary>
        /// シーンの読み込み
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        private IEnumerator LoadSceneWait(string sceneName)
        {
            // 非同期読み込み開始
            _async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

            // 読み込み終わってもロードしない
            _async.allowSceneActivation = false;

            // 読み込み待ち
            while (_async.progress < 0.9f)
            {
                yield return null;
            }

            this.IsLoading = false;
        }

        /// <summary>
        /// シーンの変更
        /// </summary>
        /// <returns></returns>
        public bool ChangeScene(Callback callback = null)
        {
            // 読み込み開始しているか？
            if (_async != null)
            {
                // 読み込みが完了して待機しているか？
                if (0.9f <= _async.progress)
                {
                    // 遷移する。
                    _async.allowSceneActivation = true;

                    if (callback != null)
                    {
                        callback();
                    }
                }
            }

            return false;
        }
    }
}