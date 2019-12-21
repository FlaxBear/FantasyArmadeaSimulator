using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    <summary>
    各カードの情報を格納しているクラス
    </summary>
*/
public class CardModel
{
    public string id;               // カードID(内部処理用)
    public string cardName;         // カード名称(表示用)
    public string characterName;    // キャラクター名称(内部処理用)
    public int cost;                // コスト(表示＆内部処理用)
    public int cp;                  // CP(表示＆内部処理用)
    public short type;              // タイプ(内部処理用)
    public string attribute;        // 属性(表示用)
    public short attributeNumber;   // 属性番号(内部処理用)
    public string race;             // 種族(表示用)
    public short raceNumber;        // 種族番号(内部処理用)
    public short effectType;        // 効果タイプ(内部処理用)

    public string effect;           // 効果内容(内部処理用)
    public Sprite image;            // 画像データ(表示用)

    /// <summary>コストラクタ:カード情報をエンティティから呼び出し各メンバ変数に設定する</summary>
    /// <param name="cardID">カードID(エンティティ名)</param>
    public CardModel(string cardID)
    {
        // エンティティ呼び出し
        CardEntity cardEntity = Resources.Load<CardEntity>("CardList/" + cardID);
        id = cardEntity.id;
        cardName = cardEntity.cardName;
        characterName = cardEntity.characterName;
        cost = cardEntity.cost;
        cp = cardEntity.cp;
        type = cardEntity.typeNumber;
        attribute = cardEntity.attribute;
        attributeNumber = cardEntity.attributeNumber;
        race = cardEntity.race;
        raceNumber = cardEntity.raceNumber;
        effectType = cardEntity.effectType;
        effect = cardEntity.effect;
        image = cardEntity.image;
    }

    /// <summary>CPの増減を反映させる</summary>
    /// <param name="changeCp">増減CP値</param>
    public void changeCP(int changeCp)
    {
        cp += changeCp;
        if(cp <= 0) {
            // 計算結果が0以下ならば、0に修正
            cp = 0;
        }
    }
}
