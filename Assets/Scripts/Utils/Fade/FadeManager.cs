using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Util
{
    /// <summary>
    /// シーン遷移時のフェードイン・アウトを制御するためのクラス
    /// </summary>
    public class FadeManager : SingletonMonoBehaviour<FadeManager>
    {
        // コールバック
        public delegate void Callback();

        // 透明度
        private float _fadeAlpha = 0;

        // 現在の状態
        private bool _isFading = false;
        public bool IsFading
        {
            get { return _isFading; }
            private set { _isFading = value; }
        }

        // フェードの色
        public Color _fadeColor = Color.black;

        /// <summary>
        /// フェードの表示用GUI
        /// </summary>
        public void OnGUI()
        {
            // Fade
            if (0.0f < _fadeAlpha)
            {
                // 色と透明度を更新して白テクスチャを描画
                this._fadeColor.a = this._fadeAlpha;
                GUI.color = this._fadeColor;
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            }
        }

        /// <summary>
        /// フェードイン
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public IEnumerator FadeIn(float interval)
        {
            // Fade 開始
            this.IsFading = true;

            float time = 0.0f;

            while (time <= interval)
            {
                this._fadeAlpha = Mathf.Lerp(0f, 1f, time / interval);
                time += Time.deltaTime;
                yield return 0;
            }

            // フェード終わり
            this.IsFading = false;
        }
        /// <param name="callback"></param>
        public IEnumerator FadeIn(float interval, Callback callback = null)
        {
            // フェード
            yield return StartCoroutine(FadeIn(interval));

            if (callback != null)
            {
                // コールバック
                callback();
            }
        }

        /// <summary>
        /// フェードアウト
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public IEnumerator FadeOut(float interval)
        {
            // フェード開始
            this.IsFading = true;

            float time = 0.0f;

            while (time <= interval)
            {
                this._fadeAlpha = Mathf.Lerp(1f, 0f, time / interval);
                time += Time.deltaTime;
                yield return 0;
            }

            // フェード終わり
            this.IsFading = false;
        }
        /// <param name="callback"></param>
        public IEnumerator FadeOut(float interval, Callback callback = null)
        {
            // フェードアウト
            yield return StartCoroutine(FadeOut(interval));

            if (callback != null)
            {
                // コールバック処理
                callback();
            }
        }
    }
}