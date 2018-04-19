using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Util.Trello
{
    // Trello関連の管理クラス
    public class TrelloManager : SingletonMonoBehaviour<TrelloManager>
    {
        // 開発者用Key
        [SerializeField]
        private string _key = string.Empty;

        // 開発者用Token
        [SerializeField]
        private string _token = string.Empty;

        // 投稿するボード名
        [SerializeField]
        private string _boardName = string.Empty;

        // 投稿するリスト名
        [SerializeField]
        private string _listName = string.Empty;

        // スクリーンショットの画像
        private byte[] file = null;

        // Trello のAPIクラス
        Trello _trello = null;

        // 読み込みフラグ
        public bool IsLoading { get; set; }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns></returns>
        private void Start()
        {
            // API用クラス作成
            _trello = new Trello(_key, _token);
        }

        /// <summary>
        /// 新規のカードを作成
        /// </summary>
        /// <returns>カードオブジェクト</returns>
        public TrelloCard NewCard()
        {
            return _trello.NewCard();
        }

        /// <summary>
        /// Trelloカードを送信
        /// </summary>
        /// <param name="card">送信するカード</param>
        /// <returns>作成したかしていないか</returns> 
        public bool SendCard(TrelloCard card)
        {
            if (IsLoading)
            {
                return false;
            }

            // 送信開始
            StartCoroutine(SendCardInternal(card, _boardName, _listName));

            return true;
        }

        /// <summary>
        /// Trelloカードを送信
        /// </summary>
        /// <param name="card">送信するカード</param>
        /// <returns>作成したかしていないか</returns> 
        public bool SendCardScreenShot(TrelloCard card)
        {
            if (IsLoading)
            {
                return false;
            }

            // 送信開始
            StartCoroutine(SendCardScreenShotInternal(card));

            return true;
        }

        /// <summary>
        /// スクリーンショットのカードを送信
        /// </summary>
        /// <returns></returns>
        private IEnumerator SendCardScreenShotInternal(TrelloCard card)
        {
            // JPGの作成
            StartCoroutine(UploadJPG());

            // テクスチャ待ち
            while (file == null)
            {
                yield return null;
            }

            // カードにファイルの設定
            card.fileSource = file;
            card.fileName = DateTime.UtcNow.ToString() + ".jpg";

            // カードの送信
            yield return StartCoroutine(SendCardInternal(card, _boardName, _listName));
        }

        /// <summary>
        /// カードの送信
        /// </summary>
        /// <param name="card">送信するカード</param>
        /// <param name="list">リストの名前</param>
        /// <param name="board">ボードの名前</param>
        /// <returns></returns>
        private IEnumerator SendCardInternal(TrelloCard card, string board, string list)
        {
            // 読み込み開始
            IsLoading = true;

            // ボードの設定
            yield return _trello.PopulateBoards();
            _trello.SetCurrentBoard(board);

            // リストの設定
            yield return _trello.PopulateLists();
            _trello.SetCurrentList(list);

            // カードの送信
            yield return _trello.UploadCard(card);

            // 読み込み終了
            IsLoading = false;
        }

        /// <summary>
        /// UIで画面をキャプチャし、バイト配列を取得
        /// </summary>
        /// <returns>Byte array of a jpg image</returns>
        private IEnumerator UploadJPG()
        {
            // すべてのレンダリングが完了した後にのみ画面を読む
            yield return new WaitForEndOfFrame();

            // 画面のサイズ、RGB24形式のテクスチャを作成する
            int width = Screen.width;
            int height = Screen.height;
            Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

            // 画面の内容をテクスチャに読み込む
            tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            tex.Apply();

            // テクスチャをJPGにエンコードする
            file = tex.EncodeToJPG();

            // テクスチャ削除
            Destroy(tex);
        }
    }
}