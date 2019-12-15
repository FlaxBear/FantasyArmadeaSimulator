using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// カードデータ生成用エンティティ
[CreateAssetMenu(fileName="CardEntity", menuName="Create Card Entity")]
public class CardEntity : ScriptableObject
{
    public string id;               // カードID(内部処理用)
    public string cardName;         // カード名称(表示用)
    public string characterName;    // キャラクター名称(内部処理用)
    public int cost;                // コスト(表示＆内部処理用)
    public int cp;                  // CP(表示＆内部処理用)
    public short typeNumber;        // タイプ(内部処理用)
    public string attribute;        // 属性(表示用)
    public short attributeNumber;   // 属性番号(内部処理用)
    public string race;             // 種族(表示用)
    public short raceNumber;        // 種族番号(内部処理用)
    public short effectType;        // 効果タイプ(内部処理用)
    public Sprite image;            // 画像データ(表示用)
}
