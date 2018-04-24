using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    /// <summary>
    /// シーンロードのサンプル
    /// </summary>
    public class AsyncLoadSample : MonoBehaviour
    {
        // 初期ディスプレイ
        [SerializeField]
        private Display.DisplayBase _initDisplay = null;

        // 遷移する際押すマウスの対応した番号
        [SerializeField]
        private int _button = 0;

        // 遷移するシーンの名前
        [SerializeField]
        private string _sceneName = string.Empty;

        private void Awake()
        {
            // 初期ディスプレイ
            Display.DisplayManager.Instance.ChangeDisplay(_initDisplay);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(_button))
            {
                // 呼び出しはこれ
                Scene.SceneManager.Instance.ChangeScene(_sceneName);
            }
        }
    }
}