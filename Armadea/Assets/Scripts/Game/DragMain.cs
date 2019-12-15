using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
    メインエリアにドラッグされた際に実行されるクラス
*/
public class DragMain : MonoBehaviour, IDropHandler
{
    /// <summary>メインエリアにドラッグされた際に実行される関数</summary>
    /// <param name="eventData">イベントデータ(固定)</param>
    public void OnDrop(PointerEventData eventData)
    {
        // 手札からメインエリアに移動するために必要な変数
        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>();
        // メインエリアに既にカードがセットされている場合にはセット出来ないようにする
        if(this.transform.GetComponentsInChildren<CardController>().Length < 1) {
            if(card != null) { card.defaultParent = this.transform; }
        }
    }
}
