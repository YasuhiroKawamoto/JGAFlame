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
		// 遷移する際押すマウスの対応した番号
		[SerializeField]
		private int _button = 0;

		// 遷移するシーンの名前
		[SerializeField]
		private string _sceneName = string.Empty;

		void Update()
		{
			if (Input.GetMouseButtonDown(_button))
			{
				// 呼び出しはこれ
				StartCoroutine(Sceneload());
			}
		}

		private IEnumerator Sceneload()
		{
			// ローダーを作成 Monobehaviour なので new は不可
			var load = gameObject.AddComponent<SceneLoader>();

			// 読み込み開始
			load.LoadStart(_sceneName);

			// 読み込み待ち
			yield return new WaitUntil(() => load.IsLoading == false);

			// シーンの変更
			load.ChangeScene();
		}
	}
}