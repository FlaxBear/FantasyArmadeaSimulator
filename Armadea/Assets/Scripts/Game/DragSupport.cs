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
        Transform playerHand = GameManager.instance.getPlayerHand();
        Transform playerSupport = GameManager.instance.getPlayerSupport();

        // このターンで既にサポートエリアにカードセットしているがどうかの判定
        if(GameManager.instance.supportSetCardCheckResult()) {
            if(card != null) {
                if(this.transform == playerHand) {
                    // ハンド → ハンド
                    return;
                } else if(this.transform == playerSupport) {
                    // ハンド → サポート
                    if(playerSupport.GetComponentsInChildren<CardController>().Length < 4) {
                        // サポートが4枚以下
                        card.defaultParent = playerSupport;
                    } else {
                        return;
                    }
                } else {
                     // ハンド → サポートカード
                    if(playerSupport.GetComponentsInChildren<CardController>().Length < 4)
                    {
                        // サポートが4枚以下
                        card.defaultParent = playerSupport;
                    } else {
                        // サポートが4枚
                        CardMovement deleteCardArea = GetComponent<CardMovement>();
                        CardController deleteCard = GetComponent<CardController>();
                        if(deleteCard != null && deleteCardArea.defaultParent == playerSupport) {
                            GameManager.instance.setTrash(deleteCard.model.cardName, 1);
                            Destroy(deleteCard.gameObject);
                            card.defaultParent = playerSupport;
                        } else {
                            return;
                        }
                    }
                }
                // カードをセットすればデッキから1枚引く
                GameManager.instance.playerOneDraw();
                GameManager.instance.playerDeckCountRefresh();
                // このターンはこれ以上カードをセット出来ないようにする
                GameManager.instance.changeSupportSetCardCheck(false);
            }
        }
    }
}
