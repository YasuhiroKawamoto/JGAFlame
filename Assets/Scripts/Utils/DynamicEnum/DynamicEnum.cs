using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

namespace Util
{
    /// <summary>
    /// 動的な列挙を作るクラス
    /// </summary>
    public class DynamicEnum
    {
        // 定義するPath
        private string outputScriptPath = string.Empty;
        // 列挙を定義するScript
        private string SCRIPT_FILE_NAME = string.Empty;

        public DynamicEnum(string fileName = "null")
        {
            SCRIPT_FILE_NAME = fileName;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool Save(List<string> list)
        {
            outputScriptPath = Directory.GetFiles(Directory.GetCurrentDirectory(), "DynamicEnum.cs", SearchOption.AllDirectories).FirstOrDefault();

            if (string.IsNullOrEmpty(outputScriptPath))
            {
                outputScriptPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets");
            }

            // ファイル名
            var file = SCRIPT_FILE_NAME + ".cs";
            // フルパス取得
            var fullPath = Path.Combine(Directory.GetParent(outputScriptPath).FullName, file);

            //Assets以下でなければエラーポップアップ
            if (!fullPath.Contains(Path.Combine(Directory.GetCurrentDirectory(), "Assets")))
            {
                Debug.LogError("スクリプトはAssets以下に配置してください : " + fullPath);
            }
            //ファイルが保存できる場合
            else
            {
                using (var writer = new StreamWriter(fullPath, false))
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var key in list)
                        sb.Append("\t" + key + ",\r\n");
                    var innerContent = sb.ToString();
                    // Scriptに書き込む形式
                    string fileContent =
                   @"
// 手動で変更しないでください

public enum " + SCRIPT_FILE_NAME + @"
{
[CONTENT]
}";

                    var content = fileContent.Replace("[CONTENT]", innerContent);
                    writer.Write(content);
                    Debug.Log(string.Format("保存しました : {0}", fullPath));

                    // 保存成功
                    return true;
                }
            }

            // 保存失敗
            return false;
        }
    }
}

#endif
