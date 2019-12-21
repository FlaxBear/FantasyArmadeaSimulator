using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>デッキコントローラー</summary>
public class DeckController
{

    /// <summary>デッキからカードIDを元にカードモデルを生成し、手札部分のオブジェクトに加える関数</summary>
    /// <param name="deck">デッキ(カードIDを持っている入れる)</param>
    /// <param name="hand">手札の場所のオブジェクト</param>
    /// <param name="engi">カードのプレハブ</param>
    public void giveCardToHand(List<string> deck, Transform hand, CardController prefab)
    {
        // デッキにカードが0枚以外は、処理を行う
        if(deck.Count > 0) {
            string cardID = deck[0];
            GameManager.instance.createCard(hand, prefab, cardID);
            deck.RemoveAt(0);
        }
    }
    

    /// <summary>デッキ枚数表示用のテキストの更新処理</summary>
    /// <param name="deckCount">デッキ枚数表示用のテキストオブジェクト</param>
    /// <param name="deck">デッキリスト</param>
    public void deckCountRefresh(Text deckCount, List<string> deck)
    {
        deckCount.text = "Deck : " + deck.Count.ToString();
    }

    /// <summary>メインエリアセット時のコスト支払い処理</summary>
    /// <param name="deck">デッキリスト</param>
    /// <param name="cost">コスト支払い数</param>
    /// <param name="deckCount">デッキ枚数表示用のテキストオブジェクト</param>
    public void costPay(List<string> deck, int cost, Text deckCount)
    {
        for(int i = 0; i < cost; i++)
        {
            deck.RemoveAt(0);
        }
        deckCountRefresh(deckCount, deck);
    }
}
