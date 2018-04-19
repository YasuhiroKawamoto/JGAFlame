using UnityEngine;
using System.Collections;

namespace Util.Trello
{
    // Trelloのカードクラス
    public class TrelloCard
    {

        public string name = "";
        public string desc = "";
        public string due = "null";
        public string idList = "";
        public string urlSource = "null";
        public byte[] fileSource = null;
        public string fileName = null;
        // コンストラクタ
        public TrelloCard()
        {

        }
    }
}
