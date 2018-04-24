using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Util.Display
{
    [Serializable]
    public abstract class DisplayBase : MonoBehaviour
    {
        /// <summary>
        /// Displayの初期化処理	
        /// </summary>
        public abstract IEnumerator Enter();

        /// <summary>
        /// Displayの読み込み後の処理
        /// </summary>
        public virtual void EnterComplete()
        {
            this.gameObject.SetActive(true);
        }

        /// <summary>
        /// 変更後の後始末
        /// </summary>
        public virtual void Exit()
        {
            this.gameObject.SetActive(false);
        }
    }
}