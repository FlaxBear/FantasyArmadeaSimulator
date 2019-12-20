using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>ポイント置き場を操作するコントローラー</summary>
public class PointCountController : MonoBehaviour
{
    /// <summary>ポイント置き場のテキストを更新するための処理</summary>
    /// <param name="pointCount">変更するテキストオブジェクト</param>
    /// <param name="point">表示するポイント</param>
    public void pointRefresh(Text pointCount, int point)
    {
        pointCount.text = "Point : " + point;
    }
}
