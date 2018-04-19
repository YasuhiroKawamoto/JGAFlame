using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.Trello;
public class TrelloSample : MonoBehaviour
{

    private IEnumerator Start()
    {
        var manager = TrelloManager.Instance;
        yield return new WaitUntil(() => !manager.IsLoading);

        // カードの作成
        var card = manager.NewCard();
        card.name = "a";
        card.desc = "";

        manager.SendCardScreenShot(card);
    }
}
