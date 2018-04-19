using UnityEngine;
using System.Collections;

namespace Util.Trello
{
    // Trelloのカードクラス
    public class TrelloCard
    {
        // タイトル名
        public string name = string.Empty;

        // 説明
        public string desc = string.Empty;

        // 日程
        public string due = "null";

        // 送付URL
        public string urlSource = "null";

        // 送付ファイル
        public byte[] fileSource = null;

        // ファイル名
        public string fileName = null;

        // コンストラクタ
        public TrelloCard()
        {

        }
    }
}
