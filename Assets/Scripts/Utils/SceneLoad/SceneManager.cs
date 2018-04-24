using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util.Scene
{
    // シーン遷移用クラス
    public class SceneManager : SingletonMonoBehaviour<SceneManager>
    {
        /// <summary>
        /// シーンの変更
        /// </summary>
        /// <param name="sceneName"></param>
        public void ChangeScene(string sceneName)
        {
            StartCoroutine(ChangeSceneInternal(sceneName));
        }

        /// <summary>
        /// シーンの変更フェードインのみ
        /// </summary>
        /// <param name="sceneName"></param>
        public void ChangeSceneFadeIn(string sceneName, float interval = 1.0f)
        {
            StartCoroutine(ChangeSceneInternal(sceneName, interval));
        }

        /// <summary>
        /// シーンの変更フェードインとアウト
        /// </summary>
        /// <param name="sceneName"></param>
        public void ChangeSceneFadeInOut(string sceneName, float interval = 1.0f)
        {
            StartCoroutine(ChangeSceneInternal(sceneName, interval, interval));
        }

        /// <summary>
        /// シーンの変更
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="isFade"></param>
        /// <returns></returns>
        private IEnumerator ChangeSceneInternal(string sceneName, float inInterval = 0.0f, float outInterval = 0.0f)
        {
            // ローダーを作成 Monobehaviour なので new は不可
            var load = gameObject.AddComponent<SceneLoader>();

            // フェード
            yield return StartCoroutine(FadeManager.Instance.FadeIn(inInterval));

            // 読み込み開始
            load.LoadStart(sceneName);

            // 読み込み待ち
            yield return new WaitUntil(() => load.IsLoading == false);

            // フェードアウト
            StartCoroutine(FadeManager.Instance.FadeOut(outInterval));

            // シーンの変更
            load.ChangeScene();

        }
    }
}
