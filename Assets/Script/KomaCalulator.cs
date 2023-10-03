using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Index;
using state;
using UnityEditor;
using UnityEditorInternal;
using static UnityEditor.VersionControl.Asset;
using System.Runtime;

public class KomaCalulator : MonoBehaviour
{

    TurnManager TM;
    GameObject Board;
    GameObject obj;
    GameObject mon;
    GameObject Boards;
    
          public int[,] Ban;
    bool isOnce = true;
    bool O = true;
    Monte monte;
    Area area;
    int NowTurn;
   public bool AIBlueTurn;
   public int PlayerCount = 0;
    bool CanMove = false;
    bool NotBuild;
    BridgeButtonManager bb;
    BridgeButtonManager bb2;
    [HideInInspector] public int BoardX;
    [HideInInspector] public int BoardY;
    int Movecount;
    // Start is called before the first frame update
    void Start()
    {
        Board = GameObject.Find("UserInterface/Board");
        Boards = GameObject.Find("BoardBridge");
       // obj = GameObject.Find("BlueBridge(Clone)");
        // obj2 = GameObject.Find("BlueBridge(Clone)");
        mon = Boards.transform.GetChild(0).gameObject;
        TM = Board.GetComponent<TurnManager>();
        
        // bb2 = obj2.GetComponent<BridgeButtonManager>();
        Ban = new int[TM.BoardXMax, TM.BoardYMax];
        area = Board.GetComponent<Area>();
        monte = mon.GetComponent<Monte>();
    }
    private void Update()
    {

        AIBanState();
        for (int y = 0; y < TM.BoardYMax; y++)
        {//Add commit push
            for (int x = 0; x < TM.BoardXMax; x++)
            {
                if (Ban[x, y] == 10)
                {
                    AIAreaCheckBlue(x ,y);
                }
                if (Ban[x ,y] == -10)
                {
                    AIAreaCheckRed(x, y);
                }
            }
        }
    }
    public List<KomaIndex> GetAllCanBuildKoma()
    {


        var canPutStones = new List<KomaIndex>();

        // var putStoneStates = States;
        //Debug.Log(putStoneStates);
        for (var y = 0; y < TM.BoardYMax; y++)
        {
            for (var x = 0; x < TM.BoardXMax; x++)
            {
                area = Board.transform.GetChild(x).GetChild(y).GetComponent<Area>();
                if (area.RedWall == true)
                {

                }
                else if (area.BlueWall == true)
                {
                    //  putStoneStates = state.State.Blue;

                }
                else if (area.pond == true)
                {

                }
                else if (area.castle == true)
                {

                }
                else
                {

                    canPutStones.Add(new KomaIndex(x, y));
                    Debug.Log(12);

                }
                Debug.Log(bb.BoardX);
                Debug.Log(TM.BoardXMax);
                Debug.Log(TM.BoardYMax);

            }
        }

        return canPutStones;
    }
   public void AIAreaCheckRed(int x , int y)
    {

        while (true)
        {

        }
        
      
       //エリア判定
       //職人判定
       //駒移動,置くときのターンの移転
       //alpha-beta
    }
    public void AIAreaCheckBlue(int x , int y)
    {
        while (true)
        {

        }
    }
    public int AreaMathChengh(int x, int y)
    {
        area = TM.transform.GetChild(x).GetChild(y).GetComponent<Area>();
        if (area.BlueWall)
        {
            Debug.Log("Blue");
            return 10;
            //if
        }
        else if (area.RedWall)
        {
            Debug.Log("Red");
            return -10;
            //if
        }
        else if (area.RedArea)
        {
            Debug.Log("RedA");
            return -30;
            //if
        }
        else if (area.BlueArea)
        {
            Debug.Log("BlueA");
            return 30;
        }
        else if (area.pond)
        {
            Debug.Log("Ike");
            return -1;
        }
        else if (area.castle)
        {
            Debug.Log("castle");
            return 100;
        }
        
            if (area.Bridge)
            {
                Debug.Log("Bluek");
               
                return 5;
            }
        
      
            if (area.Bridge)
            {
                Debug.Log("Redk");
          
                return -5;
            }
        
        else
        {
            Debug.Log("tyu");
            return 0;
        }
        

    }
    public void AIBanState()
    {

        for (int y = 0; y < TM.BoardYMax; y++)
        {
            for (int x = 0; x < TM.BoardXMax; x++)
            {

                Ban[x, y] = AreaMathChengh(x, y);

            }
        }

    }
    
    public void Randam(int N )
    {
        Debug.Log("aaaaaaaaaaaaaaaaaaaaa");
        if (AIBlueTurn == false)
        {
            N += TM.PieceNumber;
        }
        obj = Boards.transform.GetChild(N).gameObject;
        bb = obj.GetComponent<BridgeButtonManager>();
        for (; ; )
        {
           int rnd = Random.Range(9, 13);
            if (rnd == 1)
            {
                //  bg_Red.MoveForwardBridge();
                //  bg_Red.BridgeRester();
                bool can = AICanMove(bb.BoardX + 1, bb.BoardY);
                if (can)
                {
                    Ban[bb.BoardX, bb.BoardY] -= 11;
                    Ban[bb.BoardX + 1, bb.BoardY] += 11;
                    PlayerCount++;
                    
                }
                //  continue;
                break;

            }
            if (rnd == 2)
            {
                //bg_Red.MoveBackwardBridge();
                //bg_Red.BridgeRester();
                bool can = AICanMove(bb.BoardX, bb.BoardY + 1);
                if (can)
                {
                    Ban[bb.BoardX, bb.BoardY + 1] += 11;
                    Ban[bb.BoardX, bb.BoardY] -= 11;
                    PlayerCount++;
                }
                break;
            }
            if (rnd == 3)
            {
                //bg_Red.MoveRightBridge();
                //bg_Red.BridgeRester();
                bool can = AICanMove(bb.BoardX + 1, bb.BoardY);
                if (can)
                {
                    Ban[bb.BoardX + 1, bb.BoardY] += 11;
                    Ban[bb.BoardX, bb.BoardY] -= 11;
                    PlayerCount++;
                }
                break;
            }
            if (rnd == 4)
            {
                //bg_Red.MoveRightBridge();
                // bg_Red.BridgeRester();
                bool can = AICanMove(bb.BoardX - 1, bb.BoardY);
                if (can)
                {
                    Ban[bb.BoardX - 1, bb.BoardY] += 11;
                    Ban[bb.BoardX, bb.BoardY] -= 11;
                    PlayerCount++;
                }
                break;
            }
            if (rnd == 5)
            {
                bool can = AICanMove(bb.BoardX - 1, bb.BoardY - 1);
                if (can)
                {
                    Ban[bb.BoardX - 1, bb.BoardY - 1] += 11;
                    Ban[bb.BoardX, bb.BoardY] -= 11;
                    PlayerCount++;
                }
                //bg_Red.MoveLeftBridge();
                //bg_Red.BridgeRester();
                break;
            }
            if (rnd == 6)
            {
                //bg_Red.MoveLeftForwardBridge();
                //bg_Red.BridgeRester();
                bool can =AICanMove(bb.BoardX - 1, bb.BoardY + 1);
                if (can)
                {
                    Ban[bb.BoardX - 1, bb.BoardY + 1] += 11;
                    Ban[bb.BoardX, bb.BoardY] -= 11;
                    PlayerCount++;
                }
                break;
            }
            if (rnd == 7)
            {
                //bg_Red.MoveRightForwardBridge();
                //bg_Red.BridgeRester();
                bool can = AICanMove(bb.BoardX + 1, bb.BoardY - 1);
                if (can)
                {
                    Ban[bb.BoardX + 1, bb.BoardY - 1] += 11;
                    Ban[bb.BoardX, bb.BoardY] -= 11;
                    PlayerCount++;
                }
                break;
            }
            if (rnd == 8)
            {
                //bg_Red.MoveRightBackwardBridge();
                //bg_Red.BridgeRester();
                bool can = AICanMove(bb.BoardX + 1, bb.BoardY + 1);
                if (can)
                {
                    Ban[bb.BoardX + 1, bb.BoardY + 1] += 11;
                    Ban[bb.BoardX, bb.BoardY] -= 11;
                    PlayerCount++;
                }
                break;

            }
            if (rnd == 9)
            {
                // bg_Red.BuildAndDestroyForward();
                // bg_Red.BridgeRester();
                int can = AICanBuildDestoroy(bb.BoardX + 1, bb.BoardY);
                if (can == 1)
                {
                    Ban[bb.BoardX + 1, bb.BoardY] = 0;
                    PlayerCount++;
                }
                if (can == 2)
                {
                    Ban[bb.BoardX + 1, bb.BoardY] = 10;
                    PlayerCount++;
                }
                if (can == -2)
                {
                    Ban[bb.BoardX + 1, bb.BoardY] = -10;
                    PlayerCount++;
                }
                break;
            }
            if (rnd == 10)
            {
                //   bg_Red.BuildAndDestroyBackward();
                // bg_Red.BridgeRester();
                // break;
                int can = AICanBuildDestoroy(bb.BoardX - 1, bb.BoardY);
                if (can == 1)
                {
                    Ban[bb.BoardX - 1, bb.BoardY] = 0;
                    PlayerCount++;

                }
                if (can == 2)
                {
                    Ban[bb.BoardX - 1, bb.BoardY] = 10;
                    PlayerCount++;
                }
                if (can == -2)
                {
                    Ban[bb.BoardX - 1, bb.BoardY] = -10;
                    PlayerCount++;
                }
                break;
            }
            if (rnd == 11)
            {
                //bg_Red.BuildAndDestroyRight();
                //bg_Red.BridgeRester();
                int can = AICanBuildDestoroy(bb.BoardX, bb.BoardY + 1);
                if (can == 1)
                {
                    Ban[bb.BoardX, bb.BoardY + 1] = 0;
                    PlayerCount++;

                }
                if (can == 2)
                {
                    Ban[bb.BoardX, bb.BoardY + 1] = 10;
                    PlayerCount++;
                }
                if (can == -2)
                {
                    Ban[bb.BoardX, bb.BoardY + 1] = -10;
                    PlayerCount++;
                }

                break;
            }
            if (rnd == 12)
            {
                //bg_Red.BuildAndDestroyLeft();
                //bg_Red.BridgeRester();
                int can = AICanBuildDestoroy(bb.BoardX, bb.BoardY - 1);
                if (can == 1)
                {
                    Ban[bb.BoardX, bb.BoardY - 1] = 0;
                    PlayerCount++;

                }
                if (can == 2)
                {
                    Ban[bb.BoardX, bb.BoardY - 1] = 10;
                    PlayerCount++;
                }
                if (can == -2)
                {
                    Ban[bb.BoardX, bb.BoardY - 1] = -10;
                    PlayerCount++;
                }
                break;

            }
        }
    }
    public bool AICanMove(int x, int y)
    {   
        if (Ban[x, y] == -1 || Ban[x, y] == 10 || Ban[x, y] == -10 || Ban[x, y] == -11 || Ban[x, y] == 11 || x > TM.BoardXMax || y > TM.BoardYMax)
        {
            return false;
        }
        return true;
    }
    public int AICanBuildDestoroy(int x, int y)
    {
       
        if (Ban[x, y] == 100 || Ban[x, y] == 11 || Ban[x, y] == -11)
        {
            return 0;
        }
        if (Ban[x, y] == 10 || Ban[x, y] == -10)
        {
            return 1;
        }
        if (TM.BlueTurn)
        {
            return 2;
            //Ban[bg.BoardX, bg.BoardY] += 
        }
        else {
            return -2;
        }
    }
    public int AIScoreCheck()
    {
        var myscore = 0;
        var enemyscore = 0;
        for (int y = 0; y < TM.BoardYMax; y++)
        {
            for (int x = 0; x < TM.BoardXMax; x++)
            {
                if(Ban[x ,y] == 10)
                {
                    myscore += 10;
                }
                if (Ban[x,y] == -10)
                {
                    enemyscore += 10;
                }
                if (Ban[x,y] == 100)
                {

                }
                

            }
        }
        
        return enemyscore -  myscore;
    }
  /*  public int AIAreaCheck()
    {
        for (int y = 0; y < TM.BoardYMax; y++)
        {
            for (int x = 0; x < TM.BoardXMax; x++)
            {

                

            }
        }
    }
    public bool AIAreaCheckUp()
    {
         for (int x = 0; x < TM.BoardXMax; x++)
            {



            }
        
    }
  */
    public void GetPutkomaState()
    {
        for (var i = 0; i < 30;)
        {

            CanMove = TM.CanMove(bb.BoardY, bb.BoardX + 1);
            if (CanMove)
            {
                bb.BuildAndDestroyRight();
                //bb.BoardX -= 1;
                break;

            }
            // bb.BoardX -= 1;
            CanMove = TM.CanMove(bb.BoardY, bb.BoardX - 1);
            if (CanMove)
            {
                Debug.Log(8);
                bb.BuildAndDestroyLeft();
                // bb.BoardX += 1;
                break;

            }
            //  bb.BoardX += 1;
            //  bb.BoardX += 1;
            CanMove = TM.CanMove(bb.BoardY + 1, bb.BoardX);
            if (CanMove)
            {
                bb.BuildAndDestroyBackward();
                break;

            }
            CanMove = TM.CanMove(bb.BoardY - 1, bb.BoardX);
            if (CanMove)
            {

                bb.BuildAndDestroyForward();
                break;

            }
            if (TM.BoardYMax - bb.BoardY < 5 || TM.BoardXMax - bb.BoardX < 5)
            {

                bb.MoveRightForwardBridge();
                break;

            }
            else
            {
                bb.MoveRightBackwardBridge();
                break;

            }

        }

    }
    public List<KomaIndex> GetPutkomaStates()
    {
        var canPutStones = new List<KomaIndex>();

        var Index = new List<KomaIndex>();
        int x = bb.BoardX;
        int y = bb.BoardY;

        area = Board.transform.GetChild(x).GetChild(y).GetComponent<Area>();
        Debug.Log(x);
        CanMove = TM.CanMove(x + 1, y);
        if (CanMove)
        {
            // bb2.BuildAndDestroyRight();
            //bb.BoardX -= 1;
            canPutStones.Add(new KomaIndex(x, y));
            //Index.Add(new KomaIndex(x, y));
            Debug.Log("eeeee");


        }
        CanMove = TM.CanMove(x - 1, y);
        if (CanMove)
        {
            // bb2.BuildAndDestroyRight();
            //bb.BoardX -= 1;
            canPutStones.Add(new KomaIndex(x, y));
            //Index.Add(new KomaIndex(x, y));
            Debug.Log("hidari");


        }
        CanMove = TM.CanMove(x, y + 1);
        if (CanMove)
        {
            // bb2.BuildAndDestroyRight();
            //bb.BoardX -= 1;
            canPutStones.Add(new KomaIndex(x, y));
            //Index.Add(new KomaIndex(x, y));
            Debug.Log("ue");

        }
        CanMove = TM.CanMove(x, y - 1);
        if (CanMove)
        {
            // bb2.BuildAndDestroyRight();
            //bb.BoardX -= 1;
            canPutStones.Add(new KomaIndex(x, y));
            //Index.Add(new KomaIndex(x, y));
            Debug.Log("eeeee");

        }
        CanMove = TM.CanMove(x - 1, y - 1);
        if (CanMove)
        {
            // bb2.BuildAndDestroyRight();
            //bb.BoardX -= 1;
            canPutStones.Add(new KomaIndex(x, y));
            //Index.Add(new KomaIndex(x, y));
            Debug.Log("eeeee");

        }
        CanMove = TM.CanMove(x + 1, y - 1);
        if (CanMove)
        {
            // bb2.BuildAndDestroyRight();
            //bb.BoardX -= 1;
            canPutStones.Add(new KomaIndex(x, y));
            //Index.Add(new KomaIndex(x, y));
            Debug.Log("eeeee");

        }
        CanMove = TM.CanMove(x - 1, y + 1);
        if (CanMove)
        {
            // bb2.BuildAndDestroyRight();
            //bb.BoardX -= 1;
            canPutStones.Add(new KomaIndex(x, y));
            //Index.Add(new KomaIndex(x, y));
            Debug.Log("eeeee");

        }
        CanMove = TM.CanMove(x + 1, y + 1);
        if (CanMove)
        {
            // bb2.BuildAndDestroyRight();
            //bb.BoardX -= 1;
            canPutStones.Add(new KomaIndex(x, y));
            //Index.Add(new KomaIndex(x, y));
            Debug.Log("eeeee");

        }
        return canPutStones;


    }
    public int Win_Blue()
    {
        int Count = 0;
        Count++;
        return Count;
    }
    public int Win_Red()
    {
        int Count = 0;
        Count++;
        return Count;
    }
    public void GetPutkomaState2(int x, int y)
    {
        for (var i = 0; i < 30;)
        {
            CanMove = TM.CanMove(bb2.BoardY, bb2.BoardX + 1);
            if (CanMove)
            {
                bb2.BuildAndDestroyRight();
                //bb.BoardX -= 1;
                break;

            }
            // bb.BoardX -= 1;
            CanMove = TM.CanMove(bb2.BoardY, bb2.BoardX - 1);
            if (CanMove)
            {
                Debug.Log(8);
                bb2.BuildAndDestroyLeft();
                // bb.BoardX += 1;
                break;
            }
            //  bb.BoardX += 1;
            //  bb.BoardX += 1;
            CanMove = TM.CanMove(bb2.BoardY + 1, bb2.BoardX);
            if (CanMove)
            {
                bb2.BuildAndDestroyBackward();
                break;
            }
            CanMove = TM.CanMove(bb2.BoardY - 1, bb2.BoardX);
            if (CanMove)
            {

                bb2.BuildAndDestroyForward();
                break;
            }
            if (TM.BoardYMax - bb2.BoardY < 2 || TM.BoardXMax - bb2.BoardX < 2)
            {

                bb2.MoveRightForwardBridge();
                break;
            }
            else
            {
                bb2.MoveRightBackwardBridge();
                break;
            }
        }
    }
}
 