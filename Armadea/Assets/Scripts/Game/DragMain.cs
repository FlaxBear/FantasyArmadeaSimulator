using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>メインエリアにドラッグされた際に実行されるクラス</summary>
public class DragMain : MonoBehaviour, IDropHandler
{
    /// <summary>メインエリアにドラッグされた際に実行される関数</summary>
    /// <param name="eventData">イベントデータ(固定)</param>
    public void OnDrop(PointerEventData eventData)
    {
        // ドラッグしているカードの情報を取得するために必要な変数
        CardController CardInfo = eventData.pointerDrag.GetComponent<CardController>();

        // 手札からメインエリアに移動するために必要な変数
        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>();
        // メインエリアに既にカードがセットされている場合にはセット出来ないようにする
        if(this.transform.GetComponentsInChildren<CardController>().Length < 1) {
            if(card != null) { 
                GameManager.instance.costPay(CardInfo.model.cost);
                card.defaultParent = this.transform; 
            }
        }
    }
}
