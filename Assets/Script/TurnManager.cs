/*
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEditorInternal;
using Koma;
using System.Reflection;

public class TurnManager : MonoBehaviour
{

    [Header("マップのCSVファイル名"),SerializeField] string MapCSV;
    [Header("最大ターン数"), SerializeField]public int MaxTurnNumber;
    [Header("生成する駒の数"), SerializeField]public int PieceNumber;
    [Header("青のターンかどうか")]public bool BlueTurn = true;
    [Header("縦のマス数")]public int BoardXMax;
    [Header("横のマス数")]public int BoardYMax;
    [Header("赤い駒のプレハブ"), SerializeField] GameObject RedBridge;
    [Header("青い駒のプレハブ"), SerializeField] GameObject BlueBridge;
    [Header("赤陣営のスコア表示"), SerializeField] Text RedScoreText;
    [Header("青陣営のスコア表示"), SerializeField] Text BlueScoreText;
    [Header("現在のターン表示"), SerializeField] Text TurnText;

    [Header("陣地のスコア"),SerializeField] int AreaScore = 30;
    [Header("城壁のスコア"),SerializeField] int WallScore = 10;
    [Header("城のスコア"),SerializeField] int CastleScore = 100;

    [HideInInspector]public int BlueScore = 0;
    [HideInInspector]public int RedScore = 0;
    [HideInInspector]public bool UntapPhase = false;
    [HideInInspector]public bool TurnEnd = false;
    [HideInInspector]public List<string[]> MapData = new List<string[]>();

    Area area;
     KomaCalulator kc;
    Monte monte;
    Alpha alpha;
    BridgeButtonManager[] bb = new BridgeButtonManager[6];
    Transform square;
    GameObject Board;
    GameObject koma;
    public int BridgeActCount = 0;
    int BridgestandbyCount = 0;
   public int NowTurn = 0;
    TextAsset csvFile;
    private void Start()
    {
        koma = GameObject.Find("Board");
        monte =  koma.GetComponent<Monte>();
        kc = koma.GetComponent<KomaCalulator>();
       alpha = koma.GetComponent<Alpha>();
    }

    void Awake()
    {

        Board = GameObject.Find("BoardBridge");
        ConnectArea();

        Debug.Log("PieceDeployed");
        csvFile = Resources.Load("CSV/" + MapCSV) as TextAsset;
        StringReader reader = new StringReader(csvFile.text);
        Debug.Log("CSVReaded");

        while(reader.Peek() != -1)
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
      
    //   Debug.Log(BridgeActCount);
        if(BridgeActCount >= PieceNumber)
        {
            if(!BlueTurn)
            {
                NowTurn++;   
            }
            if (BlueTurn)
            {
               
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

        if(NowTurn >= MaxTurnNumber)
        {
            Debug.Log("GameSet");

            if(BlueScore > RedScore)
            {
               
                Debug.Log("BlueWin");
            }

            else if(BlueScore < RedScore)
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
   
    public void Bridgestandby()
    {
        BridgestandbyCount++;
        if(BridgestandbyCount >= PieceNumber)
        {
            UntapPhase = false;
            BridgestandbyCount = 0;
       
        }
    }
  
 
    public void BuildAndDestroyBridge(int x,int y)
    {
        area = this.transform.GetChild(x).GetChild(y).GetComponent<Area>();
        if(area.RedWall || area.BlueWall)
        {
                area.RedWall = false;
                area.BlueWall = false;
                area.RedAreaLeak = true;
                area.BlueAreaLeak  = true;
        }
        
        else if(BlueTurn)
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

    public bool CanMove(int x,int y)
    {
     
        area = this.transform.GetChild(x).GetChild(y).GetComponent<Area>();
        if(area.RedWall || area.BlueWall || area.pond || area.Bridge)
        {
            return false;
        }

        else
        {
            return true;
        }
    }

    public Vector2 MoveBridge(int x,int y)
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
        for(int i = 0; i < BoardYMax; i++)
        {
            for(int j = 0; j < BoardXMax; j++)
            {
                square = this.transform.GetChild(i).GetChild(j);
                area = square.GetComponent<Area>();
                area.AreaDeployer(MapData[i][j]);
            }
        }
    }

    public void ConnectArea()
    {
        for(int i = 0; i < BoardYMax; i++)
        {
            for(int j = 0; j < BoardXMax; j++)
            {
                square = this.transform.GetChild(i).GetChild(j);
                area = square.GetComponent<Area>();
                area.TM = this;
            }
        }
    }

    public void BridgeDeployer (bool isBlue,int x, int y)
    {
        if(isBlue)
        {
            GameObject Bridge = Instantiate(BlueBridge, new Vector2(x,y), Quaternion.identity, Board.transform);
            BridgeButtonManager BBM = Bridge.GetComponent<BridgeButtonManager>();
            BBM.TM = this;
            BBM.BoardX = x;
            BBM.BoardY = y;
            BBM.BridgeStartPosition();
        }

        else
        {
            GameObject Bridge = Instantiate(RedBridge, new Vector2(x,y), Quaternion.identity, Board.transform);
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
        for(int i = 0; i < BoardXMax; i++)
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
        for(int i = 0; i < BoardXMax; i++)
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
        for(int i = 0; i < BoardXMax; i++)
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
        (int Blue,int Red) ScoreIndex = (0,0);
        for(int i = 0; i < BoardXMax; i++)
        {
            for (int j = 0; j < BoardYMax; j++)
            {
                ScoreIndex = this.transform.GetChild(i).GetChild(j).GetComponent<Area>().AddScore(AreaScore,WallScore,CastleScore);
            
                BlueScore += ScoreIndex.Blue;
                RedScore += ScoreIndex.Red;
            }
        }

        Debug.Log("BlueScore:" + BlueScore);
        Debug.Log("RedScore:" + RedScore);
    }

    public void CallBridgeRester()
    {
        for(int i = 0; i < Board.transform.childCount; i++)
        {

           

            Board.transform.GetChild(i).GetComponent<BridgeButtonManager>().BridgeRester();
        }
        if (BlueTurn)
        {
            
           for (int N = 0; N < Board.transform.childCount/2; N++)
            {
         //    monte.MonteCarloSearch(N);
              
            }
         
           // kc.CheckPosition();
        }
        if (!BlueTurn)
        {
            if (NowTurn >= MaxTurnNumber * 0.8)
            {

                for (int N = PieceNumber ; N < Board.transform.childCount  ; N++)
                {
                    monte.MonteCarloSearch(N);

                }

            }
            else
            {
                int[,] Ban = kc.AIBanState();
                alpha.AlphaBeta(1, Ban, 3);
                for (int n = 0; n < PieceNumber * 2; n++)
                {
                    GameObject komar = Board.transform.GetChild(n).gameObject;
                    BridgeButtonManager[] bbb = new BridgeButtonManager[12];
                    bbb[n] = komar.GetComponent<BridgeButtonManager>();
                    kc.X[n] = bbb[n].BoardX;
                    kc.Y[n] = bbb[n].BoardY;
                }
                // kc.GetCanMoveIndex(1);
                /*  for (int N = PieceNumber; N < Board.transform.childCount; N++)
                  {
                      Debug.Log(N);
                       int[,] Ban = kc.AIBanState();
                       alpha.AlphaBeta(2, Ban, N);
                      for (int n = 0; n < PieceNumber * 2; n++)
                      {
                          GameObject komar = Board.transform.GetChild(n).gameObject;
                          BridgeButtonManager[] bbb = new BridgeButtonManager[12];
                          bbb[n] = komar.GetComponent<BridgeButtonManager>();
                          kc.X[n] = bbb[n].BoardX;
                          kc.Y[n] = bbb[n].BoardY;
                      }

                  }
                
            }
        }
    }
    
  
}
*/
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

public class TurnManager : MonoBehaviour
{

    [Header("マップのCSVファイル名"), SerializeField] string MapCSV;
    [Header("最大ターン数"), SerializeField]public int MaxTurnNumber;
    [Header("生成する駒の数"), SerializeField]public int PieceNumber;
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


    [Header("青いコマの置き場所"), SerializeField]public GameObject BlueBridges;
    [Header("赤いコマの置き場所"), SerializeField]public GameObject RedBridges;


    [HideInInspector] public int BlueScore = 0;
    [HideInInspector] public int RedScore = 0;
    [HideInInspector] public bool UntapPhase = false;
    [HideInInspector] public bool TurnEnd = false;
    [HideInInspector] public List<string[]> MapData = new List<string[]>();

    Area area;
    Button RedButton;
    Button BlueButton;
    Transform square;
    int BridgeActCount = 0;
    int BridgestandbyCount = 0;
   public int NowTurn = 0;
    TextAsset csvFile;


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

        Debug.Log("BlueScore:" + BlueScore);
        Debug.Log("RedScore:" + RedScore);
    }

    public void CallBridgeRester()
    {
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
         KomaCalulator komaCalulator;
     //ターン確認
        Alpha a;
        komaCalulator =  this.transform.GetComponent<KomaCalulator>();
        a = this.transform.GetComponent<Alpha>();
       int[,] ban = komaCalulator.AIBanState();
          a.AlphaBeta(2, ban, 0 ,true);
       /*var c = komaCalulator.BanCanMove(ban, 0, true);
        foreach(var d in c)
        {
            if(d.Build == true)
            {
                Debug.Log(999);
            }
            else
            {
                Debug.Log("gggg");
            }
        }
       */

    }

  
}