using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    <summary>
    Scene Game 上で行う処理
    </summary>
*/
public class GameManager : MonoBehaviour
{

    /** メンバー変数定義 */
    /** ゲームオブジェクト */
    [SerializeField] CardController playerCardPrefab = default;   // 自分(Player)の生成するカードのオブジェクト(Inspectorに設定項目あり)
    [SerializeField] CardController enemyCardPrefab = default;    // 相手(Enemy)の生成するカードのオブジェクト(Inspectorに設定項目あり)
    [SerializeField] Transform playerHandTransform = default;     // 自分(Player)の手札部分のオブジェクト(Inspectorに設定項目あり)
    [SerializeField] Transform enemyHandTransform = default;      // 相手(Enemy)の手札部分のオブジェクト(Inspectorに設定項目あり)
    [SerializeField] Transform playerSupportTransform = default;  // 自分(Player)のサポート部分のオブジェクト(Inspectorに設定項目あり)
    [SerializeField] Transform enemySupportTransform = default;   // 相手(Enemy)のサポート部分のオブジェクト(Inspectorに設定項目あり)
    [SerializeField] Transform playerMainTransform = default;     // 自分(Player)のメイン部分のオブジェクト(Inspectorに設定項目あり)
    [SerializeField] Transform enemyMainTransform = default;      // 相手(Enemy)のメイン部分のオブジェクト(Inspectorに設定項目あり)
    [SerializeField] Transform playerEngiTransform = default;     // 自分(Player)の艶技部分のオブジェクト(Inspectorに設定項目あり)
    [SerializeField] Transform enemyEngiTransform = default;      // 相手(Enemy)の艶技部分のオブジェクト(Inspectorに設定項目あり)
    
    /** ゲーム内使用変数 */
    // 自分(Player)のデッキリスト変数
    List<string> playerDeck = new List<string>() {
        "01004",
        "01004",
        "01004",
        "01004",
        "01004",
        "01004",
        "01004",
        "01004",
        "01004",
        "01004",
        "01004",
        "01004",
        "01004",
        "01004",
        "01081",
        "01001",
        "01002",
        "01003",
        "01041",
        "01081",
        "01081",
        "01081",
        "01081",
        "01081",
        "01004",
        "01041",
        "01081",
        "01001",
        "01002",
        "01003",
        "01041",
        "01081",
        "01081",
        "01081",
        "01081",
        "01081"
    };

    // 相手(Enemy)のデッキリスト変数
    List<string> enemyDeck = new List<string>() {
        "01006",
        "01007",
        "01008",
        "01009",
        "01010",
        "01004",
        "01041",
        "01081",
        "01001",
        "01002",
        "01003",
        "01041",
        "01081",
        "01081",
        "01081",
        "01081",
        "01081"
    };
    int playerPoint = 0;
    int enemyPoint = 0;
    byte gamePhase = 0;                     // フェーズ管理用変数(
                                            // 0:メインフェイズ, 
                                            // 1:バトルフェイズ(キャラセット), 
                                            // 2:バトルフェイズ(艶技), 
                                            // 3:バトルフェイズ(姫昇天)~次ターン準備
                                            //)
    bool gameFirst = true;                  // 先攻保持プレイヤー管理用変数(true:プレイヤー, false:エネミー)
    public bool supportSetCardCheck = true; // メインフェイズ時、サポートセット制限管理用変数(true:未セット, false:セット済み)
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
        gamePhase = 0;                      // メインフェイズから開始
        gameFirst =true;                   // 先攻はプレイヤーに設定(のちに変更)
        supportSetCardCheck = true;         // サポートセットフラグ
        SetFlagChange(false, false, false); // カード移動を一時的に無効させる
        settingInitHand();                  // 各プレイヤーに手札を配る
        turnPhase();                        // フェイズへ
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
            giveCardToHand(playerDeck, playerHandTransform, playerCardPrefab);
            giveCardToHand(enemyDeck, enemyHandTransform, enemyCardPrefab);
        }
    }

    /// <summary>デッキからカードIDを元にカードモデルを生成し、手札部分のオブジェクトに加える関数</summary>
    /// <param name="deck">デッキ(カードIDを持っている入れる)</param>
    /// <param name="hand">手札の場所のオブジェクト</param>
    /// <param name="engi">カードのプレハブ</param>
    void giveCardToHand(List<string> deck, Transform hand, CardController prefab)
    {
        // デッキにカードが場合以外は、処理を行う
        if(deck.Count > 0) {
            string cardID = deck[0];
            createCard(hand, prefab, cardID);
            deck.RemoveAt(0);
        }
    }
    /// <summary>外部用の自分のデッキから手札に加える処理を行う関数</summary>
    public void playerOneDrow()
    {
        giveCardToHand(playerDeck, playerHandTransform, playerCardPrefab);
    }


    /// <summary>外部用の相手のデッキから手札に加える処理を行う関数</summary>
    public void enemyOneDrow()
    {
        giveCardToHand(enemyDeck, enemyHandTransform, enemyCardPrefab);
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
                // 2:バトルフェイズ(艶技),
                battlePhaseEngi();
                break;
            case 3:
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
            enemyMainPhase();
            SetFlagChange(true, false, false);  // サポートエリアのみ許可
        }
    }

    void enemyMainPhase()
    {
        Debug.Log("相手の行動");
        CardController[] cardList = enemyHandTransform.GetComponentsInChildren<CardController>();
        CardController card = cardList[1];
        // カードを移動
        card.transform.SetParent(enemySupportTransform);
        enemyOneDrow();
    }

    /// <summary>バトルフェーズのキャラクターセット時に行う処理</summary>
    void battlePhaseCharSet(){
        Debug.Log("バトルフェーズ:キャラクターセット");
        if(gameFirst) {
            SetFlagChange(false, true, false);  // メインエリアのみ許可
        } else {
            enemyBattlePhaseCharSet();
            SetFlagChange(false, true, false);  // メインエリアのみ許可
        }
    }

    void enemyBattlePhaseCharSet()
    {
        Debug.Log("相手の行動");
        CardController[] cardList = enemyHandTransform.GetComponentsInChildren<CardController>();
        CardController card = cardList[0];
        // カードを移動
        card.transform.SetParent(enemyMainTransform);
    }

    /// <summary>バトルフェーズの艶技発動時に行う処理</summary>
    void battlePhaseEngi(){
        Debug.Log("バトルフェーズ:艶技発動");

        // プレイヤー(自分)の艶技エリアにカードがある場合削除
        if(EngiCheckCount(playerEngiTransform) > 0) {
            CardController[] engiCardList = playerEngiTransform.GetComponentsInChildren<CardController>();
            Destroy(engiCardList[0].gameObject);
        }
        
        // プレイヤー(自分)が先攻なら許可、後攻ならエネミー(相手)の処理を行う
        if(gameFirst) {
            SetFlagChange(false, false, true);  // 艶技エリアのみ許可
        } else {
            // エネミー(相手)が出したなら許可、出していないなら次のフェーズ遷移処理
            bool engiContinueCheck = enemyBattlePhaseEngi();
            if(engiContinueCheck) {
                SetFlagChange(false, false, true);  // 艶技エリアのみ許可
            } else {
                gamePhase++;
                turnPhase();
            }
        }
    }

    bool enemyBattlePhaseEngi()
    {
        Debug.Log("相手の行動");
        return false;
    }
    
    /// <summary>バトルフェイズ(姫昇天)~次ターン準備までに行う処理</summary>
    void battlePhaseHimeToNextTurn() {
        battlePhaseHime();
        battlePhaseCale();
        endPhase();
        nextTurn();
    }

    /// <summary>バトルフェーズの姫昇天発動時に行う処理</summary>
    void battlePhaseHime(){
        Debug.Log("バトルフェーズ:姫昇天");
    }

    /// <summary>バトルフェーズのCP計算時に行う処理</summary>
    void battlePhaseCale(){
        Debug.Log("CP計算");
    }

    /// <summary>エンドフェイズに行う処理</summary>
    void endPhase(){
        Debug.Log("エンドフェイズ");
        destroyMainCard(playerMainTransform);
        destoryEngiCard(playerEngiTransform);
        destroyMainCard(enemyMainTransform);
        destoryEngiCard(enemyEngiTransform);
    }

    /// <summary>次ターン時のために行う処理</summary>
    void nextTurn(){
        gamePhase = 0;
        gameFirst = !gameFirst;
        supportSetCardCheck = true;
        turnPhase();
    }

    /* システムメソッド **/
    /// <summary>カードのオブジェクトを特定の場所に生成する関数</summary>
    /// <param name="hand">手札フィールドのオブジェクト</param>
    /// <param name="prefab">Player用プレハブかEnemy用プレハブか</param>
    /// <param name="cardID">生成するカードID</param>
    void createCard(Transform hand, CardController prefab, string cardID) 
    {
        CardController card = Instantiate(prefab, hand, false);
        card.Init(cardID);
    }

    // テスト用
    void enemySetCard()
    {
        // 相手の手札のカードリスト取得 
        CardController[] cardList = enemyHandTransform.GetComponentsInChildren<CardController>();
        // 場に出すカードを選択
        CardController card = cardList[1];
        // カードを移動
        //card.movement.SetCardTransform(enemySupportTransform);
        card.transform.SetParent(enemySupportTransform);


        // 相手の手札のカードリスト取得 
        CardController[] cardList2 = playerHandTransform.GetComponentsInChildren<CardController>();
        // 場に出すカードを選択
        CardController card2 = cardList2[1];
        // カードを移動
        //card.movement.SetCardTransform(enemySupportTransform);
        card2.transform.SetParent(playerSupportTransform);

        /* CP変更 */
        // フィールドのカードリストを取得
        CardController[] suppertCardList = playerSupportTransform.GetComponentsInChildren<CardController>();
        CardController testCard = suppertCardList[0];
        Debug.Log(testCard.model.cp);
        testCard.model.changeCP(-6000);
        Debug.Log(testCard.model.cp);
        testCard.Refresh();

        // カード破壊
        Destroy(testCard.gameObject);
    }

    /// <summary>TurnEndButton押下時の実行させる処理関数</summary>
    public void pushEndTurnEndButton() 
    {
        bool nextTurnCheck = true;
        if(gameFirst) {
            switch(gamePhase)
            {
                case 0:
                    // 0:メインフェイズ,
                    enemyMainPhase();
                    break;
                case 1:
                    // 1:バトルフェイズ(キャラセット), 
                    enemyBattlePhaseCharSet();
                    break;
                case 2:
                    // 2:バトルフェイズ(艶技),
                    // 艶技のカードがセットされているかを判断し無ければ次のフェーズへ、有れば相手のセット処理を行う
                    // 相手が出したなら継続、出さなけれな次のフェーズへ
                    if(EngiCheckCount(playerEngiTransform) == 1) {
                        nextTurnCheck = enemyBattlePhaseEngi();
                        if(nextTurnCheck) {
                            nextTurnCheck = false;
                        } else {
                            nextTurnCheck = true;
                        }
                    } else {
                        nextTurnCheck = true;
                    }
                    break;
            }
        }
        if(nextTurnCheck) {
            gamePhase++;
        }
        turnPhase();
    }
    
    // 艶技エリアの枚数を返す
    int EngiCheckCount(Transform engiTransform)
    {
        CardController[] engiCardList = engiTransform.GetComponentsInChildren<CardController>();
        return engiCardList.Length;
    }

    void destroyMainCard(Transform mainTrTransform)
    {
        CardController[] mainCardList = mainTrTransform.GetComponentsInChildren<CardController>();
        if(mainCardList.Length == 1) {
            Destroy(mainCardList[0].gameObject);
        }
    }

    void destoryEngiCard(Transform engiTransform)
    {
        CardController[] engiCardList = engiTransform.GetComponentsInChildren<CardController>();
        if(engiCardList.Length == 1) {
            Destroy(engiCardList[0].gameObject);
        }
    }
}