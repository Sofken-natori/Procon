using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Networking.Match;
using System.Runtime;
using System.Runtime.Serialization;
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
    [Header("Http通信のID")] public int id = 10;

    [Header("陣地のスコア"), SerializeField] int AreaScore = 30;
    [Header("城壁のスコア"), SerializeField] int WallScore = 10;
    [Header("城のスコア"), SerializeField] int CastleScore = 100;


    [Header("青いコマの置き場所"), SerializeField] public GameObject BlueBridges;
    [Header("赤いコマの置き場所"), SerializeField] public GameObject RedBridges;


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

        Debug.Log("PieceDeployed");
        csvFile = Resources.Load("CSV/" + MapCSV) as TextAsset;
        StringReader reader = new StringReader(csvFile.text);
        Debug.Log("CSVReaded");

        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            MapData.Add(line.Split(','));
        }

        Debug.Log("MapDataDeployed");

        CallAreaDeployer();
        Debug.Log("AreaDeployed");
        PieceNumber /= 2;
        UntapPhase = true;
        Debug.Log("----------------------------------------------------InitEnd----------------------------------------------------");
    }

    // Update is called once per frame
    void Update()
    {
        BlueScoreText.text = BlueScore.ToString();
        RedScoreText.text = RedScore.ToString();
        TurnText.text = NowTurn.ToString();

        if (BridgeActCount >= PieceNumber)
        {
            if (!BlueTurn)
            {
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("StageSelectScene");
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

    public void BuildAndDestroyBridge(int x, int y)
    {
        area = this.transform.GetChild(x).GetChild(y).GetComponent<Area>();
        if (area.RedWall || area.BlueWall)
        {
            area.RedWall = false;
            area.BlueWall = false;
            area.RedAreaLeak = true;
            area.BlueAreaLeak = true;
        }

        else if (BlueTurn)
        {
            area.BlueWall = true;
            area.BlueAreaLeak = false;
        }

        else
        {
            area.RedWall = true;
            area.RedAreaLeak = false;
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

    public Vector2 MoveBridge(int x, int y)
    {
        // Set the bridge position
        square = this.transform.GetChild(x).GetChild(y);
        area = square.GetComponent<Area>();
        area.Bridge = true;
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
            GameObject Bridge = Instantiate(BlueBridge, new Vector2(x, y), Quaternion.identity, BlueBridges.transform);
            BridgeButtonManager BBM = Bridge.GetComponent<BridgeButtonManager>();
            BBM.TM = this;
            BBM.BoardX = x;
            BBM.BoardY = y;
            BBM.BridgeStartPosition();
        }

        else
        {
            GameObject Bridge = Instantiate(RedBridge, new Vector2(x, y), Quaternion.identity, RedBridges.transform);
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
     //   int[,] ban = komaCalulator.AIBanState();
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
               
               
             
 





