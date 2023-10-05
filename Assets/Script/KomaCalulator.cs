using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Koma;
using UnityEditor;
using UnityEditorInternal;
using static UnityEditor.VersionControl.Asset;
using System.Runtime;
using  System.Linq;
using System;
using System.Data;

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
    public KomaIndex[] komaIndex = new KomaIndex[12];
   public int[] RedX = new int[12];
   public int[] RedY = new int[12];
   public int[] BlueY = new int[12];
   public int[] BlueX = new int[12];
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
        AIBanState();
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
    public int[,] AIdown(int N, bool Bule)
    {
        if (Bule)
        {
            bool can = AICanMove(BlueX[N] + 1, BlueY[N]);

            if (can)
            {

                Ban[BlueX[N], BlueY[N]] -= 11;
                Ban[BlueX[N] + 1, BlueY[N]] += 11;
                BlueY[N] += 1;
                return Ban;
            }
        }
            else 
            {
               bool can = AICanMove(RedX[N] + 1, RedY[N]);

                if (can)
                {

                    Ban[RedX[N], RedY[N]] -= 11;
                    Ban[RedX[N] + 1, RedY[N]] += 11;
                    RedY[N] += 1;
                    return Ban;
                }



            }
        return Ban;

    }
    public int[,] AIRight(int N , bool Bule)
    {
        if (Bule)
        {

            bool can = AICanMove(BlueX[N], BlueY[N] - 1);

            if (can)
            {

                Ban[BlueX[N], BlueY[N]] -= 11;
                Ban[BlueX[N], BlueY[N] - 1] += 11;
                BlueX[N] -= 1;
            }
        }
        else
        {
            bool can = AICanMove(RedX[N], RedY[N] - 1);

            if (can)
            {

                Ban[RedX[N], RedY[N]] -= 11;
                Ban[RedX[N], RedY[N] - 1] += 11;
                RedX[N] -= 1;
            }
        }
        return Ban;
    }
    public int[,] AILeft(int N , bool Blue )
    {

        if (Blue)
        {

            bool can = AICanMove(BlueX[N], BlueY[N] + 1);

            if (can)
            {

                Ban[BlueX[N], BlueY[N]] -= 11;
                Ban[BlueX[N], BlueY[N] + 1] += 11;
                BlueX[N] += 1;

            }
        }
        else
        {
            bool can = AICanMove(RedX[N], RedY[N] + 1);

            if (can)
            {

                Ban[RedX[N], RedY[N]] -= 11;
                Ban[RedX[N], RedY[N] + 1] += 11;
                RedX[N] += 1;

            }
        }
      
        return Ban;
    }
    public int[,] AIUP(int N , bool Blue)
    {
        if (Blue)
        {

            bool can = AICanMove(BlueX[N] - 1, BlueY[N]);

            if (can)
            {

                Ban[BlueX[N], BlueY[N]] -= 11;
                Ban[BlueX[N] - 1, BlueY[N]] += 11;
                BlueX[N] -= 1;

            }

        }
        else
        {
            bool can = AICanMove(RedX[N] - 1, RedY[N]);

            if (can)
            {

                Ban[RedX[N], RedY[N]] -= 11;
                Ban[RedX[N] - 1, RedY[N]] += 11;
                RedX[N] -= 1;

            }

        }

        return Ban;
    }
    public int[,] AIHidarisita(int N , bool Blue)
    {

        if (Blue)
        {

            bool can = AICanMove(BlueX[N] - 1, BlueY[N] + 1);

            if (can)
            {

                Ban[BlueX[N], BlueY[N]] -= 11;
                Ban[BlueX[N] - 1, BlueY[N] + 1] += 11;
                BlueX[N] -= 1;
                BlueY[N] += 1;
            }
        }
        else
        {
            bool can = AICanMove(RedX[N] - 1, RedY[N] + 1);

            if (can)
            {

                Ban[RedX[N], RedY[N]] -= 11;
                Ban[RedX[N] - 1, RedY[N] +1] += 11;
                RedX[N] -= 1;
                RedY[N] += 1;

            }

        }
       
        return Ban;
    }
    public int[,] AIMigisita(int N , bool Blue)
    {
        if (Blue)
        {

            bool can = AICanMove(BlueX[N] + 1, BlueY[N] + 1);

            if (can)
            {

                Ban[BlueX[N], BlueY[N]] -= 11;
                Ban[BlueX[N] + 1, BlueY[N] + 1] += 11;
                BlueX[N] += 1;
                BlueY[N] += 1;
            }
        }
        else
        {
            bool can = AICanMove(RedX[N] + 1, RedY[N] + 1);

            if (can)
            {

                Ban[RedX[N], RedY[N]] -= 11;
                Ban[RedX[N] + 1, RedY[N] + 1] += 11;
                RedX[N] += 1;
                RedY[N] += 1;

            }

        }
        return Ban;
    }
    public int[,] AImigiue(int N , bool Blue)
    {
        if (Blue)
        {

            bool can = AICanMove(BlueX[N] + 1, BlueY[N] - 1);

            if (can)
            {

                Ban[BlueX[N], BlueY[N]] -= 11;
                Ban[BlueX[N] + 1, BlueY[N] - 1] += 11;
                BlueX[N] += 1;
                BlueY[N] -= 1;
            }
        }
        else
        {
            bool can = AICanMove(RedX[N] + 1, RedY[N] - 1);

            if (can)
            {

                Ban[RedX[N], RedY[N]] -= 11;
                Ban[RedX[N] + 1, RedY[N] - 1] += 11;
                RedX[N] += 1;
                RedY[N] -= 1;

            }

        }
        return Ban;
    }
    public int[,] AIhidariue(int N, bool Blue)
    {



        if (Blue)
        {

            bool can = AICanMove(BlueX[N] - 1, BlueY[N] - 1);

            if (can)
            {

                Ban[BlueX[N], BlueY[N]] -= 11;
                Ban[BlueX[N] - 1, BlueY[N] - 1] += 11;
                BlueX[N] -= 1;
                BlueY[N] -= 1;
            }
        }
        else
        {
            bool can = AICanMove(RedX[N] - 1, RedY[N] +- 1);

            if (can)
            {

                Ban[RedX[N], RedY[N]] -= 11;
                Ban[RedX[N] - 1, RedY[N] - 1] += 11;
                RedX[N] -= 1;
                RedY[N] -= 1;

            }

        }

        return Ban;
    }
    public int[,] BuildDown(int N ,bool Blue)
    {
        if (Blue)
        {
            int can = AICanBuildDestoroy(BlueX[N], BlueY[N] + 1);
            if (can == 1)
            {
                Ban[BlueX[N] , BlueY[N] +1] = 0;
            }
            else if (can == 2)
            {
                Ban[BlueX[N] , BlueY[N] +1] = 10;

            }
        }
        else
        {
            int can = AICanBuildDestoroy(RedX[N], RedY[N] + 1);
            if (can == 1)
            {
                Ban[RedX[N], RedY[N] + 1] = 0;
            }
            else if (can == 2)
            {
                Ban[RedX[N], RedY[N] + 1] = -10;

            }
        }
        PlayerCount++;
        return Ban;
    }
    public int[,] BuildUp(int N , bool Blue)
    {
        if (Blue)
        {
            int can = AICanBuildDestoroy(BlueX[N], BlueY[N] - 1);
            if (can == 1)
            {
                Ban[BlueX[N], BlueY[N] - 1] = 0;
            }
            else if (can == 2)
            {
                Ban[BlueX[N], BlueY[N] - 1] = 10;

            }
        }
        else
        {
            int can = AICanBuildDestoroy(RedX[N], RedY[N] - 1);
            if (can == 1)
            {
                Ban[RedX[N], RedY[N] + 1] = 0;
            }
            else if (can == 2)
            {
                Ban[RedX[N], RedY[N] + 1] = -10;

            }
        }

       
        return Ban;
    }
    public int[,] BuildLeft(int N , bool Blue)
    {
        if (Blue)
        {
            int can = AICanBuildDestoroy(BlueX[N] -1, BlueY[N] );
            if (can == 1)
            {
                Ban[BlueX[N] -1, BlueY[N]] = 0;
            }
            else if (can == 2)
            {
                Ban[BlueX[N] -1, BlueY[N] ] = 10;

            }
        }
        else
        {
            int can = AICanBuildDestoroy(RedX[N] -1, RedY[N] );
            if (can == 1)
            {
                Ban[RedX[N] -1, RedY[N] ] = 0;
            }
            else if (can == 2)
            {
                Ban[RedX[N] -1, RedY[N] ] = -10;

            }
        }

        PlayerCount++;
        return Ban;
    }
    public int[,] BuildRight(int N , bool Blue)
    {
        if (Blue)
        {
            int can = AICanBuildDestoroy(BlueX[N] + 1, BlueY[N]);
            if (can == 1)
            {
                Ban[BlueX[N] + 1, BlueY[N]] = 0;
            }
            else if (can == 2)
            {
                Ban[BlueX[N] + 1, BlueY[N]] = 10;

            }
        }
        else
        {
            int can = AICanBuildDestoroy(RedX[N] + 1, RedY[N]);
            if (can == 1)
            {
                Ban[RedX[N] + 1, RedY[N]] = 0;
            }
            else if (can == 2)
            {
                Ban[RedX[N] + 1, RedY[N]] = -10;

            }
        }

        PlayerCount++;
        return Ban;
    }

    public int[,] Randam(int N, bool Blue)
    {
        if (AIBlueTurn == false)
        {
            N += TM.PieceNumber;  
        }
        if (Blue)
        {
            int rnd = UnityEngine.Random.Range(1, 13);
            if (rnd == 1)
            {
                Ban = AIdown(N , true);
                PlayerCount++;
                return Ban;

            }
            if (rnd == 2)
            {
                AILeft(N, true);
                PlayerCount++;
                return Ban;
            }
            if (rnd == 3)
            {
                AIRight(N, true);
                PlayerCount++;
                return Ban;
            }
            if (rnd == 4)
            {
                AIUP(N, true);
                PlayerCount++;
                return Ban;
            }
            if (rnd == 5)
            {
                AIHidarisita(N, true);
                PlayerCount++;
                return Ban;
            }
            if (rnd == 6)
            {
                AIMigisita(N, true);
                PlayerCount++;
                return Ban;
            }
            if (rnd == 7)
            {
                AIhidariue(N, true);
                PlayerCount++;
                return Ban;
            }
            if (rnd == 8)
            {
                AImigiue(N, true);
                PlayerCount++;
                return Ban;

            }
            if (rnd == 9)
            {
                BuildUp(N, true);
                PlayerCount++;
                return Ban;
            }
            if (rnd == 10)
            {
                BuildDown(N, true);
                PlayerCount++;
                return Ban;
            }
            if (rnd == 11)
            {
                BuildRight(N, true);
                PlayerCount++;
                return Ban;
            }
            if (rnd == 12)
            {
                BuildLeft(N, true);
                PlayerCount++;
                return Ban;

            }
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
            else
            {

                return 2;
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

    
    public List<KomaIndex> GetCanMoveIndex(int N , bool Blue)
    {
        var CanMoves = new List<KomaIndex>();
        if (Blue)
        {
            
            CanMove = AICanMoves(BlueY[N], BlueX[N] + 1);
            if (CanMove)
            {

                CanMoves.Add(new KomaIndex(BlueX[N] + 1, BlueY[N], false));
            }
            CanMove = AICanMoves(BlueY[N], BlueX[N] - 1);
            if (CanMove)
            {

                CanMoves.Add(new KomaIndex(BlueX[N] - 1, BlueY[N], false));
            }
            CanMove = AICanMoves(BlueY[N] + 1, BlueX[N]);
            if (CanMove)
            {

                CanMoves.Add(new KomaIndex(BlueX[N], BlueY[N] + 1, false));
            }
            CanMove = AICanMoves(BlueY[N] - 1, BlueX[N]);
            if (CanMove)
            {

                CanMoves.Add(new KomaIndex(BlueX[N], BlueY[N] - 1, false));
            }
            CanMove = AICanMoves(BlueY[N] - 1, BlueX[N] - 1);

            if (CanMove)
            {

                CanMoves.Add(new KomaIndex(BlueX[N] - 1, BlueY[N] - 1, false));
            }
            CanMove = AICanMoves(BlueY[N] - 1, BlueX[N] + 1);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(BlueX[N] + 1, BlueY[N] - 1, false));
            }
            CanMove = AICanMoves(BlueY[N] + 1, BlueX[N] - 1);
            if (CanMove)
            {

                CanMoves.Add(new KomaIndex(BlueX[N] - 1, BlueY[N] + 1, false));
            }
            CanMove = AICanMoves(BlueY[N] + 1, BlueX[N] + 1);
            if (CanMove)
            {

                CanMoves.Add(new KomaIndex(BlueX[N] + 1, BlueY[N] + 1, false));
            }

            CanMove = CanBuild(BlueY[N], BlueX[N] + 1);
            if (CanMove)
            {

                CanMoves.Add(new KomaIndex(BlueX[N] + 1, BlueY[N], false));
            }
            CanMove = CanBuild(BlueY[N], BlueX[N] - 1);
            if (CanMove)
            {

                CanMoves.Add(new KomaIndex(BlueX[N] - 1, BlueY[N], false));
            }
            CanMove = CanBuild(BlueY[N] + 1, BlueX[N]);
            if (CanMove)
            {

                CanMoves.Add(new KomaIndex(BlueX[N], BlueY[N] + 1, false));
            }
            CanMove = CanBuild(BlueY[N] - 1, BlueX[N]);
            if (CanMove)
            {

                CanMoves.Add(new KomaIndex(BlueX[N], BlueY[N] - 1, false));
            }
        }
        else
        {
            CanMove = AICanMoves(RedY[N], RedX[N] + 1);
            if (CanMove)
            {

                CanMoves.Add(new KomaIndex(RedX[N] + 1, RedY[N], false));
            }
            CanMove = AICanMoves(RedY[N], RedX[N] - 1);
            if (CanMove)
            {

                CanMoves.Add(new KomaIndex(RedX[N] - 1, RedY[N], false));
            }
            CanMove = AICanMoves(RedY[N] + 1, RedX[N]);
            if (CanMove)
            {

                CanMoves.Add(new KomaIndex(RedX[N], RedY[N] + 1, false));
            }
            CanMove = AICanMoves(RedY[N] - 1, RedX[N]);
            if (CanMove)
            {

                CanMoves.Add(new KomaIndex(RedX[N], RedY[N] - 1, false));
            }
            CanMove = AICanMoves(RedY[N] - 1, RedX[N] - 1);

            if (CanMove)
            {

                CanMoves.Add(new KomaIndex(RedX[N] - 1, RedY[N] - 1, false));
            }
            CanMove = AICanMoves(RedY[N] - 1, RedX[N] + 1);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(RedX[N] + 1,     RedY[N] - 1, false));
            }
            CanMove = AICanMoves(RedY[N] + 1, RedX[N] - 1);
            if (CanMove)
            {

                CanMoves.Add(new KomaIndex(RedX[N] - 1, RedY[N] + 1, false));
            }
            CanMove = AICanMoves(RedY[N] + 1, RedX[N] + 1);
            if (CanMove)
            {

                CanMoves.Add(new KomaIndex(RedX[N] + 1, RedY[N] + 1, false));
            }

            CanMove = CanBuild(BlueY[N], BlueX[N] + 1);
            if (CanMove)
            {

                CanMoves.Add(new KomaIndex(BlueX[N] + 1, BlueY[N], false));
            }
            CanMove = CanBuild(BlueY[N], BlueX[N] - 1);
            if (CanMove)
            {

                CanMoves.Add(new KomaIndex(BlueX[N] - 1, BlueY[N], false));
            }
            CanMove = CanBuild(BlueY[N] + 1, BlueX[N]);
            if (CanMove)
            {

                CanMoves.Add(new KomaIndex(BlueX[N], BlueY[N] + 1, false));
            }
            CanMove = CanBuild(BlueY[N] - 1, BlueX[N]);
            if (CanMove)
            {

                CanMoves.Add(new KomaIndex(BlueX[N], BlueY[N] - 1, false));
            }
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
        if (area.Bridge || area.pond || area.BlueWall || area.RedWall)
        {
            return false;
        }
       
        return true;
    }

    public List<KomaIndex> BanCanMove( int[,] ban, int N , bool Blue)
    {
        var CanMoves = new List<KomaIndex>();
        if (Blue)
        {
            bool CanMove = AICan(ban, BlueX[N] + 1, BlueY[N]);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(BlueX[N] + 1, BlueY[N], false));
            }
            CanMove = AICan(ban, BlueX[N] - 1, BlueY[N]);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(BlueX[N] - 1, BlueY[N], false));
            }
            CanMove = AICan(ban, BlueX[N], BlueY[N] - 1);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(BlueX[N], BlueY[N] - 1, false));
            }
            CanMove = AICan(ban, BlueX[N], BlueY[N] + 1);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(BlueX[N], BlueY[N] + 1, false));
            }
            CanMove = AICan(ban, BlueX[N] + 1, BlueY[N] + 1);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(BlueX[N] + 1, BlueY[N] + 1, false));
            }
            CanMove = AICan(ban, BlueX[N] - 1, BlueY[N] - 1);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(BlueX[N] - 1, BlueY[N] - 1, false));
            }
            CanMove = AICan(ban, BlueX[N] - 1, BlueY[N] + 1);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(BlueX[N] - 1, BlueY[N] + 1, false));
            }
            CanMove = AICan(ban, BlueX[N] + 1, BlueY[N] - 1);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(BlueX[N] + 1, BlueY[N] - 1, false));
            }

            CanMove = AIBuild(ban, BlueX[N] + 1, BlueY[N]);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(BlueX[N] + 1, BlueY[N], true));
            }
            CanMove = AIBuild(ban, BlueX[N] - 1, BlueY[N]);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(BlueX[N] - 1, BlueY[N], true));
            }
            CanMove = AIBuild(ban, BlueX[N], BlueY[N] - 1);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(BlueX[N], BlueY[N] - 1, true));
            }
            CanMove = AIBuild(ban, BlueX[N], BlueY[N] + 1);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(BlueX[N], BlueY[N] + 1, true));
            }
        }
        else
        {
           bool CanMove = AIBuild(ban, RedX[N] + 1, RedY[N]);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(RedX[N] + 1, RedY[N], true));
            }
            CanMove = AIBuild(ban, RedX[N] - 1, RedY[N]);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(RedX[N] - 1, RedY[N], true));
            }
            CanMove = AIBuild(ban, RedX[N], RedY[N] - 1);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(RedX[N], RedY[N] - 1, true));
            }
            CanMove = AIBuild(ban, RedX[N], RedY[N] + 1);
            if (CanMove)
            {

                CanMoves.Add(new KomaIndex(RedX[N], RedY[N] + 1, true));
            }
             CanMove = AICan(ban, RedX[N] + 1, RedY[N]);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(RedX[N] + 1, RedY[N], false));
            }
            CanMove = AICan(ban, RedX[N] - 1, RedY[N]);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(RedX[N] - 1, RedY[N], false));
            }
            CanMove = AICan(ban, RedX[N], RedY[N] - 1);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(RedX[N], RedY[N] - 1, false));
            }
            CanMove = AICan(ban, RedX[N], RedY[N] + 1);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(RedX[N], RedY[N] + 1, false));
            }
            CanMove = AICan(ban, RedX[N] + 1, RedY[N] + 1);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(RedX[N] + 1, RedY[N] + 1, false));
            }
            CanMove = AICan(ban, BlueX[N] - 1, BlueY[N] - 1);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(RedX[N] - 1, RedY[N] - 1, false));
            }
            CanMove = AICan(ban, RedX[N] - 1, RedY[N] + 1);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(RedX[N] - 1, RedY[N] + 1, false));
            }
            CanMove = AICan(ban, RedX[N] + 1, RedY[N] - 1);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(RedX[N] + 1, RedY[N] - 1, false));
            }

          
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
    public int[,] Move(int x , int y,int[,] ban ,int N , bool Blue )

    {
        if (Blue)
        {
            ban[BlueX[N], BlueY[N]] = 0;
        
        }
        else
        {
            ban[RedX[N], RedY[N]] = 0;
        }
        ban[x, y] = 11;
        return ban;
    }
    public List<KomaIndex> GetCanBuild(int N , bool Build)
    {
        var CanMoves = new List<KomaIndex>();
        if (Build)
        {
           
            CanMove = CanBuild(BlueY[N] + 1, BlueX[N]);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(BlueX[N] + 1, BlueY[N], true));
            }
            CanMove = CanBuild(BlueY[N] - 1, BlueX[N]);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(BlueX[N] - 1, BlueY[N], true));
            }
            CanMove = CanBuild(BlueY[N], BlueX[N] + 1);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(BlueX[N], BlueY[N] + 1, true));
            }
            CanMove = CanBuild(BlueY[N], BlueX[N] - 1);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(BlueX[N], BlueY[N] - 1, true));
            }
        }
        else
        {
            CanMove = CanBuild(RedY[N] + 1, RedX[N]);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(RedX[N] + 1, RedY[N], true));
            }
            CanMove = CanBuild(RedY[N] - 1, RedX[N]);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(RedX[N] - 1, RedY[N], true));
            }
            CanMove = CanBuild(RedY[N], RedX[N] + 1);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(RedX[N], RedY[N] + 1, true));
            }
            CanMove = CanBuild(RedY[N], RedX[N] - 1);
            if (CanMove)
            {
                CanMoves.Add(new KomaIndex(RedX[N], RedY[N] - 1, true));
            }

        }
        return CanMoves;

    }
    public void CheckPosition()
    {
        for (int n = 0; n < TM.BlueBridges.transform.childCount; n++)
        {
            montes = TM.BlueBridges.transform.GetChild(n).gameObject;// Boards.transform.GetChild(n).gameObject;
              BBB = montes.GetComponent<BridgeButtonManager>();
              BlueX[n]  =   BBB.BoardX;
              BlueY[n]  =   BBB.BoardY;
            BanX[n] = BBB.BoardX;
            BanY[n] = BBB.BoardY;
          
        }
        for(int n = 0; n < TM.RedBridges.transform.childCount; n++)
        {
            montes = TM.RedBridges.transform.GetChild(n).gameObject;// Boards.transform.GetChild(n).gameObject;
            BBB = montes.GetComponent<BridgeButtonManager>();
            //   Debug.Log(BBB.BoardX);
            //   Debug.Log(BBB.BoardY);
            RedX[n] = BBB.BoardX;
            RedY[n] = BBB.BoardY;
            BanX[n] = BBB.BoardX;
            BanY[n] = BBB.BoardY;
           
        }
    }
 

}
 