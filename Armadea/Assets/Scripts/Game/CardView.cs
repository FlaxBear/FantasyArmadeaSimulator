using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
    <summary>
    各カードの表示情報を格納しているクラス
    </summary>
*/
public class CardView : MonoBehaviour
{
    [SerializeField] Text nameText = default;           // カード名を表示するテキストオブジェクト(Inspectorに設定項目あり)
    [SerializeField] Text cpText = default;             // CPを表示するテキストオブジェクト(Inspectorに設定項目あり)
    [SerializeField] Text costText = default;           // コストを表示するテキストオブジェクト(Inspectorに設定項目あり)
    [SerializeField] Text attributeText = default;      // 属性を表示するテキストオブジェクト(Inspectorに設定項目あり)
    [SerializeField] Text raceText = default;           // 種族を表示するテキストオブジェクト(Inspectorに設定項目あり)
    [SerializeField] Image image = default;             // カード画像を表示するテキストオブジェクト(Inspectorに設定項目あり)
    [SerializeField] Image engieffect = default;        // 艶技時に出すことが可能であるかを表示するパネルオブジェクト(Inspectorに設定項目あり)

    /// <summary>表示させるための各オブジェクトの設定</summary>
    /// <param name="cardModel">カードモデル</param>
    public void Show(CardModel cardModel)
    {
        // 各種設定
        nameText.text = cardModel.cardName.Replace("]", "]\n"); // カード名(異名([])表記は二行に分けて表示する)
        cpText.text = cardModel.cp.ToString();                  // CP(int型なので文字列に変換)
        costText.text = cardModel.cost.ToString();              // コスト(int型なので文字列に変換)
        attributeText.text = cardModel.attribute;               // 属性
        raceText.text = cardModel.race;                         // 種族
        image.sprite = cardModel.image;                         // カード画像
        engieffect.enabled = false;
        // 画像が表示されない場合、タイプが分からないのでimageのカラー色でタイプを分かるようにする
        switch(cardModel.type) 
        {
            case 1:
                // 赤:情熱
                image.color = new Color32(255, 102, 102, 255);
                break;
            case 2:
                // 青:妖艶
                image.color = new Color32(102, 102, 255, 255);
                break;
            case 3:
                // 緑:清純
                image.color = new Color32(102, 255, 102, 255);
                break;
            default:
                break;
        }
    }

    /// <summary>CP再描画用</summary>
    /// <param name="cardModel">カードモデル</param>
    public void Refresh(CardModel cardModel)
    {
        cpText.text = cardModel.cp.ToString();
    }

    /// <summary>艶技使用可能を表示するエフェクトを変更する関数</summary>
    /// <param name="changeResult">変更する状態</param>
    public void setEngiEffect(bool changeResult)
    {
        engieffect.enabled = changeResult;
    }
}
