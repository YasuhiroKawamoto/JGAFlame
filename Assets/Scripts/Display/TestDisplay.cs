using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util.Display
{
    public sealed class TestDisplay : DisplayBase
    {
        // 遷移するディスプレイ
        [SerializeField]
        private DisplayBase _changeDisplay = null;

        // 遷移する際押すマウスの対応した番号
        [SerializeField]
        private int _button = 1;

        public override IEnumerator Enter()
        {
            yield return null;
        }

        public override void EnterComplete()
        {
            base.EnterComplete();
        }

        public override void Exit()
        {
            base.Exit();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(_button))
            {
                // 呼び出しはこれ
                DisplayManager.Instance.ChangeDisplay(_changeDisplay);
            }
        }
    }
}