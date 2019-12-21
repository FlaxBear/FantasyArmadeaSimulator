using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>相手(エネミー)の操作を行っているクラス</summary>
public class EnemyController
{
    DeckController deckController;                  // デッキコントローラー

    public EnemyController() {
        deckController = new DeckController();
    }

    /// <summary>メインフェーズ時の相手の処理</summary>
    /// <param name="handTransform">手札のオブジェクト</param>
    /// <param name="supportTransform">サポートエリアのオブジェクト</param>
    /// <param name="deck">デッキリスト</param>
    /// <param name="cardPrefab">カードプレハブ</param>
    /// <param name="deckCount">デッキ枚数を表示するテキストオブジェクト</param>
    public void enemyMainPhase(Transform handTransform, Transform supportTransform, List<string> deck, CardController cardPrefab, Text deckCount)
    {
        Debug.Log("相手の行動");
        // 手札の一番最初のカードを出すようにしている
        CardController[] cardList = handTransform.GetComponentsInChildren<CardController>();
        CardController card = cardList[0];
        // カードを移動
        card.transform.SetParent(supportTransform);
        // ドロー&デッキカウント更新
        deckController.giveCardToHand(deck, handTransform, cardPrefab);
        deckController.deckCountRefresh(deckCount, deck);
    }

    /// <summary>バトルフェーズの相手の処理</summary>
    /// <param name="handTransform">手札のオブジェクト</param>
    /// <param name="mainTransform">メインエリアのオブジェクト</param>
    /// <param name="deck">デッキリスト</param>
    /// <param name="deckCount">デッキ枚数を表示するテキストオブジェクト</param>
    public void enemyBattlePhaseCharSet(Transform handTransform, Transform mainTransform, List<string> deck, Text deckCount)
    {
        Debug.Log("相手の行動");
        CardController[] cardList = handTransform.GetComponentsInChildren<CardController>();
        CardController card = cardList[0];
        // カードを移動
        card.transform.SetParent(mainTransform);
        deckController.costPay(deck, card.model.cost, deckCount);
    }

    
    /// <summary>相手の艶技を出す処理</summary>
    /// <returns>出した場合は,true 出さない場合は,false</returns>
    public bool enemyBattlePhaseEngi()
    {
        Debug.Log("相手の艶技行動");
        return false;
    }
}
