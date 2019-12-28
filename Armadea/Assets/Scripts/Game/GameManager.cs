using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>Scene Game 上で行う処理</summary>
public class GameManager : MonoBehaviour
{

    /** メンバー変数定義 */
    /** ゲームオブジェクト */
    [SerializeField] CardController playerCardPrefab = default;     // 自分(Player)の生成するカードのオブジェクト(Inspectorに設定項目あり)
    [SerializeField] CardController enemyCardPrefab = default;      // 相手(Enemy)の生成するカードのオブジェクト(Inspectorに設定項目あり)
    [SerializeField] Transform playerHandTransform = default;       // 自分(Player)の手札部分のオブジェクト(Inspectorに設定項目あり)
    [SerializeField] Transform enemyHandTransform = default;        // 相手(Enemy)の手札部分のオブジェクト(Inspectorに設定項目あり)
    [SerializeField] Transform playerSupportTransform = default;    // 自分(Player)のサポート部分のオブジェクト(Inspectorに設定項目あり)
    [SerializeField] Transform enemySupportTransform = default;     // 相手(Enemy)のサポート部分のオブジェクト(Inspectorに設定項目あり)
    [SerializeField] Transform playerMainTransform = default;       // 自分(Player)のメイン部分のオブジェクト(Inspectorに設定項目あり)
    [SerializeField] Transform enemyMainTransform = default;        // 相手(Enemy)のメイン部分のオブジェクト(Inspectorに設定項目あり)
    [SerializeField] Transform playerEngiTransform = default;       // 自分(Player)の艶技部分のオブジェクト(Inspectorに設定項目あり)
    [SerializeField] Transform enemyEngiTransform = default;        // 相手(Enemy)の艶技部分のオブジェクト(Inspectorに設定項目あり)
    [SerializeField] Text playerDeckCount = default;                // 自分(Player)のデッキカウントテキストのオブジェクト(Inspectorに設定項目あり)
    [SerializeField] Text enemyDeckCount = default;                 // 相手(Enemy)のデッキカウントテキストのオブジェクト(Inspectorに設定項目あり)
    [SerializeField] Text playerPointCount = default;               // 自分(Player)のポイントカウントテキストとのオブジェクト(Inspectorに設定項目あり)
    [SerializeField] Text enemyPointCount = default;                // 相手(Enemy)のデポイントカウントテキストとのオブジェクト(Inspectorに設定項目あり)
    [SerializeField] Transform trashText = default;                 // 捨て場の表示用テキストのオブジェクト(Inspectorに設定項目あり)
    [SerializeField] Transform messageText = default;               // メッセージ表示用テキストのオブジェクト(Inspectorに設定項目あり)
    PointCountController pointCount = default;                      // ポイントコントローラー
    DeckController deckController = default;                        // デッキコントローラー
    EnemyController enemyController = default;                      // エネミーコントローラー
    HimeAscension himeAscension = default;                          // 姫昇天用のクラス
    EngiProcess engiProcess = default;                              // 艶技用のクラス
    /** ゲーム内使用変数 */
    // 自分(Player)のデッキリスト変数
    List<string> playerDeck = new List<string>() {
        "01001",
        "01001",
        "01001",
        "01002",
        "01002",
        "01002",
        "01003",
        "01003",
        "01003",
        "01004",
        "01004",
        "01004",
        "01005",
        "01005",
        "01005",
        "01006",
        "01006",
        "01006",
        "01007",
        "01007",
        "01007",
        "01008",
        "01008",
        "01008",
        "01009",
        "01009",
        "01009",
        "01010",
        "01010",
        "01010",
        "01011",
        "01011",
        "01011",
        "01012",
        "01012",
        "01012",
        "01015",
        "01015",
        "01016",
        "01016",
    };
    // 自分(Player)の捨て場リスト変数
    List<string> playerTrash = new List<string>() {};
    // 自分(Player))のポイント置き場
    int playerPoint = 0;
    // メインフェイズ時、サポートセット制限管理用変数(true:未セット, false:セット済み)
    bool playerSupportSetCardCheck = true;
    // 相手(Enemy)のデッキリスト変数
    List<string> enemyDeck = new List<string>() {
        "01081",
        "01081",
        "01081",
        "01082",
        "01082",
        "01082",
        "01083",
        "01083",
        "01083",
        "01084",
        "01084",
        "01084",
        "01085",
        "01085",
        "01085",
        "01086",
        "01086",
        "01086",
        "01087",
        "01087",
        "01087",
        "01088",
        "01088",
        "01088",
        "01089",
        "01089",
        "01089",
        "01090",
        "01090",
        "01090",
        "01091",
        "01091",
        "01091",
        "01092",
        "01092",
        "01092",
        "01099",
        "01099",
        "01100",
        "01100",
    };
    // 相手(Enemy)の捨て場リスト変数
    List<string> enemyTrash = new List<string>() {};
    // 相手(Enemy)のポイント置き場
    int enemyPoint = 0;
    // メインフェイズ時、サポートセット制限管理用変数(true:未セット, false:セット済み)
    bool enemySupportSetCardCheck = true;
    byte gamePhase = 0;                     // フェーズ管理用変数(
                                            // 0:メインフェイズ, 
                                            // 1:バトルフェイズ(キャラセット), 
                                            // 2:バトルフェイズ(艶技), 
                                            // 3:バトルフェイズ(姫昇天)~次ターン準備
                                            //)
    bool gameFirst = true;                  // 先攻保持プレイヤー管理用変数(true:プレイヤー, false:エネミー)
    short trashShow = 0;                    // 捨て場を表示しているか(0:表示していない, playerを表示中, enemyを表示中)
    short engiCount = 0;                    // 艶技発動回数
    public static GameManager instance;     // シングルトン化させるために必要な変数（どこからでもアクセスできるようにする)

    /** メソッド定義 */
    /** システム */
    /// <summary>このクラスがinstance変数として呼ばれた場合、このクラスを生成する※基本変更不可</summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    
    /** フローメソッド */
    /// <summary>Sceneが遷移した時に実行される関数※基本変更不可</summary>
    void Start() 
    {
        startGame();
    }

    /** 作成メソッド */

    /// <summary>ゲーム開始時の処理を行う関数</summary>
    void startGame() 
    {
        // クラスの初期設定
        pointCount = new PointCountController();
        deckController = new DeckController();
        enemyController = new EnemyController();
        himeAscension = new HimeAscension(
            playerSupportTransform, 
            enemySupportTransform, 
            playerMainTransform, 
            enemyMainTransform, 
            playerPoint, 
            enemyPoint
        );
        engiProcess = new EngiProcess(
            playerSupportTransform, 
            enemySupportTransform, 
            playerMainTransform, 
            enemyMainTransform
        );

        messageText.gameObject.SetActive(false);                                // メッセージ表示用のテキストを非表示にする
        trashText.gameObject.SetActive(false);                                  // 捨て場表示用のテキストを非表示にする
        gamePhase = 0;                                                          // メインフェイズから開始
        gameFirst = true;                                                       // 先攻はプレイヤーに設定(のちに変更)
        playerSupportSetCardCheck = true;                                             // サポートセットフラグ
        playerPoint = 0;                                                        // プレイヤー(自分)のポイント
        enemyPoint = 0;                                                         // エネミー(相手)のポイント
        SetFlagChange(false, false, false);                                     // カード移動を一時的に無効させる

        shuffleDeck(playerDeck);                                                // プレイヤー(自分)デッキをシャッフルする
        shuffleDeck(enemyDeck);                                                 // エネミー(相手)デッキをシャッフルする
        settingInitHand();                                                      // 各プレイヤーに手札を配る
        deckController.deckCountRefresh(playerDeckCount, playerDeck);           // プレイヤーのデッキカウントテキストを更新
        deckController.deckCountRefresh(enemyDeckCount, enemyDeck);             // エネミーのデッキカウントテキストを更新
        pointCount.pointRefresh(playerPointCount, playerPoint);                 // プレイヤー(自分)のポイントテキストを更新
        pointCount.pointRefresh(enemyPointCount, enemyPoint);                   // エネミー(相手)のポイントテキストを更新
        turnPhase();                                                            // フェイズへ
    }

    void shuffleDeck(List<string> deck)
    {
        // デッキをシャッフルする
        for(int i = 0; i < deck.Count; i++) {
            string temp = deck[i];
            int randomIndex = Random.Range(0, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    /// <summary>手札から場に出す際の場所の許可を管理する関数</summary>
    /// <param name="support">サポートエリアの許可</param>
    /// <param name="main">メインエリアの許可</param>
    /// <param name="engi">艶技エリアの許可</param>
    void SetFlagChange(bool support, bool main, bool engi)
    {
        // サポートエリアの許可
        if(support) {
            playerHandTransform.GetComponent<DragSupport>().enabled = true;
            playerSupportTransform.GetComponent<DragSupport>().enabled = true;
        } else {
            playerHandTransform.GetComponent<DragSupport>().enabled = false;
            playerSupportTransform.GetComponent<DragSupport>().enabled = false;
        }
        // メインエリアのセット許可
        if(main) {
            playerHandTransform.GetComponent<DragMain>().enabled = true;
            playerMainTransform.GetComponent<DragMain>().enabled = true;
        } else {
            playerHandTransform.GetComponent<DragMain>().enabled = false;
            playerMainTransform.GetComponent<DragMain>().enabled = false;
        }
        // 艶技エリアのセット許可
        if(engi) {
            playerHandTransform.GetComponent<DragEngi>().enabled = true;
            playerEngiTransform.GetComponent<DragEngi>().enabled = true;
        } else {
            playerHandTransform.GetComponent<DragEngi>().enabled = false;
            playerEngiTransform.GetComponent<DragEngi>().enabled = false;
        }
    }

    /// <summary>自分、相手の初期手札を作成する処理関数</summary>
    void settingInitHand()
    {
        // カードを4まい配る
        for(int i = 0; i < 4; i++) {
            deckController.giveCardToHand(playerDeck, playerHandTransform, playerCardPrefab);
            deckController.giveCardToHand(enemyDeck, enemyHandTransform, enemyCardPrefab);

            deckController.giveCardToHand(playerDeck, playerSupportTransform, playerCardPrefab);
        }
    }

    /// <summary>各フェーズに処理を行うための処理関数</summary>
    void turnPhase()
    {
        switch(gamePhase)
        {
            case 0:
                // 0:メインフェイズ,
                mainPhase();
                break;
            case 1:
                // 1:バトルフェイズ(キャラセット), 
                battlePhaseCharSet();
                break;
            case 2:
                // サポートエリアタイプ一致CP補正
                supportTypeCardCPChange();
                // 2:バトルフェイズ(艶技),
                battlePhaseEngi();
                break;
            case 3:
                // 艶技エフェクトのリセット
                engiEffectReset(playerHandTransform);
                engiEffectReset(enemyHandTransform);
                // 3:バトルフェイズ(姫昇天)~次ターン準備
                battlePhaseHimeToNextTurn();
                break;
            default:
                // 0 ~ 4以外はありえない
                break;
        }
    }

    /// <summary>メインフェーズ時に行う処理</summary>
    void mainPhase(){
        Debug.Log("メインフェーズ");
        if(gameFirst) {
            SetFlagChange(true, false, false);  // サポートエリアのみ許可
        } else {
            if(enemySupportSetCardCheck) {
                enemyController.enemyMainPhase(enemyHandTransform, enemySupportTransform, enemyDeck, enemyCardPrefab, enemyDeckCount);
                enemySupportSetCardCheck = false;
            }
            SetFlagChange(true, false, false);  // サポートエリアのみ許可
        }
    }

    /// <summary>バトルフェーズのキャラクターセット時に行う処理</summary>
    void battlePhaseCharSet(){
        Debug.Log("バトルフェーズ:キャラクターセット");
        if(gameFirst) {
            SetFlagChange(false, true, false);  // メインエリアのみ許可
        } else {
            enemyController.enemyBattlePhaseCharSet(enemyHandTransform, enemyMainTransform, enemyDeck, enemyDeckCount);
            SetFlagChange(false, true, false);  // メインエリアのみ許可
        }
    }

    /// <summary>メインエリアのCP補正</summary>
    void supportTypeCardCPChange()
    {
        // プレイヤー(自分)のサポートエリアのオブジェクト
        CardController[] playerSuppertCardList = playerSupportTransform.GetComponentsInChildren<CardController>();
        // エネミー(相手)のサポートエリアのオブジェクト
        CardController[] enemySuppertCardList = enemySupportTransform.GetComponentsInChildren<CardController>();
        // プレイヤー(自分)のメインエリアのオブジェクト
        CardController playerMainCard = playerMainTransform.GetComponentsInChildren<CardController>()[0];
        // エネミー(相手)のメインエリアのオブジェクト
        CardController enemyMainCard = enemyMainTransform.GetComponentsInChildren<CardController>()[0];

        // プレイヤー(自分)の処理
        int addCp = 0;
        foreach(CardController supportCard in playerSuppertCardList) {
            if(playerMainCard.model.type == supportCard.model.type) {
                addCp += 1000;
            }
        }
        playerMainCard.model.changeCP(addCp);
        playerMainCard.Refresh();

        addCp = 0;
        foreach(CardController supportCard in enemySuppertCardList) {
            if(enemyMainCard.model.type == supportCard.model.type) {
                addCp += 1000;
            }
        }
        enemyMainCard.model.changeCP(addCp);
        enemyMainCard.Refresh();
    }

    /// <summary>バトルフェーズの艶技発動時に行う処理</summary>
    void battlePhaseEngi(){
        Debug.Log("バトルフェーズ:艶技発動");

        // プレイヤー(自分)の艶技エリアにカードがある場合削除
        if(EngiCheckCount(playerEngiTransform) > 0) {
            CardController[] engiCardList = playerEngiTransform.GetComponentsInChildren<CardController>();
            playerTrash.Add(engiCardList[0].model.cardName);
            Destroy(engiCardList[0].gameObject);
        }

        // エネミー(相手)の艶技エリアにカードがある場合削除
        if(EngiCheckCount(enemyEngiTransform) > 0) {
            CardController[] engiCardList = enemyEngiTransform.GetComponentsInChildren<CardController>();
            enemyTrash.Add(engiCardList[0].model.cardName);
            Destroy(engiCardList[0].gameObject);
        }

        
        // プレイヤー(自分)が先攻なら許可、後攻ならエネミー(相手)の処理を行う
        if(gameFirst) {
            CardController[] playerHandList = playerHandTransform.GetComponentsInChildren<CardController>();
            foreach(CardController playerHandCard in playerHandList) {
                if(playerHandCard.model.effectType == 2) {
                    bool result = engiProcess.engiProcess(playerHandCard.model.effect, 1);
                    if(result){
                        playerHandCard.model.engiCheck = true;
                    } else {
                        playerHandCard.model.engiCheck = false;
                    }
                    playerHandCard.view.setEngiEffect(result);
                }
            }
            SetFlagChange(false, false, true);  // 艶技エリアのみ許可
        } else {
            // エネミー(相手)が出したなら許可、出していないなら次のフェーズ遷移処理
            // 1回だけ処理を行い、あとはpushEndTurnEndButton関数の方で処理を行う
            if(engiCount == 0) {
                // 相手は艶技を出すかどうかのチェック
                bool engiContinueCheck = enemyController.enemyBattlePhaseEngi(enemyHandTransform, enemyEngiTransform, enemyDeck, enemyCardPrefab, enemyDeckCount);
                if(engiContinueCheck) {
                    // 出した場合、継続でプレイヤーに出す処理を行う
                    engiCount++;
                    CardController[] playerHandList = playerHandTransform.GetComponentsInChildren<CardController>();
                    foreach(CardController playerHandCard in playerHandList) {
                        if(playerHandCard.model.effectType == 2) {
                            bool result = engiProcess.engiProcess(playerHandCard.model.effect, 1);
                            if(result){
                                playerHandCard.model.engiCheck = true;
                            } else {
                                playerHandCard.model.engiCheck = false;
                            }
                            playerHandCard.view.setEngiEffect(result);
                        }
                    }
                    SetFlagChange(false, false, true);  // 艶技エリアのみ許可
                } else {
                    // 出さない場合、次のフェーズへ
                    gamePhase++;
                    turnPhase();
                }
            }
        }
    }

    /// <summary>艶技使用可能エフェクトのリセットを行う処理</summary>
    /// <param name="HandTransform"></param>
    void engiEffectReset(Transform HandTransform) {
        CardController[] playerHandList = playerHandTransform.GetComponentsInChildren<CardController>();
        foreach(CardController playerHandCard in playerHandList) {
            playerHandCard.view.setEngiEffect(false);
        }
    }

    /// <summary>バトルフェイズ(姫昇天)~次ターン準備までに行う処理</summary>
    void battlePhaseHimeToNextTurn() {
        
        if(engiJudgment()) {
            battlePhaseHime();
            battlePhaseCale();
        }
        endPhase();
        nextTurn();
    }

    /// <summary>艶技の勝利判定</summary>
    /// <returns>フェイズを続行するかどうかを判断</returns>
    bool engiJudgment()
    {
        bool flag = true;  // true: 次のフェイズに移行
                            // false: このフェイズで終了

        // 艶技を使っているか
        if(engiCount != 0)
        {
            CardController playerMainCard = playerMainTransform.GetComponentsInChildren<CardController>()[0];
            CardController enemyMainCard = enemyMainTransform.GetComponentsInChildren<CardController>()[0];
            if(engiCount % 2 == 0) {
                // 偶数なら後攻が勝利している
                if(gameFirst) {
                    enemyPoint++;
                    pointCount.pointRefresh(enemyPointCount, enemyPoint);
                    deckController.giveCardToHand(playerDeck, playerHandTransform, playerCardPrefab);
                    deckController.deckCountRefresh(playerDeckCount, playerDeck);
                    Destroy(enemyMainCard.gameObject);
                    destroyMainCard(playerMainTransform, 1);
                } else {
                    playerPoint++;
                    pointCount.pointRefresh(playerPointCount, playerPoint);
                    deckController.giveCardToHand(enemyDeck, enemyHandTransform, enemyCardPrefab);
                    deckController.deckCountRefresh(enemyDeckCount, enemyDeck);
                    Destroy(playerMainCard.gameObject);
                    destroyMainCard(enemyMainTransform, 2);
                }
            } else {
                // 奇数なら先攻が勝利している
                if(gameFirst) {
                    playerPoint++;
                    pointCount.pointRefresh(playerPointCount, playerPoint);
                    deckController.giveCardToHand(enemyDeck, enemyHandTransform, enemyCardPrefab);
                    deckController.deckCountRefresh(enemyDeckCount, enemyDeck);
                    Destroy(playerMainCard.gameObject);
                    destroyMainCard(enemyMainTransform, 2);
                } else {
                    enemyPoint++;
                    pointCount.pointRefresh(enemyPointCount, enemyPoint);
                    deckController.giveCardToHand(playerDeck, playerHandTransform, playerCardPrefab);
                    deckController.deckCountRefresh(playerDeckCount, playerDeck);
                    Destroy(enemyMainCard.gameObject);
                    destroyMainCard(playerMainTransform, 1);
                }
            }
            flag = false;
            destoryEngiCard(playerEngiTransform, 1);
            destoryEngiCard(enemyEngiTransform, 2);
        }

        return flag;
    }

    /// <summary>バトルフェーズの姫昇天発動時に行う処理</summary>
    void battlePhaseHime(){
        Debug.Log("バトルフェーズ:姫昇天");
        // プレイヤー(自分)側の処理
        himeProcess(playerDeck, playerSupportTransform, playerMainTransform, enemyMainTransform, 1);
        deckController.deckCountRefresh(playerDeckCount, playerDeck);
        // エネミー(相手)側の処理
        himeProcess(enemyDeck, enemySupportTransform, enemyMainTransform, playerMainTransform, 2);
        deckController.deckCountRefresh(enemyDeckCount, enemyDeck);
    }

    /// <summary>姫昇天を発動の処理</summary>
    /// <param name="deck">デッキリスト</param>
    /// <param name="supportTransform">サポートエリアのオブジェクト</param>
    /// <param name="playerMainTransform">メインエリアのオブジェクト</param>
    /// <param name="enemyMainTransform">メインエリアのオブジェクト</param>
    /// <param name="playerNumber">誰が実行したか(1:プレイヤー(自分),2:エネミー(相手))</param>
    void himeProcess(List<string> deck, Transform supportTransform, Transform playerMainTransform, Transform enemyMainTransform, short playerNumber) {
        bool himeResult = false;
        // デッキにカードが0枚場合以外は、処理を行う
        if(deck.Count > 0) 
        {
            // デッキの1枚目をCardModelで取得
            string cardID = deck[0];
            CardModel himeCard = new CardModel(cardID);
            if(playerNumber == 1) {
                playerTrash.Add(himeCard.cardName);
            } else {
                enemyTrash.Add(himeCard.cardName);
            }
            deck.RemoveAt(0);
            if(himeCard.effectType == 1) {
                himeResult = himeCheck(supportTransform, playerMainTransform, himeCard);
                if(himeResult) {
                    himeAscension.himeAscension(himeCard.effect, playerNumber);
                    Debug.Log("プレイヤー姫昇天発動");
                }
            }
        }
    }

    /// <summary>姫昇天を発動しているかどうかの判定</summary>
    /// <param name="supportTransform">サポートエリアのオブジェクト</param>
    /// <param name="mainTrTransform">メインエリアのオブジェクト</param>
    /// <param name="himeCard">姫昇天用にデッキから引いたカード</param>
    /// <returns></returns>
    bool himeCheck(Transform supportTransform, Transform mainTrTransform, CardModel himeCard)
    {
        bool result = false;
        // サポートのカードリストを取得し、比較
        CardController[] suppertCardList = supportTransform.GetComponentsInChildren<CardController>();
        foreach(CardController supportCard in suppertCardList)
        {
            if(supportCard.model.characterName == himeCard.characterName) {
                result = true;
                break;
            }
        }
        // メインのカードを取得し、比較
        CardController[] mainCardList = mainTrTransform.GetComponentsInChildren<CardController>();
        if(mainCardList[0].model.characterName == himeCard.characterName) {
            result = true;
        }
        return result;
    }

    /// <summary>バトルフェーズのCP計算時に行う処理</summary>
    void battlePhaseCale(){
        Debug.Log("CP計算");

        CardController playerMainCard = playerMainTransform.GetComponentsInChildren<CardController>()[0];
        CardController enemyMainCard = enemyMainTransform.GetComponentsInChildren<CardController>()[0];

        Debug.Log("Player:" + playerMainCard.model.cp.ToString());
        Debug.Log("Enemy:" + enemyMainCard.model.cp.ToString());
        
        if(playerMainCard.model.cp < enemyMainCard.model.cp) {
            // 相手が勝った場合
            enemyPoint++;
            deckController.giveCardToHand(playerDeck, playerHandTransform, playerCardPrefab);
            deckController.deckCountRefresh(playerDeckCount, playerDeck);
            pointCount.pointRefresh(enemyPointCount, enemyPoint);
            Destroy(enemyMainCard.gameObject);
            destroyMainCard(playerMainTransform, 1);
            Debug.Log("相手の勝ち");
        } else if(playerMainCard.model.cp > enemyMainCard.model.cp) {
            // 自分が勝った場合
            playerPoint++;
            deckController.giveCardToHand(enemyDeck, enemyHandTransform, enemyCardPrefab);
            deckController.deckCountRefresh(enemyDeckCount, enemyDeck);
            pointCount.pointRefresh(playerPointCount, playerPoint);
            Destroy(playerMainCard.gameObject);
            destroyMainCard(enemyMainTransform, 2);
            Debug.Log("自分の勝ち");
        }　else {
            // 引き分け
            deckController.giveCardToHand(playerDeck, playerHandTransform, playerCardPrefab);
            deckController.giveCardToHand(enemyDeck, enemyHandTransform, enemyCardPrefab);
            deckController.deckCountRefresh(playerDeckCount, playerDeck);
            deckController.deckCountRefresh(enemyDeckCount, enemyDeck);
            destroyMainCard(playerMainTransform, 1);
            destroyMainCard(enemyMainTransform, 2);
            Debug.Log("引き分け");
        }
    }

    /// <summary>エンドフェイズに行う処理</summary>
    void endPhase(){
        Debug.Log("エンドフェイズ");
        int playerHandCount = playerHandTransform.GetComponentsInChildren<CardController>().Length;
        int enemyHandCount = enemyHandTransform.GetComponentsInChildren<CardController>().Length;

        if(playerHandCount == 0) {
            Debug.Log("Player Win");
        } else if(enemyHandCount == 0) {
            Debug.Log("Enemy Win");
        }
    }

    /// <summary>メインエリアのカードを削除する</summary>
    /// <param name="mainTrTransform">メインエリアのオブジェクト</param>
    /// <param name="playerNumber">誰が実行したか(1:プレイヤー(自分),2:エネミー(相手))</param>
    void destroyMainCard(Transform mainTrTransform, short playerNumber)
    {
        CardController[] mainCardList = mainTrTransform.GetComponentsInChildren<CardController>();
        if(mainCardList.Length == 1) {
            if(playerNumber == 1) {
                playerTrash.Add(mainCardList[0].model.cardName);
            } else {
                enemyTrash.Add(mainCardList[0].model.cardName);
            }
            Destroy(mainCardList[0].gameObject);
        }
    }

    /// <summary>艶技エリアのカードを削除する</summary>
    /// <param name="engiTransform">艶技エリアのオブジェクト</param>
    /// <param name="playerNumber">誰が実行したか(1:プレイヤー(自分),2:エネミー(相手))</param>
    void destoryEngiCard(Transform engiTransform, short playerNumber)
    {
        CardController[] engiCardList = engiTransform.GetComponentsInChildren<CardController>();
        if(engiCardList.Length == 1) {
            if(playerNumber == 1) {
                playerTrash.Add(engiCardList[0].model.cardName);
            } else {
                enemyTrash.Add(engiCardList[0].model.cardName);
            }
            Destroy(engiCardList[0].gameObject);
        }
    }


    /// <summary>次ターン時のために行う処理</summary>
    void nextTurn(){
        gamePhase = 0;
        gameFirst = !gameFirst;
        playerSupportSetCardCheck = true;
        enemySupportSetCardCheck = true;
        engiCount = 0;
        turnPhase();
    }

    /* システムメソッド **/

    /// <summary>TurnEndButton押下時の実行させる処理関数</summary>
    public void pushEndTurnEndButton() 
    {
        bool nextTurnCheck = true;
        // プレイヤーが先攻の処理
        switch(gamePhase)
        {
            case 0:
                // 0:メインフェイズ,
                if(playerSupportSetCardCheck == false || playerSupportTransform.GetComponentsInChildren<CardController>().Length == 4) {
                    if(gameFirst) {
                        enemyController.enemyMainPhase(enemyHandTransform, enemySupportTransform, enemyDeck, enemyCardPrefab, enemyDeckCount);
                    }
                } else {
                    nextTurnCheck = false;
                }
                break;
            case 1:
                // 1:バトルフェイズ(キャラセット), 
                CardController[] mainCardList = playerMainTransform.GetComponentsInChildren<CardController>();
                if(mainCardList.Length == 1) {
                    if(gameFirst) {
                        enemyController.enemyBattlePhaseCharSet(enemyHandTransform, enemyMainTransform, enemyDeck, enemyDeckCount);
                    }
                } else {
                    return;
                }
                break;
            case 2:
                // 2:バトルフェイズ(艶技),
                // 艶技のカードがセットされているかを判断し無ければ次のフェーズへ、有れば相手のセット処理を行う
                // 相手が出したなら継続、出さなけれな次のフェーズへ
                if(EngiCheckCount(playerEngiTransform) == 1) {
                    engiCount++;
                    nextTurnCheck = enemyController.enemyBattlePhaseEngi(enemyHandTransform, enemyEngiTransform, enemyDeck, enemyCardPrefab, enemyDeckCount);
                    if(nextTurnCheck) {
                        engiCount++;
                        nextTurnCheck = false;
                    } else {
                        nextTurnCheck = true;
                    }
                } else {
                    nextTurnCheck = true;
                }
                break;
            default:
                Debug.Log("Error");
                break;
        }

        if(nextTurnCheck) {
            gamePhase++;
        }
        turnPhase();
    }

    /// <summary>ボタンを押すと自分(Player)の捨て場一覧を表示させる関数</summary>
    public void pushPlayerTrashButton()
    {
        string ShowText = "";
        Text text = trashText.GetComponentInChildren<Text>();
        if(trashShow == 0 || trashShow == 2) {
            for(int i = 0; i < playerTrash.Count; i++) {
                if(i % 2 == 1) {
                    ShowText += "* " + playerTrash[i] + "\n";
                } else {
                    ShowText += "* " + playerTrash[i] + "\t";
                }
            }
            text.text = ShowText;
            trashText.gameObject.SetActive(true);
            trashShow = 1;
        } else {
            text.text = "";
            trashText.gameObject.SetActive(false);
            trashShow = 0;
        }
    }

    /// <summary>ボタンを押すと相手(Enemy)の捨て場一覧を表示させる関数</summary>
    public void pushEnemyTrashButton()
    {
        string ShowText = "";
        Text text = trashText.GetComponentInChildren<Text>();
        if(trashShow == 0 || trashShow == 1) {
            for(int i = 0; i < enemyTrash.Count; i++) {
                if(i % 2 == 1) {
                    ShowText += "* " + enemyTrash[i] + "\n";
                } else {
                    ShowText += "* " + enemyTrash[i] + "\t";
                }
            }
            text.text = ShowText;
            trashText.gameObject.SetActive(true);
            trashShow = 2;
        } else {
            text.text = "";
            trashText.gameObject.SetActive(false);
            trashShow = 0;
        }
    }

    /// <summary>ボタンを押せばタイトル画面に戻る処理</summary>
    public void OnBackButton() {
        SceneManager.LoadScene("Title");
    }

    /// <summary>艶技エリアの枚数を返す</summary>
    /// <param name="engiTransform">艶技エリアのオブジェクト</param>
    /// <returns>艶技エリアの枚数</returns>
    int EngiCheckCount(Transform engiTransform)
    {
        CardController[] engiCardList = engiTransform.GetComponentsInChildren<CardController>();
        return engiCardList.Length;
    }


    // シングルトン関数
    /// <summary>プレイヤー(自分)のデッキから1枚手札に加える</summary>
    public void playerOneDraw()
    {
        deckController.giveCardToHand(playerDeck, playerHandTransform, playerCardPrefab);
        deckController.deckCountRefresh(playerDeckCount, playerDeck);
    }

    /// <summary>プレイヤー(自分)のデッキのカウントを更新する</summary>
    public void playerDeckCountRefresh()
    {
        pointCount.pointRefresh(playerPointCount, playerPoint);
    }

    /// <summary>メインエリアセット時のカードコスト支払い処理</summary>
    /// <param name="cost">コスト数</param>
    public void costPay(int cost)
    {
        deckController.costPay(playerDeck, cost, playerDeckCount, 1);
        deckController.deckCountRefresh(playerDeckCount, playerDeck);
    }

    /// <summary>カードのオブジェクトを特定の場所に生成する関数</summary>
    /// <param name="hand">手札フィールドのオブジェクト</param>
    /// <param name="prefab">Player用プレハブかEnemy用プレハブか</param>
    /// <param name="cardID">生成するカードID</param>
    public void createCard(Transform hand, CardController prefab, string cardID) 
    {
        CardController card = Instantiate(prefab, hand, false);
        card.Init(cardID);
    }

    /// <summary>プレイヤーのサポートエリアカードセット処理の行ったかの状態を返す処理</summary>
    /// <returns>セット処理を行ったかの判定(true: 行っていない, false: 行っている)</returns>
    public bool supportSetCardCheckResult()
    {
        return playerSupportSetCardCheck;
    }

    /// <summary>プレイヤーのサポートエリアカードセット処理の行ったかの状態を変更する処理</summary>
    /// <param name="result">変更する状態(true: 行っていない, false: 行っている)</param>
    public void changeSupportSetCardCheck(bool result)
    {
        playerSupportSetCardCheck = result;
    }

    /// <summary>プレイヤー(自分)のポイント置き場の枚数を渡す処理</summary>
    /// <returns>プレイヤー(自分)のポイント置き場の枚数</returns>
    public int getPlayerPoint()
    {
        return playerPoint;
    }

    /// <summary>エネミー(相手)のポイント置き場の枚数を渡す処理</summary>
    /// <returns>エネミー(相手)のポイント置き場の枚数</returns>
    public int getEnemyPoint()
    {
        return enemyPoint;
    }

    /// <summary>エネミー(相手)の艶技処理確認を行う</summary>
    /// <param name="handCard">艶技カードコントローラークラスのデータ</param>
    /// <returns>使用可能かのフラグ</returns>
    public bool getEngiResult(CardController handCard)
    {
        return engiProcess.engiProcess(handCard.model.effect, 2);
    }

    /// <summary>各プレイヤーの捨て場にカードをセットする</summary>
    /// <param name="cardName">カード名</param>
    /// <param name="playerNumber">プレイヤーナンバー</param>
    public void setTrash(string cardName, short playerNumber)
    {
        if(playerNumber == 1) {
            playerTrash.Add(cardName);
        } else {
            enemyTrash.Add(cardName);
        }
    }

    /// <summary>プレイヤー(自分)のサポートエリアのオブジェクトを返す</summary>
    /// <returns>プレイヤーのサポートエリアのオブジェクト</returns>
    public Transform getPlayerSupport() {
        return playerSupportTransform;
    }

    /// <summary>エネミー(相手)のサポートエリアのオブジェクトを返す</summary>
    /// <returns>エネミーのサポートエリアのオブジェクト</returns>
    public Transform getPlayerHand() {
        return playerHandTransform;
    }

    /// <summary>エネミー(相手)のサポートカードを一枚破壊する</summary>
    /// <param name="number">削除するカードの順番番号</param>
    public void destroyEnemySupport(int number) {
        Destroy(enemySupportTransform.GetComponentsInChildren<CardController>()[number].gameObject);
    }
}