using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Util.Display
{
    public class DisplayManager : SingletonMonoBehaviour<DisplayManager>
    {
        // 現在の表示しているディスプレイ
        private DisplayBase _currentDisplay = null;

        public void ChangeDisplay(DisplayBase display)
        {
            // 現在のシーンがある場合は終了処理
            if (_currentDisplay != null)
            {
                _currentDisplay.Exit();
            }

            // 読み込みなど開始
            StartCoroutine(DisplayLoad(display));
        }

        private IEnumerator DisplayLoad(DisplayBase display)
        {
            _currentDisplay = display;

            // 初期化
            yield return StartCoroutine(_currentDisplay.Enter());

            _currentDisplay.EnterComplete();
        }

    }
}