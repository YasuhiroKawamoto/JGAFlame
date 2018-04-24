using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Util.Save
{
	/// <summary>
	/// 継承したクラスをAES複合化したバイナリで本体にセーブ可能にする
	/// </summary>
	abstract public class SaveSingleton<T> where T : SaveSingleton<T>, new()
	{
		private static readonly string ENCRYPT_KEY = "c6eahbq9sjuawhvdr9kvhpsm5qv393ga";

		private static readonly int ENCRYPT_PASSWORLD_COUNT = 16;

		private static readonly string PASSWORLD_CHARS = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

		private static readonly int PASSWORD_CHARS_LENGTH = PASSWORLD_CHARS.Length;

		private static readonly string _extension = ".bytes";

		private static string _fileName = typeof(T).ToString();

		// 実態
		private static T _instance;

		/// <summary>
		/// 実態を取得
		/// </summary>
		/// <returns></returns>
		public static T Instance
		{
			get
			{
				if (null == _instance)
				{
					_instance = new T();
				}
				return _instance;
			}
		}

		/// <summary>
		/// 保存するファイルのpathを取得
		/// </summary>
		/// <returns>ファイルのpath</returns>
		private string GetLoadFilePath()
		{
			// パスの設定
			var savePath = Application.persistentDataPath + "/" + ENCRYPT_KEY;

			// 保存するフォルダの作成
			DirectoryUtils.SafeCreateDirectory(savePath);

			// ファイルpath作成
			var load = savePath + "/" + _fileName + _extension;
			return load;
		}

		/// <summary>
		/// データの保存
		/// </summary>
		public void Save()
		{
			if (_instance == null)
			{
				return;
			}

			// Json化
			string json = JsonUtility.ToJson(_instance);

			// 暗号化
			string iv;
			string base64;
			EncryptAesBase64(json, out iv, out base64);

			// ログ
			Debug.Log("[Encrypt]json:" + json);
			Debug.Log("[Encrypt]base64:" + base64);

			// 保存
			byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
			byte[] base64Bytes = Encoding.UTF8.GetBytes(base64);

			using (FileStream fs = new FileStream(GetLoadFilePath(), FileMode.Create, FileAccess.Write))
			{
				using (BinaryWriter bw = new BinaryWriter(fs))
				{
					bw.Write(ivBytes.Length);
					bw.Write(ivBytes);

					bw.Write(base64Bytes.Length);
					bw.Write(base64Bytes);
				}
			}
		}

		/// <summary>
		/// データの読み込み
		/// </summary>
		/// <returns>読み込みの結果 true の場合成功</returns>
		public bool Load()
		{
			// path　の取得
			var loadFilePath = GetLoadFilePath();

			// ファイルが存在しない場合は読み込み失敗
			if (File.Exists(loadFilePath) == false)
			{
				return false;
			}

			// 読み込み
			byte[] ivBytes = null;
			byte[] base64Bytes = null;
			using (FileStream fs = new FileStream(loadFilePath, FileMode.Open, FileAccess.Read))
			{
				using (BinaryReader br = new BinaryReader(fs))
				{
					int length = br.ReadInt32();
					ivBytes = br.ReadBytes(length);

					length = br.ReadInt32();
					base64Bytes = br.ReadBytes(length);
				}
			}

			// 複合化
			string json;
			string iv = Encoding.UTF8.GetString(ivBytes);
			string base64 = Encoding.UTF8.GetString(base64Bytes);
			DecryptAesBase64(base64, iv, out json);

			// ログ
			Debug.Log("[Decrypt]json:" + json);

			// セーブデータ復元
			var obj = JsonUtility.FromJson<T>(json);
			// 復元出ない場合は失敗
			if (obj == null)
			{
				Debug.LogFormat("{0}の読み込み失敗", typeof(T).ToString());
				return false;
			}
			Debug.LogFormat("{0}の読み込み成功", typeof(T).ToString());

			_instance = obj;

			return true;
		}

		/// <summary>
		/// 初期化
		/// </summary>
		public void Reset()
		{
			_instance = null;
		}

		/// <summary>
		/// !!! セーブデータの削除 !!!
		/// !!!!! 呼ぶとき注意 !!!!!!!
		/// </summary>
		public void Delete()
		{
			// path 取得
			var load = GetLoadFilePath();

			// ファイルが存在するか確認
			if (File.Exists(load))
			{
				// 削除
				File.Delete(load);
			}
		}

		/// <summary>
		/// AES暗号化(Base64形式)
		/// </summary>
		public static void EncryptAesBase64(string json, out string iv, out string base64)
		{
			byte[] src = Encoding.UTF8.GetBytes(json);
			byte[] dst;
			EncryptAes(src, out iv, out dst);
			base64 = Convert.ToBase64String(dst);
		}

		/// <summary>
		/// AES複合化(Base64形式)
		/// </summary>
		public static void DecryptAesBase64(string base64, string iv, out string json)
		{
			byte[] src = Convert.FromBase64String(base64);
			byte[] dst;
			DecryptAes(src, iv, out dst);
			json = Encoding.UTF8.GetString(dst).Trim('\0');
		}

		/// <summary>
		/// AES暗号化
		/// </summary>
		public static void EncryptAes(byte[] src, out string iv, out byte[] dst)
		{
			iv = CreatePassword(ENCRYPT_PASSWORLD_COUNT);
			dst = null;
			using (RijndaelManaged rijndael = new RijndaelManaged())
			{
				rijndael.Padding = PaddingMode.PKCS7;
				rijndael.Mode = CipherMode.CBC;
				rijndael.KeySize = 256;
				rijndael.BlockSize = 128;

				byte[] key = Encoding.UTF8.GetBytes(ENCRYPT_KEY);
				byte[] vec = Encoding.UTF8.GetBytes(iv);

				using (ICryptoTransform encryptor = rijndael.CreateEncryptor(key, vec))
				using (MemoryStream ms = new MemoryStream())
				using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
				{
					cs.Write(src, 0, src.Length);
					cs.FlushFinalBlock();
					dst = ms.ToArray();
				}
			}
		}

		/// <summary>
		/// AES複合化
		/// </summary>
		public static void DecryptAes(byte[] src, string iv, out byte[] dst)
		{
			dst = new byte[src.Length];
			using (RijndaelManaged rijndael = new RijndaelManaged())
			{
				rijndael.Padding = PaddingMode.PKCS7;
				rijndael.Mode = CipherMode.CBC;
				rijndael.KeySize = 256;
				rijndael.BlockSize = 128;

				byte[] key = Encoding.UTF8.GetBytes(ENCRYPT_KEY);
				byte[] vec = Encoding.UTF8.GetBytes(iv);

				using (ICryptoTransform decryptor = rijndael.CreateDecryptor(key, vec))
				using (MemoryStream ms = new MemoryStream(src))
				using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
				{
					cs.Read(dst, 0, dst.Length);
				}
			}
		}

		/// <summary>
		/// パスワード生成
		/// </summary>
		/// <param name="count">文字列数</param>
		/// <returns>パスワード</returns>
		public static string CreatePassword(int count)
		{
			StringBuilder sb = new StringBuilder(count);
			for (int i = count - 1; i >= 0; i--)
			{
				char c = PASSWORLD_CHARS[UnityEngine.Random.Range(0, PASSWORD_CHARS_LENGTH)];
				sb.Append(c);
			}
			return sb.ToString();
		}
	}
}