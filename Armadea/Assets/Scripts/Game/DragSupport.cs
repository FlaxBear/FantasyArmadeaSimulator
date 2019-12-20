using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>サポートエリアにドラッグされた際に実行されるクラス</summary>
public class DragSupport : MonoBehaviour, IDropHandler
{
    /// <summary>サポートエリアにドラッグされた際に実行される関数</summary>
    /// <param name="eventData">イベントデータ(固定)</param>
    public void OnDrop(PointerEventData eventData)
    {
         // 手札からサポートエリアに移動するために必要な変数
        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>();

        // サポートエリアに4枚以下の場合と4枚の場合で処理が違うため分けている
        if(this.transform.GetComponentsInChildren<CardController>().Length < 4) {
            // 4枚以下の場合
            // このターンで既にサポートエリアにカードセットしているがどうかの判定
            if(GameManager.instance.supportSetCardCheck) {
                if(card != null) {
                    // 手札から手札に送られることを防ぐ
                    if(card.defaultParent != this.transform) {
                        card.defaultParent = this.transform;
                        // カードをセットすればデッキから1枚引く
                        GameManager.instance.playerOneDraw();
                        GameManager.instance.playerDeckCountRefresh();
                        // このターンはこれ以上カードをセット出来ないようにする
                        GameManager.instance.supportSetCardCheck = false;
                    }
                }
            }
        } else {
            // 4枚の場合
            // このターンで既にサポートエリアにカードセットしているがどうかの判定
            if(GameManager.instance.supportSetCardCheck) {
                if(card != null) {
                    // 手札から手札に送られることを防ぐ
                    if(card.defaultParent != this.transform) {
                        // サポートエリアから1枚選択して捨て場に送る(削除)機能
                        card.defaultParent = this.transform;
                        GameManager.instance.playerOneDraw();
                        GameManager.instance.playerDeckCountRefresh();
                        // このターンはこれ以上カードをセット出来ないようにする
                        GameManager.instance.supportSetCardCheck = false;
                    }
                }
            }
        }
    }
}
