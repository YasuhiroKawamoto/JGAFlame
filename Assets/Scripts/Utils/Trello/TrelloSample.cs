using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Util.Trello;

public class TrelloSample : MonoBehaviour
{
    private void Start()
    {
        var manager = TrelloManager.Instance;

        // カードの作成
        var card = manager.NewCard();
        card.name = "タイトル名";
        card.desc = "説明内容";

        // 時間設定
        card.due = DateTime.Now.ToString();

        manager.SendCardScreenShot(card);
    }
}
