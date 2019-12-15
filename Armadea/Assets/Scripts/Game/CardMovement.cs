using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
    <summary>
    Card Objectをマウス操作を行うためのクラス
    </summary>
*/
public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // カードの操作で特定の場所以外でクリックが離された時に手札に戻るようになっている
    public Transform defaultParent;

    /// <summary>カードのドラッグ開始時に実行される</summary>
    /// <param name="eventData">イベントデータ(固定)</param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        // カードのドラッグ開始時
        defaultParent = transform.parent;
        transform.SetParent(defaultParent.parent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    /// <summary>カードのドラッグ中に実行される</summary>
    /// <param name="eventData">イベントデータ(固定)</param>
    public void OnDrag(PointerEventData eventData)
    {
        // カードのドラッグ機能 
        transform.position = eventData.position;
    }

    /// <summary>カードのドラッグ終了時に実行される</summary>
    /// <param name="eventData">イベントデータ(固定)</param>
    public void OnEndDrag(PointerEventData eventData)
    {
        // カードのドラッグ終了時
        transform.SetParent(defaultParent, false);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    /// <summary>ドラッグした際にカードをドラッグ先の親に変更処理</summary>
    /// <param name="parentTransform">親オブジェクト</param>
    public void SetCardTransform(Transform parentTransform)
    {
        // カードのドラッグの親を変更
        defaultParent = parentTransform;
        transform.SetParent(defaultParent);
    }
}
