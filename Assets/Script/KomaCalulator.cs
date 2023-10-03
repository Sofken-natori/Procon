using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Koma;
using state;
using UnityEditor;
using UnityEditorInternal;
using static UnityEditor.VersionControl.Asset;
using System.Runtime;
using  System.Linq;

public class KomaCalulator : MonoBehaviour
{
  
    TurnManager TM;
    GameObject Board;
    GameObject montes;
    public GameObject obj;
    GameObject mon;
    GameObject Boards;
    public Text txt;
    public int[,] Ban;
    Monte monte;
    Area area;
    public bool AIBlueTurn;
    public int PlayerCount = 0;
    bool CanMove = false;
    public BridgeButtonManager BBB;
  public  BridgeButtonManager[] BB = new BridgeButtonManager[12];
   public int[] X = new int[12];
   public int[] Y = new int[12];
    public int[] BanX = new int[12];
    public int[] BanY = new int[12];
   public BridgeButtonManager[] bb;
    void Start()
    {
      
        Board = GameObject.Find("UserInterface/Board");
        Boards = GameObject.Find("BoardBridge");
        mon = Boards.transform.GetChild(0).gameObject;
        TM = Board.GetComponent<TurnManager>();
        bb = new BridgeButtonManager[12];
        Ban = new int[TM.BoardXMax, TM.BoardYMax];
        area = Board.GetComponent<Area>();
        monte = mon.GetComponent<Monte>();
      
        CheckPosition();
    }
    public void Update()
    {
       
    }
    //エリア判定
    //配列判定
    //alpha-beta

    public void AIAreaCheckBlue(int[,] ban)
    {
        for (int i = 0; i < TM.BoardXMax; i++)
        {
            for (int j = 0; j < 1; j++)
            {
                AIAreaCheckBlueAround(0, i, true,  ban);
            }

            for (int j = 0; j < 1; j++)
            {
                AIAreaCheckBlueAround(i, 0, true , ban);
            }
            for (int k = TM.BoardYMax - 1; k < TM.BoardYMax; k++)
            {
                AIAreaCheckBlueAround(k, i, true , ban);
            }
            for (int l = TM.BoardYMax - 1; l < TM.BoardYMax; l++)
            {
                AIAreaCheckBlueAround(i, l, true , ban);
            }


        }
     
        for (int Y = 0; Y < TM.BoardYMax; Y++)
        {
            for (int X = 0; X < TM.BoardXMax; X++)
            {
                if (Ban[X, Y] == 0 || Ban[X, Y] == 100)
                {
                    Ban[X, Y] += 30;
                    RedAndBlue();
                }
                else if ( Ban[X, Y] == 10)
                {
                    Ban[X, Y] = 10;
                }
                else if (Ban[X, Y] == -30)
                {

                }
                else
                {
                    Ban[X, Y] -= 2;
                }
            }
        }

    }
    public void RedAndBlue()
    {
        
    }
    public void AIAreaCheckBlueAround(int x, int y, bool Blue , int[,] Ban)
    {

        if (Blue)
        {

            //エリアの上からをどうするか
            //alpha-beta
            //評価関数
            //配列とMonteの整備
            if (Ban[x, y] != 2 && Ban[x, y] != 10 && Ban[x , y] != 30 && Ban[x ,y] != -30 && Ban[x ,y] != 130 && Ban[x ,y] != 15 )
            {
                //NowTurn++;
                Ban[x, y] += 2;
                if (y != TM.BoardYMax - 1)
                {
                    AIAreaCheckBlueAround(x, y + 1, true , Ban);
                }
                if (y != 0)
                {

                    AIAreaCheckBlueAround(x, y - 1, true , Ban);
                }
                if (x != 0)
                {

                    AIAreaCheckBlueAround(x - 1, y, true , Ban);
                }
                if (x != TM.BoardXMax - 1)
                {

                    AIAreaCheckBlueAround(x + 1, y, true , Ban);
                }
            }
        }
        else
        {
            if (Ban[x, y] != 2 && Ban[x, y] != 10)
            {
               
                Ban[x, y] += 2;
                if (y != TM.BoardYMax - 1)
                {
                    AIAreaCheckBlueAround(x, y + 1, false, Ban);
                }
                if (y != 0)
                {

                    AIAreaCheckBlueAround(x, y - 1, false, Ban);
                }
                if (x != 0)
                {

                    AIAreaCheckBlueAround(x - 1, y, false, Ban);
                }
                if (x != TM.BoardXMax - 1)
                {

                    AIAreaCheckBlueAround(x + 1, y, false, Ban);
                }
            }
        }
    }

    public int AreaMathChengh(int x, int y )
    {
        area = this.transform.GetChild(x).GetChild(y).GetComponent<Area>();
        if (area.BlueWall)
        {
         
            return 10;
        
        }
        else if (area.RedWall)
        {
           
            return -10;
           
        }
        else if (area.RedArea)
        {
           
            return -30;
           
        }
        else if (area.BlueArea)
        {
         
            return 30;
        }
        else if (area.pond)
        {
           // Debug.Log("Ike");
            return -1;
        }
        else if (area.castle)
        {
            // Debug.Log("castle");
            return 100;
        }
        else if (area.Bridge)
        {
            // Debug.Log("Bluek");

            return 11;
        }
        else
        {
            //  Debug.Log("tyu");
            return 0;
        }


    }
    public int[,] AIBanState()
    {
       txt.text = "";
     
        for (int y = 0; y < TM.BoardYMax; y++)
        {
           txt.text += "\n";
            for (int x = 0; x < TM.BoardXMax; x++)
            {
                Ban[x, y] = AreaMathChengh(y, x);
             txt.text += Ban[x, y].ToString() + "  ";
            }
        }
        return Ban;

    }
    #region モンテカルロ法用ランダム
    public int[,] AIdown(int N)
    {
        /* bool can = AICanMove(bb[N].BoardY + 1, bb[N].BoardX);

         if (can)
         {

             Ban[bb[N].BoardY, bb[N].BoardX] -= 11;
             Ban[bb[N].BoardY + 1, bb[N].BoardX] += 11;
             bb[N].BoardY += 1;

         }
        */
        bool can = AICanMove(Y[N] + 1, X[N]);

        if (can)
        {

            Ban[Y[N], X[N]] -= 11;
            Ban[Y[N] +1, X[N]] += 11;
            Y[N] += 1;

        }
        return Ban;

    }
    public int[,] AIRight(int N)
    {

        /*   bool can = AICanMove(bb[N].BoardY, bb[N].BoardX - 1);
           if (can)
           {

               Ban[bb[N].BoardY, bb[N].BoardX - 1] += 11;
               Ban[bb[N].BoardY, bb[N].BoardX] -= 11;
               bb[N].BoardX -= 1;
           }
        */
        bool can = AICanMove(Y[N], X[N]-1);

        if (can)
        {

            Ban[Y[N], X[N]] -= 11;
            Ban[Y[N] , X[N]-1] += 11;
            X[N] -= 1;
        }
        return Ban;
    }
    public int[,] AILeft(int N)
    {

        /*  bool can = AICanMove(bb[N].BoardY , bb[N].BoardX + 1);
          if (can)
          {

              Ban[bb[N].BoardY , bb[N].BoardX + 1] += 11;
              Ban[bb[N].BoardY, bb[N].BoardX] -= 11;
              bb[N].BoardX += 1;
          }
        */
        bool can = AICanMove(Y[N] , X[N]+1);

        if (can)
        {

            Ban[Y[N], X[N]] -= 11;
            Ban[Y[N] , X[N]+1] += 11;
            X[N] += 1;

        }
        return Ban;
    }
    public int[,] AIUP(int N)
    {

        /*  bool can = AICanMove(bb[N].BoardY - 1, bb[N].BoardX);
          if (can)
          {

              Ban[bb[N].BoardY - 1, bb[N].BoardX] += 11;
              Ban[bb[N].BoardY, bb[N].BoardX] -= 11;
              bb[N].BoardY -= 1;
          }
        */
        bool can = AICanMove(Y[N] - 1, X[N]);

        if (can)
        {
          
            Ban[Y[N], X[N]] -= 11;
            Ban[Y[N] - 1, X[N]] += 11;
            Y[N] -= 1;

        }
        return Ban;
    }
    public int[,] AIHidarisita(int N)
    {

        /* bool can = AICanMove(bb[N].BoardY + 1, bb[N].BoardX - 1);
         if (can)
         {

             Ban[bb[N].BoardY + 1, bb[N].BoardX - 1] += 11;
             Ban[bb[N].BoardY, bb[N].BoardX] -= 11;
             bb[N].BoardY += 1;
             bb[N].BoardX -= 1;
         }
        */
        bool can = AICanMove(Y[N] + 1, X[N] -1);

        if (can)
        {
           
            Ban[Y[N], X[N]] -= 11;
            Ban[Y[N] + 1, X[N] -1] += 11;
            Y[N] += 1;
            X[N] -= 1;
        }
        return Ban;
    }
    public int[,] AIMigisita(int N)
    {
        /*  bool can = AICanMove(bb[N].BoardY + 1, bb[N].BoardX + 1);
          if (can)
          {

              Ban[bb[N].BoardY + 1, bb[N].BoardX + 1] += 11;
              Ban[bb[N].BoardY, bb[N].BoardX] -= 11;
              bb[N].BoardY += 1;
              bb[N].BoardX += 1;
          }
        */
        bool can = AICanMove(Y[N] + 1, X[N] +1);

        if (can)
        {
           
            Ban[Y[N], X[N]] -= 11;
            Ban[Y[N] + 1, X[N]+1] += 11;
            Y[N] += 1;
            X[N] += 1;

        }
        return Ban;
    }
    public int[,] AImigiue(int N)
    {

        /*  bool can = AICanMove(bb[N].BoardY - 1, bb[N].BoardX + 1);
          if (can)
          {

              Ban[bb[N].BoardY - 1, bb[N].BoardX + 1] += 11;
              Ban[bb[N].BoardY, bb[N].BoardX] -= 11;
              bb[N].BoardY -= 1;
              bb[N].BoardX += 1;
          }
        */
        bool can = AICanMove(Y[N] - 1, X[N] + 1);

        if (can)
        {
           
            Ban[Y[N], X[N]] -= 11;
            Ban[Y[N] - 1, X[N] +1] += 11;
            Y[N] -= 1;
            X[N] += 1;
        }
        return Ban;
    }
    public int[,] AIhidariue(int N)
    {

        /*  bool can = AICanMove(bb[N].BoardY - 1, bb[N].BoardX - 1);
          if (can)
          {

              Ban[bb[N].BoardY - 1, bb[N].BoardX - 1] += 11;
              Ban[bb[N].BoardY, bb[N].BoardX] -= 11;
              bb[N].BoardY -= 1;
              bb[N].BoardX -= 1;

          }
        */
        bool can = AICanMove(Y[N] - 1, X[N]-1);

        if (can)
        {
        
            Ban[Y[N], X[N]] -= 11;
            Ban[Y[N] - 1, X[N] -1] += 11;
            Y[N] -= 1;
            X[N] -= 1;
        }
        return Ban;
    }
    public int[,] BuildDown(int N)
    {
        int can = AICanBuildDestoroy(Y[N] + 1, X[N] );
        if (can == 1)
        {
            Ban[Y[N] + 1, X[N]] = 0;
        }
        else if (can == 2)
        {
            Ban[Y[N] + 1, X[N]] = 10;

        }
        else if (can == -2)
        {
            Ban[Y[N] + 1, X[N]] = -10;
        }
        return Ban;
    }
    public int[,] BuildUp(int N)
    {
        int can = AICanBuildDestoroy(Y[N] - 1, X[N]);
        if (can == 1)
        {
            Ban[Y[N] - 1, X[N]] = 0;
          
            PlayerCount++;

        }
        else if (can == 2)
        {

            Ban[Y[N] - 1, X[N]] = 10;
          
            PlayerCount++;
        }
        else if (can == -2)
        {

            Ban[Y[N] - 1, X[N]] = -10;
    
            PlayerCount++;
        }
        return Ban;
    }
    public int[,] BuildLeft(int N)
    {
        int can = AICanBuildDestoroy(Y[N], X[N] - 1);
        if (can == 1)
        {
            Ban[Y[N], X[N] - 1] = 0;
            PlayerCount++;
        }
        else if (can == 2)
        {
            Ban[Y[N], X[N] - 1] = 10;
            PlayerCount++;
        }
        else if (can == -2)
        {
            Ban[Y[N], X[N] - 1] = -10;
            PlayerCount++;
        }
        //  break;
        return Ban;
    }
    public int[,] BuildRight(int N)
    {
        int can = AICanBuildDestoroy(Y[N], X[N] + 1);
        if (can == 1)
        {
            Ban[Y[N], X[N] + 1] = 0;

        }
        else if (can == 2)
        {
            Ban[Y[N], X[N] + 1] = 10;
        }
        else if (can == -2)
        {
            Ban[Y[N], X[N] + 1] = -10;
        }
        return Ban;
    }

    public int[,] Randam(int N)
    {
        if (AIBlueTurn == false)
        {
            N += TM.PieceNumber;  
        }
        int rnd = Random.Range(1, 13);
            if (rnd == 1)
            {
               Ban = AIdown(N);
                PlayerCount++;
                return Ban;

            }
            if (rnd == 2)
            {
                AILeft(N);
                PlayerCount++;
                return Ban;
            }
            if (rnd == 3)
            {
                AIRight(N);
                PlayerCount++;
                return Ban;
            }
            if (rnd == 4)
            {
                AIUP(N);
                PlayerCount++;
                return Ban;
            }
            if (rnd == 5)
            {
                AIHidarisita(N);
                PlayerCount++;
                return Ban;
            }
            if (rnd == 6)
            {
                AIMigisita(N);
                PlayerCount++;
                return Ban;
            }
            if (rnd == 7)
            {
                AIhidariue(N);
                PlayerCount++;
                return Ban;
            }
            if (rnd == 8)
            {
                AImigiue(N);
                PlayerCount++;
                return Ban;

            }
            if (rnd == 9)
            {
                BuildUp(N);
                PlayerCount++;
                return Ban;
            }
            if (rnd == 10)
            {
                BuildDown(N);
                PlayerCount++;
                return Ban;
            }
            if (rnd == 11)
            {
                BuildRight(N);
                PlayerCount++;
                return Ban;
            }
            if (rnd == 12)
            {
                BuildLeft(N);
                PlayerCount++;
                return Ban;

            }
        PlayerCount++;
        return Ban;

    }
    #endregion
    public bool AICanMove(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < TM.BoardXMax && y < TM.BoardYMax && Ban[x, y] != -1 && Ban[x, y] != 10 && Ban[x, y] != -10 &&   Ban[x, y] != 11 )
        {
            return true;
        }
        return false;
    }
    public int AICanBuildDestoroy(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < TM.BoardXMax && y < TM.BoardYMax && Ban[x, y] != 100 && Ban[x, y] != 11)
        {
            if (Ban[x, y] == 10 || Ban[x, y] == -10)
            {
                return 1;
            }
            else if (AIBlueTurn)
            {

                return 2;
            }
            else if (!AIBlueTurn)
            {

                return -2;
            }
        }
        
        return 0;
    }
    public bool AIScoreCheck(int[,] ban)
    {
      //  txt.text = "";
    
        var myscore = 0;
        var enemyscore = 0;
        for (int y = 0; y < TM.BoardYMax; y++)
        {
           // txt.text += "\n";
            for (int x = 0; x < TM.BoardXMax; x++)
            {
                area = this.transform.GetChild(x).GetChild(y).GetComponent<Area>();
                if (ban[y, x] == 10)
                {
                    myscore += 10;
                }
                else if (ban[y, x] == -10)
                {
                    //  area.RedWall = true;
                    enemyscore += 10;
                }
                else if (ban[y, x] == 15)
                {
                    myscore += 30;
                    enemyscore += 30;
                }
                else if (ban[y, x] == 30)
                {
                  myscore += 30;
                }
                else if (ban[y ,x] == -30)
                {
                  enemyscore += 30;
                }
                else if (ban[y, x] == 130)
                {
                    myscore += 100;
                }
                else if (ban[y, x] == -130)
                {
                    enemyscore += 100;
                }
              //  txt.text += Ban[y, x].ToString() + "  ";

            }
        }


        if (myscore > enemyscore)
        {

            return true;
        }
        return false;
    }

    
    public List<KomaIndex> GetCanMoveIndex(int N)
    {
        Debug.Log(X[N]);//1
        Debug.Log(Y[N]);//2
        var CanMoves = new List<KomaIndex>();
        CanMove = AICanMoves(Y[N] , X[N]+1);
        if (CanMove)
        {

            CanMoves.Add(new KomaIndex(X[N] +1, Y[N] ));
        }
        CanMove = AICanMoves(Y[N] , X[N] -1);
        if (CanMove)
        {

            CanMoves.Add(new KomaIndex(X[N] - 1, Y[N]));
        }
        CanMove = AICanMoves(Y[N] +1, X[N] );
        if (CanMove)
        {

            CanMoves.Add(new KomaIndex(X[N] , Y[N] +1));
        }
        CanMove = AICanMoves(Y[N] - 1, X[N]);
        if (CanMove)
        {

            CanMoves.Add(new KomaIndex(X[N] , Y[N] -1));
        }
        CanMove = AICanMoves(Y[N] - 1, X[N] - 1);

        if (CanMove)
        {

            CanMoves.Add(new KomaIndex(X[N] - 1, Y[N] - 1));
        }
        CanMove = AICanMoves(Y[N] - 1, X[N] + 1);
        if (CanMove)
        {
            CanMoves.Add(new KomaIndex(X[N] + 1, Y[N] - 1));
        }
        CanMove = AICanMoves(Y[N] + 1, X[N] - 1);
        if (CanMove)
        {

            CanMoves.Add(new KomaIndex(X[N] - 1, Y[N] + 1));
        }
        CanMove = AICanMoves(Y[N] + 1, X[N] + 1);
        if (CanMove)
        {

            CanMoves.Add(new KomaIndex(X[N] + 1, Y[N] + 1));
        }
    
        return CanMoves;
    }
    public bool CanBuild(int x, int y)
    {
        if (x >= TM.BoardXMax || y >= TM.BoardYMax || x < 0 || y < 0 )
        {
            return false;
        }
        area = this.transform.GetChild(x).GetChild(y).GetComponent<Area>();
        if (area.castle || area.Bridge )
        {
            return false;
        }
        return true;
    }
    public bool AICanMoves(int x, int y)
    {
        if (x >= TM.BoardXMax || y >= TM.BoardYMax || x < 0 || y < 0)
        {
            return false;
        }
        area = this.transform.GetChild(x).GetChild(y).GetComponent<Area>();
        if (area.Bridge || area.pond)
        {
            return false;
        }
       
        return true;
    }
    public List<KomaIndex> BanCanMove(int[,] ban ,int N)
    {
        var CanMoves = new List<KomaIndex>();
            bool CanMove = AICan(ban, X[N] + 1, Y[N]);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(X[N] + 1, Y[N]));
            }
            CanMove = AICan(ban, X[N] - 1, Y[N]);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(X[N] - 1, Y[N]));
            }
            CanMove = AICan(ban, X[N], Y[N] - 1);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(X[N], Y[N] - 1));
            }
            CanMove = AICan(ban, X[N], Y[N] + 1);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(X[N], Y[N] + 1));
            }
            CanMove = AICan(ban, X[N] + 1, Y[N] + 1);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(X[N] + 1, Y[N] + 1));
            }
            CanMove = AICan(ban, X[N] - 1, Y[N] - 1);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(X[N] - 1, Y[N] - 1));
            }
            CanMove = AICan(ban, X[N] - 1, Y[N] + 1);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(X[N] - 1, Y[N] + 1));
            }
            CanMove = AICan(ban, X[N] + 1, Y[N] - 1);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(X[N] + 1, Y[N] - 1));
            }
            return CanMoves;
        
    }
    public List<KomaIndex> BanCanBuild(int[,] ban , int N)
    {
        var CanMoves = new List<KomaIndex>();
        bool CanMove = AIBuild(ban, X[N] + 1, Y[N]);
        if (CanMove)
        {
            CanMoves.Add(new KomaIndex(X[N] + 1, Y[N]));
        }
        CanMove = AIBuild(ban, X[N] - 1, Y[N]);
        if (CanMove)
        {
            CanMoves.Add(new KomaIndex(X[N] - 1, Y[N]));
        }
        CanMove = AIBuild(ban, X[N], Y[N] - 1);
        if (CanMove)
        {
            CanMoves.Add(new KomaIndex(X[N], Y[N] - 1));
        }
        CanMove = AIBuild(ban, X[N], Y[N] + 1);
        if (CanMove)
        {
            CanMoves.Add(new KomaIndex(X[N], Y[N] + 1));
        }
      
        return CanMoves;
    }
    public bool AICan(int[,] ban,int x , int y)
    {
        if (x >= TM.BoardXMax || y >= TM.BoardYMax || x < 0 || y < 0)
        {
            return false;
        }
        else if (ban[x ,y] == 10 || ban[x ,y] == -10 || ban[x ,y] == 11|| ban[x ,y] == -1 )
        {
            return false;
        }
        return true;
    }
    public bool AIBuild(int[,] ban, int x, int y)
    {
        if (x >= TM.BoardXMax || y >= TM.BoardYMax || x < 0 || y < 0)
        {
            return false;
        }
        else if ( ban[x, y] == 11 || ban[x, y] == 100 )
        {
            return false;
        }
        return true;
    }
    public int[,] Build(int x, int y , int[,] ban , bool Blue)
    {

       
        if(ban[x, y] == 10)
        {
            ban[x ,y] = 0;
        }
        if (Blue)
        {
            ban[x, y] = 10;
        }
        else
        {
            ban[x, y] = -10;
        }
        return ban;
    }
    public int[,] Move(int x , int y,int[,] ban ,int N )
    {   ban[X[N], Y[N]] = 0;
        ban[x, y] = 11;
        return ban;
    }
    public List<KomaIndex> GetCanBuild(int N)
    {

        var CanMoves = new List<KomaIndex>();
        CanMove = CanBuild(Y[N] + 1, X[N]);
        if (CanMove)
        {
            CanMoves.Add(new KomaIndex(X[N] + 1, Y[N]));        
        }
        CanMove = CanBuild(Y[N] - 1, X[N]);
        if (CanMove)
        {
            CanMoves.Add(new KomaIndex(X[N] - 1, Y[N]));
        }
        CanMove = CanBuild(Y[N], X[N] + 1);
        if (CanMove)
        {
            CanMoves.Add(new KomaIndex(X[N], Y[N] + 1));
        }
        CanMove = CanBuild(Y[N], X[N] - 1);
        if (CanMove)
        {
            CanMoves.Add(new KomaIndex(X[N], Y[N] - 1));
        }
        return CanMoves;

    }
    public void CheckPosition()
    {
        for (int n = 0; n < TM.PieceNumber * 2; n++)
        {
               montes = Boards.transform.GetChild(n).gameObject;
              BBB = montes.GetComponent<BridgeButtonManager>();
             //   Debug.Log(BBB.BoardX);
             //   Debug.Log(BBB.BoardY);
              X[n]  =   BBB.BoardX;
              Y[n]  =   BBB.BoardY;
            BanX[n] = BBB.BoardX;
            BanY[n] = BBB.BoardY;
            //   BBB.BoardX = Y[n];
            //   BBB.BoardY = X[n];
            // BB[n].BoardY = Y[n];
            // Debug.Log(montes);
        }
    }
 

}
 