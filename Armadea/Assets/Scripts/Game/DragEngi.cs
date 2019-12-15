using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
    艶技エリアにドラッグされた際に実行されるクラス
*/
public class DragEngi : MonoBehaviour, IDropHandler
{
    /// <summary>艶技エリアにドラッグされた際に実行される関数</summary>
    /// <param name="eventData">イベントデータ(固定)</param>
    public void OnDrop(PointerEventData eventData)
    {
        // ドラッグしているカードの情報を取得するために必要な変数
        CardController CardInfo = eventData.pointerDrag.GetComponent<CardController>();
        // 手札から艶技エリアに移動するために必要な変数
        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>();

        // 艶技エリアに既にカードがセットされている場合にはセット出来ないようにする
        if(this.transform.GetComponentsInChildren<CardController>().Length < 1)
        {
            // 艶技持ちのカード以外セット出来ないようにする
            if(CardInfo.model.effectType == 2) {
                if(card != null) { card.defaultParent = this.transform;}
                GameManager.instance.playerOneDrow();
            }
        }
    }
}
