using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>艶技処理を行いクラス</summary>
public class EngiProcess
{
    Transform playerSupportTransform = default;    // 自分(Player)のサポート部分のオブジェクト
    Transform enemySupportTransform = default;     // 相手(Enemy)のサポート部分のオブジェクト
    Transform playerMainTransform = default;       // 自分(Player)のメイン部分のオブジェクト
    Transform enemyMainTransform = default;        // 相手(Enemy)のメイン部分のオブジェクト

    /// <summary>コンストラクタ処理</summary>
    /// <param name="playerSupport">プレイヤー(自分)のサポートエリアのオブジェクト</param>
    /// <param name="enemySupport">エネミー(相手)のサポートエリアのオブジェクト</param>
    /// <param name="playerMain">プレイヤー(自分)のメインエリアのオブジェクト</param>
    /// <param name="enemyMain">エネミー(相手)のメインエリアのオブジェクト</param>
    /// <param name="playerP">プレイヤー(自分)のポイント</param>
    /// <param name="enemyP">エネミー(相手)のポイント</param>
    public EngiProcess(Transform playerSupport, Transform enemySupport, Transform playerMain, Transform enemyMain)
    {
        playerSupportTransform = playerSupport;
        enemySupportTransform = enemySupport;
        playerMainTransform = playerMain;
        enemyMainTransform = enemyMain;
    }

    /// <summary>艶技処理用のテキストを解析して処理を行う</summary>
    /// <param name="effect">艶技用のテキスト</param>
    /// <param name="playerNumber">誰が実行したか(1:プレイヤー(自分),2:エネミー(相手))</param>
    /// <returns></returns>
    public bool engiProcess(string effect, short playerNumber)
    {
        bool result = false;

        switch(effect) 
        {
            case "1":
                // 同じタイプの相手のキャラが3枚以上ある
                result = enemyTypeCountCheck(playerNumber);
                break;
            case "2":
                // 相手の「純潔」のキャラが2枚以上ある
                result = enemyInnocenceCountCheck(playerNumber);
                break;
            case "3":
                // 相手の「妖艶」のキャラが2枚以上ある
                result = enemyBewitchingCountCheck(playerNumber);
                break;
            case "4":
                // 相手の「情熱」のキャラが2枚以上する
                result = enemyPassionCountCheck(playerNumber);
                break;
            case "5":
                // 自分のCPより相手のCPの方が高い
                result = enemyCPComparisonCountCheck(playerNumber);
                break;
            case "6":
                // 相手のポイント置き場にキャラが3枚以上いる
                result = enemyPointCountCheck(playerNumber);
                break;
            default:
                break;
        }

        return result;
    }

    /// <summary>同じタイプの相手のキャラが3枚以上あるかを確認する処理</summary>
    /// <param name="playerNumber">誰が実行したか(1:プレイヤー(自分),2:エネミー(相手))</param>
    /// <returns>発動不可判定</returns>
    bool enemyTypeCountCheck(short playerNumber){
        CardController mainCard;
        CardController[] supportCards;
        List<int> typeList = new List<int>();
        bool result = false;

        if(playerNumber == 1) {
            mainCard = enemyMainTransform.GetComponentsInChildren<CardController>()[0];
            supportCards = enemySupportTransform.GetComponentsInChildren<CardController>();
        } else if(playerNumber == 2) {
            mainCard = playerMainTransform.GetComponentsInChildren<CardController>()[0];
            supportCards = playerSupportTransform.GetComponentsInChildren<CardController>();
        } else {
            return false;
        }

        typeList.Add(mainCard.model.type);
        foreach(CardController supportCard in supportCards) {
            typeList.Add(supportCard.model.type);
        }

        for(int i = 1; i < 4; i++) {
            int count = 0;
            foreach(int type in typeList) {
                if(type == i) {
                    count++;
                }
            }
            if(count >= 3) {
                result = true;
            }
        }

        return result; 
    }

    /// <summary>相手の「純潔」のキャラが2枚以上ある</summary>
    /// <param name="playerNumber"></param>
    /// <returns></returns>
    bool enemyInnocenceCountCheck(short playerNumber){
        CardController mainCard;
        CardController[] supportCards;
        List<int> typeList = new List<int>();
        bool result = false;
        int count = 0;

        if(playerNumber == 1) {
            mainCard = enemyMainTransform.GetComponentsInChildren<CardController>()[0];
            supportCards = enemySupportTransform.GetComponentsInChildren<CardController>();
        } else if(playerNumber == 2) {
            mainCard = playerMainTransform.GetComponentsInChildren<CardController>()[0];
            supportCards = playerSupportTransform.GetComponentsInChildren<CardController>();
        } else {
            return false;
        }

        typeList.Add(mainCard.model.type);
        foreach(CardController supportCard in supportCards) {
            typeList.Add(supportCard.model.type);
        }

        foreach(int type in typeList) {
            if(type == 3) {
                count++;
            }
        }
        if(count >= 2) {
            result = true;
        
        }
        return result;
    }

    bool enemyBewitchingCountCheck(short playerNumber){
        CardController mainCard;
        CardController[] supportCards;
        List<int> typeList = new List<int>();
        bool result = false;
        int count = 0;

        if(playerNumber == 1) {
            mainCard = enemyMainTransform.GetComponentsInChildren<CardController>()[0];
            supportCards = enemySupportTransform.GetComponentsInChildren<CardController>();
        } else if(playerNumber == 2) {
            mainCard = playerMainTransform.GetComponentsInChildren<CardController>()[0];
            supportCards = playerSupportTransform.GetComponentsInChildren<CardController>();
        } else {
            return false;
        }

        typeList.Add(mainCard.model.type);
        foreach(CardController supportCard in supportCards) {
            typeList.Add(supportCard.model.type);
        }

        foreach(int type in typeList) {
            if(type == 2) {
                count++;
            }
        }
        if(count >= 2) {
            result = true;
        
        }
        return result;
    }
    bool enemyPassionCountCheck(short playerNumber){
        CardController mainCard;
        CardController[] supportCards;
        List<int> typeList = new List<int>();
        bool result = false;
        int count = 0;

        if(playerNumber == 1) {
            mainCard = enemyMainTransform.GetComponentsInChildren<CardController>()[0];
            supportCards = enemySupportTransform.GetComponentsInChildren<CardController>();
        } else if(playerNumber == 2) {
            mainCard = playerMainTransform.GetComponentsInChildren<CardController>()[0];
            supportCards = playerSupportTransform.GetComponentsInChildren<CardController>();
        } else {
            return false;
        }

        typeList.Add(mainCard.model.type);
        foreach(CardController supportCard in supportCards) {
            typeList.Add(supportCard.model.type);
        }

        foreach(int type in typeList) {
            if(type == 1) {
                count++;
            }
        }
        if(count >= 2) {
            result = true;
        
        }
        return result;
    }

    bool enemyCPComparisonCountCheck(short playerNumber){
        CardController mainCard, enemyCard;
        bool result = false;

        if(playerNumber == 1) {
            mainCard = playerMainTransform.GetComponentsInChildren<CardController>()[0];
            enemyCard = enemyMainTransform.GetComponentsInChildren<CardController>()[0];
        } else if(playerNumber == 2) {
            mainCard = enemyMainTransform.GetComponentsInChildren<CardController>()[0];
            enemyCard = playerMainTransform.GetComponentsInChildren<CardController>()[0];
        } else {
            return false;
        }

        if(mainCard.model.cp < enemyCard.model.cp) {
            result = true;
        }
        return result;
    }
    bool enemyPointCountCheck(short playerNumber){
        int point = 0;
        bool result = false;

        if(playerNumber == 1) {
            point = GameManager.instance.getEnemyPoint();
        } else if(playerNumber == 2) {
            point = GameManager.instance.getPlayerPoint();
        } else {
            return false;
        }

        if(point >= 3) {
            result = true;
        }
        return result;
    }
}
