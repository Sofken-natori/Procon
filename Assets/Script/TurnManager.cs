using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Runtime;
using System.Runtime.Serialization;
using ServerConnector;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using Koma;

public class TurnManager : MonoBehaviour
{

    [Header("マップのCSVファイル名"), SerializeField] string MapCSV;
    [Header("最大ターン数"), SerializeField] public int MaxTurnNumber;
    [Header("生成する駒の数"), SerializeField] public int PieceNumber;
    [Header("青のターンかどうか")] public bool BlueTurn = true;
    [Header("縦のマス数")] public int BoardXMax;
    [Header("横のマス数")] public int BoardYMax;
    [Header("赤い駒のプレハブ"), SerializeField] GameObject RedBridge;
    [Header("青い駒のプレハブ"), SerializeField] GameObject BlueBridge;
    [Header("赤陣営のスコア表示"), SerializeField] Text RedScoreText;
    [Header("青陣営のスコア表示"), SerializeField] Text BlueScoreText;
    [Header("現在のターン表示"), SerializeField] Text TurnText;
    [Header("Http通信のID")]public int id = 10;
    [Header("通信を行うか"),SerializeField]bool host = true;

    [Header("陣地のスコア"), SerializeField] int AreaScore = 30;
    [Header("城壁のスコア"), SerializeField] int WallScore = 10;
    [Header("城のスコア"), SerializeField] int CastleScore = 100;

    
    [Header("青いコマの置き場所")]public GameObject BlueBridges;
    [Header("赤いコマの置き場所")]public GameObject RedBridges;


    [HideInInspector] public int BlueScore = 0;
    [HideInInspector] public int RedScore = 0;
    [HideInInspector] public bool UntapPhase = false;
    [HideInInspector] public bool TurnEnd = false;
    [HideInInspector] public List<string[]> MapData = new List<string[]>();

    Area area;
    Transform square;
    int BridgeActCount = 0;
    int BridgestandbyCount = 0;
    public int NowTurn = 0;
    public KomaIndex[] Dontroop = new KomaIndex[6];
    TextAsset csvFile;
    MatchesInfo matchesInfo;
    ServerConnector.MatchInfo matchInfo;
    PostInfo postInfo;

     void Start()
     {
        for (int N = 0; N < BlueBridges.transform.childCount; N++)
        {
            Dontroop[N] = new KomaIndex(0, 0, true);
        }
     }
    void Awake()
    {

        ConnectArea();

        csvFile = Resources.Load("CSV/" + MapCSV) as TextAsset;
        StringReader reader = new StringReader(csvFile.text);

        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            MapData.Add(line.Split(','));
        }


        CallAreaDeployer();
        PieceNumber /= 2;
        UntapPhase = true;
     
    }

    // Update is called once per frame
    void Update()
    {
        BlueScoreText.text = BlueScore.ToString();
        RedScoreText.text = RedScore.ToString();
        TurnText.text = NowTurn.ToString();

        if (BridgeActCount >= PieceNumber)
        {
            if (BlueTurn)
            {
                if (host)
                {
                    GetBridgeMoves();
                    CallPostMatchInfo(postInfo);
                }
                NowTurn++;
            }
            Debug.Log("TurnChange");
            CallAreaLeakChecker(true);
            CallAreaLeakChecker(false);
            CallSiegeAreaChecker(true);
            CallSiegeAreaChecker(false);
            CallAreaLeakReseter(true);
            CallAreaLeakReseter(false);
            CallAddScore();
            BlueTurn = !BlueTurn;
            BridgeActCount = 0;
            UntapPhase = true;
        }

        if (NowTurn >= MaxTurnNumber)
        {
            Debug.Log("GameSet");

            if (BlueScore > RedScore)
            {
                Debug.Log("BlueWin");
            }

            else if (BlueScore < RedScore)
            {
                Debug.Log("RedWin");
            }

            else
            {
                Debug.Log("Draw");
            }

            SceneManager.LoadScene("StageSelectScene");
        }
    }

    public void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Interrupt");
            SceneManager.LoadScene("StageSelectScene");
        }

        if(host)
        {
            CallMatchInfoGet(id);
            CallAreaApply(matchInfo);
            CallMatchesInfoGet(id);
        }
    }

    public void Bridgestandby()
    {
        BridgestandbyCount++;
        if (BridgestandbyCount >= PieceNumber)
        {
            UntapPhase = false;
            BridgestandbyCount = 0;
        }
    }

    public void BuildAndDestroyBridge(int x,int y, int n = -1, bool isBlue = false)
    {
        area = this.transform.GetChild(x).GetChild(y).GetComponent<Area>();
        if (area.RedWall || area.BlueWall)
        {
                area.RedWall = false;
                area.BlueWall = false;
                area.RedAreaLeak = true;
                area.BlueAreaLeak  = true;
                if(n > -1)
                {
                    if(isBlue)
                    {
                        BlueBridges.transform.GetChild(n).GetComponent<BridgeButtonManager>().ActionType = 3;
                    }
                    else
                    {
                        RedBridges.transform.GetChild(n).GetComponent<BridgeButtonManager>().ActionType = 3;
                    }
                }
        }

        else if (BlueTurn)
        {
            area.BlueWall = true;
            area.BlueAreaLeak = false;
            if(n > -1)
            {
                BlueBridges.transform.GetChild(n).GetComponent<BridgeButtonManager>().ActionType = 2;
            }
        }

        else
        {
            area.RedWall = true;
            area.RedAreaLeak = false;
            if(n > -1)
            {
                RedBridges.transform.GetChild(n).GetComponent<BridgeButtonManager>().ActionType = 2;
            }
        }

        if(n > -1)
        {
            BridgeButtonManager b = BlueBridges.transform.GetChild(n).GetComponent<BridgeButtonManager>();

            if ((b.BeforeBoardX - x == 0 && b.BeforeBoardY - y <= -1) || (b.BeforeBoardX - y == 0 && b.BeforeBoardY - x <= -1))
                b.MoveDirection = 2;

            else if ((b.BeforeBoardX - x >= 1 && b.BeforeBoardY - y == 0) || (b.BeforeBoardX - y >= 1 && b.BeforeBoardY - x == 0))
                b.MoveDirection = 4;

            else if ((b.BeforeBoardX - x == 0 && b.BeforeBoardY - y >= 1) || (b.BeforeBoardX - y == 0 && b.BeforeBoardY - x >= 1))
                b.MoveDirection = 6;

            else if ((b.BeforeBoardX - x <= -1 && b.BeforeBoardY - y == 0) || (b.BeforeBoardX - y <= -1 && b.BeforeBoardY - x == 0))
                b.MoveDirection = 8;

            else
                Debug.Log("BeforeBoardX - x:" + (b.BeforeBoardX - x) + "BeforeBoardY - y:" + (b.BeforeBoardY - y) + "\n" + n + "個目の駒が" + x + "," + y + "に建築及び破壊したかったけど建築および破壊できなかった。");
            
        }
    }

    public bool CanMove(int x, int y)
    {
        area = this.transform.GetChild(x).GetChild(y).GetComponent<Area>();
        if (area.RedWall || area.BlueWall || area.pond || area.Bridge)
        {
            return false;
        }

        else
        {
            
            return true;
        }
    }

    public Vector2 MoveBridge(int x,int y,int n = -1)
    {
        // Set the bridge position
        square = this.transform.GetChild(x).GetChild(y);
        area = square.GetComponent<Area>();
        area.Bridge = true;
        if (n > -1)
        {
            BridgeButtonManager b = BlueBridges.transform.GetChild(n).GetComponent<BridgeButtonManager>();
            BlueBridges.transform.GetChild(n).GetComponent<BridgeButtonManager>().ActionType = 1;

            // 0: 無方向, 1: 左上, 2: 上, 3: 右上, 4: 右, 5: 右下, 6: 下, 7: 左下, 8: 左
            if (b.BeforeBoardX - x <= -1 && b.BeforeBoardY - y <= -1)
                b.MoveDirection = 1;

            else if (b.BeforeBoardX - x == 0 && b.BeforeBoardY - y <= -1)
                b.MoveDirection = 2;

            else if (b.BeforeBoardX - x >= 1 && b.BeforeBoardY - y <= -1)
                b.MoveDirection = 3;


            else if (b.BeforeBoardX - x >= 1 && b.BeforeBoardY - y == 0)
                b.MoveDirection = 4;


            else if (b.BeforeBoardX - x >= 1 && b.BeforeBoardY - y <= -1)
                b.MoveDirection = 5;

            else if (b.BeforeBoardX - x == 0 && b.BeforeBoardY - y >= 1)
                b.MoveDirection = 6;

            else if (b.BeforeBoardX - x <= -1 && b.BeforeBoardY - y >= 1)
                b.MoveDirection = 7;

            else if (b.BeforeBoardX - x <= -1 && b.BeforeBoardY - y == 0)
                b.MoveDirection = 8;

            else
                Debug.Log(n + "個目の駒が" + x + "," + y + "に移動したかったけど移動できなかった。");
            

            b.BeforeBoardX = x;
            b.BeforeBoardY = y;
        }

        return square.position;
    }

    public void BridgeRest()
    {
        BridgeActCount++;
    }

    public void isBridgeReseter(int i, int j)
    {
        area = this.transform.GetChild(i).GetChild(j).GetComponent<Area>();
        area.Bridge = false;
    }

    public void CallAreaDeployer()
    {
        for (int i = 0; i < BoardYMax; i++)
        {
           
            for (int j = 0; j < BoardXMax; j++)
            {
                square = this.transform.GetChild(i).GetChild(j);
                area = square.GetComponent<Area>();
                area.AreaDeployer(MapData[i][j]);
                area.BlueBridges = BlueBridges;
                area.RedBridges = RedBridges;
            }
        }
    }

    public void ConnectArea()
    {
        for (int i = 0; i < BoardYMax; i++)
        {
            for (int j = 0; j < BoardXMax; j++)
            {
                square = this.transform.GetChild(i).GetChild(j);
                area = square.GetComponent<Area>();
                area.TM = this;
            }
        }
    }

    public void BridgeDeployer(bool isBlue, int x, int y)
    {
        if (isBlue)
        {
            GameObject Bridge = Instantiate(BlueBridge, new Vector2(x,y), Quaternion.identity, BlueBridges.transform);
            BridgeButtonManager BBM = Bridge.GetComponent<BridgeButtonManager>();
            BBM.TM = this;
            BBM.BoardX = x;
            BBM.BoardY = y;
            BBM.BridgeStartPosition();
        }

        else
        {
            GameObject Bridge = Instantiate(RedBridge, new Vector2(x,y), Quaternion.identity, RedBridges.transform);
            BridgeButtonManager RBM = Bridge.GetComponent<BridgeButtonManager>();
            RBM.TM = this;
            RBM.BoardX = x;
            RBM.BoardY = y;
            RBM.BridgeStartPosition();
        }

        PieceNumber++;
        return;
    }

    public void CallAreaLeakChecker(bool isBlue)
    {
        for (int i = 0; i < BoardXMax; i++)
        {
            for (int j = 0; j < 1; j++)
            {
                area = this.transform.GetChild(i).GetChild((BoardYMax - 1) * j).GetComponent<Area>();
                area.AreaLeak(isBlue);
            }

            for (int j = 0; j < 1; j++)
            {
                area = this.transform.GetChild((BoardXMax - 1) * j).GetChild(i).GetComponent<Area>();
                area.AreaLeak(isBlue);
            }
        }
    }

    public void CallSiegeAreaChecker(bool isBlue)
    {
        for (int i = 0; i < BoardXMax; i++)
        {
            for (int j = 0; j < BoardYMax; j++)
            {
                area = this.transform.GetChild(i).GetChild(j).GetComponent<Area>();
                area.SiegeAreaChecker(isBlue);
            }
        }
    }

    public void CallAreaLeakReseter(bool isBlue)
    {
        for (int i = 0; i < BoardXMax; i++)
        {
            for (int j = 0; j < BoardYMax; j++)
            {
                area = this.transform.GetChild(i).GetChild(j).GetComponent<Area>();
                area.AreaLeakReseter(isBlue);
            }
        }
    }

    public void CallAddScore()
    {
        BlueScore = 0;
        RedScore = 0;
        (int Blue, int Red) ScoreIndex = (0, 0);
        for (int i = 0; i < BoardXMax; i++)
        {
            for (int j = 0; j < BoardYMax; j++)
            {
                ScoreIndex = this.transform.GetChild(i).GetChild(j).GetComponent<Area>().AddScore(AreaScore, WallScore, CastleScore);

                BlueScore += ScoreIndex.Blue;
                RedScore += ScoreIndex.Red;
            }
        }
       // Debug.Log("BlueScore:" + BlueScore);
       // Debug.Log("RedScore:" + RedScore);
    }

    public void CallBridgeRester()
    {
        AB();
        KomaCalulator komaCalulator;
        komaCalulator = this.transform.GetComponent<KomaCalulator>();
        // int[,] ban = komaCalulator.AIBanState();
        // komaCalulator.AIBanState();
        // komaCalulator.AIAreaCheckBlue(ban);
        if (BlueTurn)
        {
            for (int i = 0; i < BlueBridges.transform.childCount; i++)
            {
                BlueBridges.transform.GetChild(i).GetComponent<BridgeButtonManager>().BridgeRester();
            }
        }

        else
        {
            for (int i = 0; i < RedBridges.transform.childCount; i++)
            {
                RedBridges.transform.GetChild(i).GetComponent<BridgeButtonManager>().BridgeRester();
            }
        }
    }

    public async void CallMatchInfoGet(int id)
    {
        InfoConnector infoConnector = new InfoConnector();
        matchInfo = await infoConnector.GetMatchInfo(id);
        NowTurn = matchInfo.turn;
        foreach (Log item in matchInfo.logs)
        {
            string log = "turn:" + item.turn;

            foreach (Action action in item.actions)
            {
                log += " Type:" + action.type + " Direction:" + action.dir + " Success:" + action.succeeded;
            }

            Debug.Log(log);
        }
    }

    public async void CallMatchesInfoGet(int id)
    {
        InfoConnector infoConnector = new InfoConnector();
        matchesInfo = await infoConnector.GetMatchesInfo();
    }

    public void CallAreaApply(MatchInfo info)
    {
        for (int i = 0; i < BoardYMax; i++)
        {
            for (int j = 0; j < BoardXMax; j++)
            {
                area = this.transform.GetChild(i).GetChild(j).GetComponent<Area>();
                area.AreaApply(info.board);
            }
        }
    }

    public void GetBridgeMoves()
    {
        postInfo = new PostInfo();
        postInfo.turn = matchInfo.turn + 1;
        if (postInfo.turn % 2 == 0)
        {
            postInfo.turn++;
        }
        for (int i = 0; i < BlueBridges.transform.childCount; i++)
        {
            Move BridgeMove = new Move
            {
                dir = BlueBridges.transform.GetChild(i).gameObject.GetComponent<BridgeButtonManager>().MoveDirection,
                type = BlueBridges.transform.GetChild(i).gameObject.GetComponent<BridgeButtonManager>().ActionType
            };
            postInfo.actions.Add(BridgeMove);
        }
    }

    public void CallPostMatchInfo(PostInfo info)
    {
        InfoConnector infoConnector = new InfoConnector();
        infoConnector.PostMatchInfo(id, info);
    }

    public void AB()
    {
        KomaCalulator komaCalulator;
        Monte monte;
        Alpha a;
        Alphatest alpha;
        komaCalulator = this.transform.GetComponent<KomaCalulator>();
        alpha = this.transform.GetComponent<Alphatest>();
        monte = this.transform.GetComponent<Monte>();
        a = this.transform.GetComponent<Alpha>();
        int[,] ban = komaCalulator.AIBanState();
        if (BlueTurn)
        {

            if (NowTurn >= MaxTurnNumber * 0.8)
            {
               
                for (int N = 0; N < RedBridges.transform.childCount; N++)
                {
                    monte.MonteCarloSearch(N, true);

                }

            }
            else
            {

                int[,] Ban = komaCalulator.AIBanState();
               //alpha.AlphaBeta(1, Ban, 0, true);
               for (int N = 0; N < RedBridges.transform.childCount; N++)
                {
                    alpha.AlphaBeta(1, Ban, N, true);
                }
              

            }
            
        }
        if (!BlueTurn)
        {
            //komaCalulator.AIBlueTurn = false;
            int[,] Ban = komaCalulator.AIBanState();
            for (int N = 0; N < BlueBridges.transform.childCount; N++)
            {
              //  alpha.AlphaBeta(2, Ban, N, false);
            }
        }
    }
}