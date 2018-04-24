using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
	public class FadeSample : MonoBehaviour
	{
		void Start()
		{
			StartCoroutine(FadeInOut());
		}

		private IEnumerator FadeInOut()
		{
			// フェードイン
			yield return StartCoroutine(FadeManager.Instance.FadeIn(1.0f));

			// 1秒待つ
			yield return new WaitForSeconds(1.0f);

			// フェードアウト
			yield return StartCoroutine(FadeManager.Instance.FadeOut(1.0f));
		}
	}
}

