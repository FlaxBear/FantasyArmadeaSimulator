using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>姫昇天の処理を行うクラス</summary>
public class HimeAscension
{
    Transform playerSupportTransform = default;    // 自分(Player)のサポート部分のオブジェクト
    Transform enemySupportTransform = default;     // 相手(Enemy)のサポート部分のオブジェクト
    Transform playerMainTransform = default;       // 自分(Player)のメイン部分のオブジェクト
    Transform enemyMainTransform = default;        // 相手(Enemy)のメイン部分のオブジェクト
    int playerPoint = default;                      // 自分(Player)のポイント
    int enemyPoint = default;                       // 相手(Enemy)のポイント

    /// <summary>コンストラクタ用の処理</summary>
    /// <param name="playerSupport">プレイヤー(自分)のサポートエリアのオブジェクト</param>
    /// <param name="enemySupport">エネミー(相手)のサポートエリアのオブジェクト</param>
    /// <param name="playerMain">プレイヤー(自分)のメインエリアのオブジェクト</param>
    /// <param name="enemyMain">エネミー(相手)のメインエリアのオブジェクト</param>
    /// <param name="playerP">プレイヤー(自分)のポイント</param>
    /// <param name="enemyP">エネミー(相手)のポイント</param>
    public HimeAscension(Transform playerSupport, Transform enemySupport, Transform playerMain, Transform enemyMain, int playerP, int enemyP)
    {
        playerSupportTransform = playerSupport;
        enemySupportTransform = enemySupport;
        playerMainTransform = playerMain;
        enemyMainTransform = enemyMain;
        playerPoint = playerP;
        enemyPoint = enemyP;
    }

    /// <summary>姫昇天処理用のテキストを解析して処理を行う</summary>
    /// <param name="effect">姫昇天用のテキスト</param>
    /// <param name="playerNumber">誰が実行したか(1:プレイヤー(自分),2:エネミー(相手))</param>
    public void himeAscension(string effect, short playerNumber)
    {
        int result = 0;
        int listIndex = 0;
        List<string> effectProcess = new List<string>();

        // 文字列から配列にリスト型に変化
        effectProcess.AddRange(effect.Split(','));

        while(listIndex < effectProcess.Count) {
            switch(effectProcess[listIndex])
            {
                // 本効果
                case "1":
                    // プレイヤーメインキャラCP増加
                    // 引数 : N(前処理算出) or 増加値
                    listIndex++;
                    if(effectProcess[listIndex] != "N") {
                        addCp(int.Parse(effectProcess[listIndex]), playerNumber);
                    } else {
                        addCp(result, playerNumber);
                    }
                    result = 0;
                    break;
                case "2":
                    // エネミーメインキャラCP減少
                    // 引数 : N(前処理算出) or 減少値
                    listIndex++;
                    if(effectProcess[listIndex] != "N") {
                        subCp(int.Parse(effectProcess[listIndex]), playerNumber);
                    } else {
                        subCp(result, playerNumber);
                    }
                    result = 0;
                    break;
                // 補助効果
                case "11":
                    // 自分場サポート&メイン枚数
                    // 引数 : 1枚対象ごとのCP増加量
                    listIndex++;
                    result += playerSupportAndMainCount(effectProcess[listIndex], playerNumber);
                    break;
                case "12":
                    // 自分場サポート&メインタイプ一致枚数
                    // 引数 : タイプナンバー, 1枚対象ごとのCP増加量
                    listIndex += 2;
                    result += playerSupportAndMainTypeCount(effectProcess[listIndex-1], effectProcess[listIndex], playerNumber);
                    break;
                case "13":
                    // 自分場サポート&メイン属性一致枚数
                    // 引数 : 属性ナンバー, 1枚対象ごとのCP増加量
                    listIndex += 2;
                    result += playerSupportAndMainAttributeCount(effectProcess[listIndex-1], effectProcess[listIndex], playerNumber);
                    break;
                case "14":
                    // 自分場サポート&メイン種族一致枚数
                    // 引数 : 種族ナンバー, 1枚対象ごとのCP増加量
                    listIndex += 2;
                    result += playerSupportAndMainRaceCount(effectProcess[listIndex-1], effectProcess[listIndex], playerNumber);
                    break;
                case "15":
                    // 自分場ポイント枚数
                    // 引数 : 1枚対象ごとのCP増加量
                    listIndex++;
                    result += playerPointCount(effectProcess[listIndex], playerNumber);
                    break;
                case "21":
                    // 相手場サポート&メイン枚数
                    // 引数 : 1枚対象ごとのCP増加量
                    listIndex++;
                    result += enemySupportAndMainCount(effectProcess[listIndex], playerNumber);
                    break;
                case "22":
                    // 相手場サポート&メインタイプ一致枚数
                    // 引数 : タイプナンバー, 1枚対象ごとのCP増加量
                    listIndex += 2;
                    result += enemySupportAndMainTypeCount(effectProcess[listIndex-1], effectProcess[listIndex], playerNumber);
                    break;
                case "23":
                    // 相手場サポート&メイン属性一致枚数
                    // 引数 : 属性ナンバー, 1枚対象ごとのCP増加量
                    listIndex += 2;
                    result += enemySupportAndMainAttributeCount(effectProcess[listIndex-1], effectProcess[listIndex], playerNumber);
                    break;
                case "24":
                    // 相手場サポート&メイン種族一致枚数
                    // 引数 : 種族ナンバー, 1枚対象ごとのCP増加量
                    listIndex += 2;
                    result += enemySupportAndMainRaceCount(effectProcess[listIndex-1], effectProcess[listIndex], playerNumber);
                    break;
                case "25":
                    // 相手場ポイント枚数
                    // 引数 : 1枚対象ごとのCP増加量
                    listIndex++;
                    result += enemyPointCount(effectProcess[listIndex], playerNumber);
                    break;
                default:
                    return;
            }
            listIndex++;
        }
        return;
    }

    /// <summary>自分のメインエリアにいるカードのCPを増加させる処理</summary>
    /// <param name="result">CP増加値</param>
    /// <param name="playerNumber">誰が実行したか(1:プレイヤー(自分),2:エネミー(相手))</param>
    void addCp(int result, short playerNumber){
        CardController mainCard;

        if(playerNumber == 1) {
            mainCard = playerMainTransform.GetComponentsInChildren<CardController>()[0];
        } else if(playerNumber == 2) {
            mainCard = enemyMainTransform.GetComponentsInChildren<CardController>()[0];
        } else {
            return;
        }
        mainCard.model.changeCP(result);
        mainCard.Refresh();
    }

    /// <summary>相手のメインエリアにいるカードのCPを減少させる処理</summary>
    /// <param name="result">CP減少値</param>
    /// <param name="playerNumber">誰が実行したか(1:プレイヤー(自分),2:エネミー(相手))</param>
    void subCp(int result, short playerNumber){
        CardController mainCard;

        if(playerNumber == 1) {
            mainCard = enemyMainTransform.GetComponentsInChildren<CardController>()[0];
        } else if(playerNumber == 2) {
            mainCard = playerMainTransform.GetComponentsInChildren<CardController>()[0];
        } else {
            return;
        }
        mainCard.model.changeCP(-1 * result);
        mainCard.Refresh();
    }

    /// <summary>自分のサポートエリアとメインエリアのカードの数を数え、それに増加量をかけて算出する処理</summary>
    /// <param name="cp">CP増減量</param>
    /// <param name="playerNumber">誰が実行したか(1:プレイヤー(自分),2:エネミー(相手))</param>
    /// <returns>CP増減値</returns>
    int playerSupportAndMainCount(string cp, short playerNumber) {
        int count = 0;

        if(playerNumber == 1) {
            count += playerMainTransform.GetComponentsInChildren<CardController>().Length;
            count += playerSupportTransform.GetComponentsInChildren<CardController>().Length;
        } else if(playerNumber == 2) {
            count += enemyMainTransform.GetComponentsInChildren<CardController>().Length;
            count += enemySupportTransform.GetComponentsInChildren<CardController>().Length;
        } else {
            return 0;
        }
        return count * int.Parse(cp);
    }

    /// <summary>自分のサポートエリアとメインエリアからタイプを対象に数え、それに増加量をかけて算出する処理</summary>
    /// <param name="type">対象タイプ</param>
    /// <param name="cp">CP増減量</param>
    /// <param name="playerNumber">誰が実行したか(1:プレイヤー(自分),2:エネミー(相手))</param>
    /// <returns>CP増減値</returns>
    int playerSupportAndMainTypeCount(string type, string cp, short playerNumber) {
        CardController mainCard;
        CardController[] supportCards;
        int count = 0;

        if(playerNumber == 1) {
            mainCard = playerMainTransform.GetComponentsInChildren<CardController>()[0];
            supportCards = playerSupportTransform.GetComponentsInChildren<CardController>();
        } else if(playerNumber == 2) {
            mainCard = enemyMainTransform.GetComponentsInChildren<CardController>()[0];
            supportCards = enemySupportTransform.GetComponentsInChildren<CardController>();
        } else {
            return 0;
        }

        if(mainCard.model.type == int.Parse(type)){ count++; }
        foreach(CardController supportCard in supportCards) {
            if(supportCard.model.type == int.Parse(type)){ count++; }
        }

        return count * int.Parse(cp);
    }

    /// <summary>自分のサポートエリアとメインエリアから属性を対象に数え、それに増加量をかけて算出する処理</summary>
    /// <param name="attribute">対象属性</param>
    /// <param name="cp">CP増減量</param>
    /// <param name="playerNumber">誰が実行したか(1:プレイヤー(自分),2:エネミー(相手))</param>
    /// <returns>CP増減値</returns>
    int playerSupportAndMainAttributeCount(string attribute, string cp, short playerNumber) {
        CardController mainCard;
        CardController[] supportCards;
        int count = 0;

        if(playerNumber == 1) {
            mainCard = playerMainTransform.GetComponentsInChildren<CardController>()[0];
            supportCards = playerSupportTransform.GetComponentsInChildren<CardController>();
        } else if(playerNumber == 2) {
            mainCard = enemyMainTransform.GetComponentsInChildren<CardController>()[0];
            supportCards = enemySupportTransform.GetComponentsInChildren<CardController>();
        } else {
            return 0;
        }

        if(mainCard.model.attributeNumber == int.Parse(attribute)){ count++; }
        foreach(CardController supportCard in supportCards) {
            if(supportCard.model.attributeNumber == int.Parse(attribute)){ count++; }
        }

        return count * int.Parse(cp);
    }

    /// <summary>自分のサポートエリアとメインエリアから種族を対象に数え、それに増加量をかけて算出する処理</summary>
    /// <param name="race">対象種族</param>
    /// <param name="cp">CP増減量</param>
    /// <param name="playerNumber">誰が実行したか(1:プレイヤー(自分),2:エネミー(相手))</param>
    /// <returns>CP増減値</returns>
    int playerSupportAndMainRaceCount(string race, string cp, short playerNumber) { 
        CardController mainCard;
        CardController[] supportCards;
        int count = 0;

        if(playerNumber == 1) {
            mainCard = playerMainTransform.GetComponentsInChildren<CardController>()[0];
            supportCards = playerSupportTransform.GetComponentsInChildren<CardController>();
        } else if(playerNumber == 2) {
            mainCard = enemyMainTransform.GetComponentsInChildren<CardController>()[0];
            supportCards = enemySupportTransform.GetComponentsInChildren<CardController>();
        } else {
            return 0;
        }

        if(mainCard.model.raceNumber == int.Parse(race)){ count++; }
        foreach(CardController supportCard in supportCards) {
            if(supportCard.model.raceNumber == int.Parse(race)){ count++; }
        }

        return count * int.Parse(cp);
    }

    /// <summary>自分のポイントを数え、それに増加量をかけて算出する処理</summary>
    /// <param name="cp">CP増減量</param>
    /// <param name="playerNumber">誰が実行したか(1:プレイヤー(自分),2:エネミー(相手))</param>
    /// <returns>CP増減値</returns>
    int playerPointCount(string cp, short playerNumber) {
        int count = 0;
        if(playerNumber == 1) {
            count = playerPoint;
        } else if(playerNumber == 2) {
            count = enemyPoint;
        } else {
            return 0;
        }
        return count * int.Parse(cp);
    }

    /// <summary>自分のサポートエリアとメインエリアのカードの数を数え、それに増加量をかけて算出する処理</summary>
    /// <param name="cp">CP増加量</param>
    /// <param name="playerNumber">誰が実行したか(1:プレイヤー(自分),2:エネミー(相手))</param>
    /// <returns></returns>
    int enemySupportAndMainCount(string cp, short playerNumber) {
        int count = 0;

        if(playerNumber == 1) {
            count += enemyMainTransform.GetComponentsInChildren<CardController>().Length;
            count += enemySupportTransform.GetComponentsInChildren<CardController>().Length;
        } else if(playerNumber == 2) {
            count += playerMainTransform.GetComponentsInChildren<CardController>().Length;
            count += playerSupportTransform.GetComponentsInChildren<CardController>().Length;
        } else {
            return 0;
        }
        return count * int.Parse(cp);
    }

    /// <summary>相手のサポートエリアとメインエリアからタイプを対象に数え、それに増加量をかけて算出する処理</summary>
    /// <param name="type">対象タイプ</param>
    /// <param name="cp"></param>
    /// <param name="playerNumber">誰が実行したか(1:プレイヤー(自分),2:エネミー(相手))</param>
    /// <returns></returns>
    int enemySupportAndMainTypeCount(string type, string cp, short playerNumber) {
        CardController mainCard;
        CardController[] supportCards;
        int count = 0;

        if(playerNumber == 1) {
            mainCard = enemyMainTransform.GetComponentsInChildren<CardController>()[0];
            supportCards = enemySupportTransform.GetComponentsInChildren<CardController>();
        } else if(playerNumber == 2) {
            mainCard = playerMainTransform.GetComponentsInChildren<CardController>()[0];
            supportCards = playerSupportTransform.GetComponentsInChildren<CardController>();
        } else {
            return 0;
        }

        if(mainCard.model.type == int.Parse(type)){ count++; }
        foreach(CardController supportCard in supportCards) {
            if(supportCard.model.type == int.Parse(type)){ count++; }
        }

        return count * int.Parse(cp);
    }

    /// <summary>相手のサポートエリアとメインエリアから属性を対象に数え、それに増加量をかけて算出する処理</summary>
    /// <param name="attribute">対象属性</param>
    /// <param name="cp">CP増加量</param>
    /// <param name="playerNumber">誰が実行したか(1:プレイヤー(自分),2:エネミー(相手))</param>
    /// <returns></returns>
    int enemySupportAndMainAttributeCount(string attribute, string cp, short playerNumber) { 
        CardController mainCard;
        CardController[] supportCards;
        int count = 0;

        if(playerNumber == 1) {
            mainCard = enemyMainTransform.GetComponentsInChildren<CardController>()[0];
            supportCards = enemySupportTransform.GetComponentsInChildren<CardController>();
        } else if(playerNumber == 2) {
            mainCard = playerMainTransform.GetComponentsInChildren<CardController>()[0];
            supportCards = playerSupportTransform.GetComponentsInChildren<CardController>();
        } else {
            return 0;
        }

        if(mainCard.model.attributeNumber == int.Parse(attribute)){ count++; }
        foreach(CardController supportCard in supportCards) {
            if(supportCard.model.attributeNumber == int.Parse(attribute)){ count++; }
        }

        return count * int.Parse(cp);
    }

    /// <summary>相手のサポートエリアとメインエリアから種族を対象に数え、それに増加量をかけて算出する処理</summary>
    /// <param name="race">対象種族</param>
    /// <param name="cp">CP増加量</param>
    /// <param name="playerNumber">誰が実行したか(1:プレイヤー(自分),2:エネミー(相手))</param>
    /// <returns>CP増加量</returns>
    int enemySupportAndMainRaceCount(string race, string cp, short playerNumber) {
        CardController mainCard;
        CardController[] supportCards;
        int count = 0;

        if(playerNumber == 1) {
            mainCard = enemyMainTransform.GetComponentsInChildren<CardController>()[0];
            supportCards = enemySupportTransform.GetComponentsInChildren<CardController>();
        } else if(playerNumber == 2) {
            mainCard = playerMainTransform.GetComponentsInChildren<CardController>()[0];
            supportCards = playerSupportTransform.GetComponentsInChildren<CardController>();
        } else {
            return 0;
        }

        if(mainCard.model.raceNumber == int.Parse(race)){ count++; }
        foreach(CardController supportCard in supportCards) {
            if(supportCard.model.raceNumber == int.Parse(race)){ count++; }
        }

        return count * int.Parse(cp);
    }

    /// <summary>相手のポイントを数え、それに増加量をかけて算出する処理</summary>
    /// <param name="cp">CP増加量</param>
    /// <param name="playerNumber">誰が実行したか(1:プレイヤー(自分),2:エネミー(相手))</param>
    /// <returns></returns>
    int enemyPointCount(string cp, short playerNumber) {
        int count = 0;
        if(playerNumber == 1) {
            count = enemyPoint;
        } else if(playerNumber == 2) {
            count = playerPoint;
        } else {
            return 0;
        }
        return count * int.Parse(cp);
    }
}
