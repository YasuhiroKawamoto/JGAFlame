using UnityEngine;
using Util.Save;

// テスト用データ
public class SaveData : SaveSingleton<SaveData>
{
	public int Id;
	public string Name;
}

public class SaveSample : MonoBehaviour
{
	void Start()
	{
		// セーブデータの削除
		SaveData.Instance.Delete();

		// セーブデータの読み込み
		SaveData.Instance.Load();

		// データ設定
		SaveData.Instance.Id = 1;
		SaveData.Instance.Name = "Persona";

		// ログ
		Debug.Log(SaveData.Instance.Id);
		Debug.Log(SaveData.Instance.Name);

		// データのリセット
		SaveData.Instance.Reset();

		// ログ
		Debug.Log(SaveData.Instance.Id);
		Debug.Log(SaveData.Instance.Name);

		// データのセーブ
		SaveData.Instance.Save();
	}
}