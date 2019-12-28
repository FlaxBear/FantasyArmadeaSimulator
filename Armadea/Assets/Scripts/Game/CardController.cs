using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    <summary>
    カードの生成を行うクラス
    </summary>
*/
public class CardController : MonoBehaviour
{
    public CardView view;          // カード表示用クラス
    public CardModel model;        // カードデータ用クラス
    public CardMovement movement;  // カード移動用クラス

    /// <summary>インスタンス化直後に実行する</summary>
    private void Awake() {
        view = GetComponent<CardView>();
        movement = GetComponent<CardMovement>();
    }
    
    /// <summary>初期設定</summary>
    public void Init(string cardID)
    {
        // カードデータ用のクラスを生成し、表示用のクラスで表示設定を行う
        model = new CardModel(cardID);
        view.Show(model);
    }

    /// <summary>再表示</summary>
    public void Refresh()
    {
        view.Refresh(model);
    }
}
