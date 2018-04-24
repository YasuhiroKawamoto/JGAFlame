using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using System;
using UnityEngine.Networking;

// トレロに投稿するクラス
namespace Util.Trello
{
    public class Trello
    {
        // Trello から発行されたToken
        private string _token = string.Empty;

        // Trello から発行されたKey
        private string _key = string.Empty;

        // ボード
        private List<object> _boards;

        // ボードのリスト
        private List<object> _lists;

        // URL関連
        private const string _memberBaseUrl = "https://api.trello.com/1/members/me";
        private const string _boardBaseUrl = "https://api.trello.com/1/boards/";
        private const string _cardBaseUrl = "https://api.trello.com/1/cards/";

        // 現在のボードID
        private string _currentBoardId = string.Empty;

        // 現在のリストID
        private string _currentListId = string.Empty;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="key">トレロから発行されたキー</param>
        /// <param name="token">トレロから発行されたトークン</param>
        public Trello(string key, string token)
        {
            this._key = key;
            this._token = token;
        }

        /// <summary>
        /// UnityWebRequestがエラーを返したかどうかをチェック。
        /// </summary>
        /// <param name="errorMessage">エラーメッセージ</param>
        /// <param name="www">リクエストするオブジェクト</param>
        private void CheckWebRequestStatus(string errorMessage, UnityWebRequest www)
        {
            if (www.isNetworkError || www.isHttpError)
            {
                throw new TrelloException(errorMessage + ": " + www.error + "(" + www.downloadHandler.text + ")");
            }
        }

        /// <summary>
        /// ユーザ用の使用可能なボードのリストをダウンロード。
        /// </summary>
        /// <returns>ボードのJsonリスト</returns>
        public List<object> PopulateBoards()
        {
            _boards = null;
            var www = UnityWebRequest.Get(string.Format("{0}?key={1}&token={2}&boards=all", _memberBaseUrl, _key, _token));
            www.chunkedTransfer = false;

            var operation = www.SendWebRequest();

            // 応答待ち
            while (!operation.isDone)
            {
                CheckWebRequestStatus("Trelloサーバーへの接続に失敗しました。", www);
            }

            var dict = Json.Deserialize(www.downloadHandler.text) as Dictionary<string, object>;

            Debug.Log(www.downloadHandler.text);

            _boards = (List<object>)dict["boards"];
            return _boards;
        }

        /// <summary>
        /// リストを検索するための現在のボードを設定
        /// </summary>
        /// <param name="name">ボード名</param>
        public void SetCurrentBoard(string name)
        {
            if (_boards == null)
            {
                throw new TrelloException("ボードのリストが存在しないので、選択することはできません。");
            }

            for (int i = 0; i < _boards.Count; i++)
            {
                var board = (Dictionary<string, object>)_boards[i];
                if ((string)board["name"] == name)
                {
                    _currentBoardId = (string)board["id"];
                    Debug.Log(_currentBoardId);
                    return;
                }
            }

            _currentBoardId = "";
            throw new TrelloException("ボードが見つかりませんでした。");
        }

        /// <summary>
        /// ボードリストを取得ののちキャッシュ
        /// </summary>
        /// <returns>A parsed JSON list of lists.</returns>
        public List<object> PopulateLists()
        {
            _lists = null;

            if (_currentBoardId == null)
            {
                throw new TrelloException("リストの取得に失敗しました。ボードを選択してください。");
            }

            var www = UnityWebRequest.Get(string.Format("{0}{1}?key={2}&token={3}&lists=all", _boardBaseUrl, _currentBoardId, _key, _token));
            www.chunkedTransfer = false;

            var operation = www.SendWebRequest();

            // 応答待ち
            while (!operation.isDone)
            {
                CheckWebRequestStatus("Trelloサーバーへの接続に失敗しました。", www);
            }

            var dict = Json.Deserialize(www.downloadHandler.text) as Dictionary<string, object>;

            Debug.Log(www.downloadHandler.text);

            _lists = (List<object>)dict["lists"];
            return _lists;
        }

        /// <summary>
        /// 現在のリストを取得してカードをアップロード
        /// </summary>
        /// <param name="name">リスト名</param>
        public void SetCurrentList(string name)
        {
            if (_lists == null)
            {
                throw new TrelloException("ボードを取得していないのでリストを取得できません。");
            }

            for (int i = 0; i < _lists.Count; i++)
            {
                var list = (Dictionary<string, object>)_lists[i];
                if ((string)list["name"] == name)
                {
                    _currentListId = (string)list["id"];
                    Debug.Log(_currentListId);
                    return;
                }
            }

            _currentListId = "";
            throw new TrelloException("No such list found.");
        }

        /// <summary>
        /// 新規のカードを作成
        /// </summary>
        /// <returns>カードオブジェクト</returns>
        public TrelloCard NewCard()
        {
            var card = new TrelloCard();
            return card;
        }

        /// <summary>
        /// 例外オブジェクトからカードを作成してTrelloサーバーにアップロード。
        /// </summary>
        /// <returns>例外オブジェクト</returns>
        /// <param name="e">E.</param>
        public TrelloCard UploadExceptionCard(Exception e)
        {
            TrelloCard card = new TrelloCard();
            card.name = e.GetType().ToString();
            card.due = DateTime.Now.ToString();
            card.desc = e.Message;

            return UploadCard(card);
        }

        /// <summary>
        /// カードをTrelloサーバーにアップロード。
        /// </summary>
        /// <returns>カードオブジェクト</returns>
        /// <param name="card">カード</param>
        public TrelloCard UploadCard(TrelloCard card)
        {
            WWWForm post = new WWWForm();
            post.AddField("name", card.name);
            post.AddField("desc", card.desc);
            post.AddField("due", card.due);
            post.AddField("idList", _currentListId);
            post.AddField("urlSource", card.urlSource);

            if (card.fileSource != null && card.fileName != null)
            {
                post.AddBinaryData("fileSource", card.fileSource, card.fileName);
            }

            var www = UnityWebRequest.Post(string.Format("{0}?key={1}&token={2}", _cardBaseUrl, _key, _token), post);
            www.chunkedTransfer = false;

            var operation = www.SendWebRequest();

            // 応答待ち
            while (!operation.isDone)
            {
                CheckWebRequestStatus("Trelloカードのアップロードに失敗しました。", www);
            }
            Debug.Log("Trello card sent!\nResponse " + www.responseCode);

            return card;
        }
    }
}