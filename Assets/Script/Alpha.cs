using Koma;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Alpha : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    GameObject Board;
    GameObject TM;
    KomaCalulator komaCalulator;
    public TurnManager turnManager;
    public KomaIndex[] k = new KomaIndex[12];
    public Text txt;
    public bool Move;
    bool F = false;
    int castle = 100;
    int bluekoma_point = 10;
    int jinti_point = 30;
    private void Start()
    {
        Board = GameObject.Find("BoardBridge");
        TM = GameObject.Find("Board");
        turnManager = TM.GetComponent<TurnManager>();
        komaCalulator = TM.GetComponent<KomaCalulator>();

    }
    public int[,] EvaluateBuildStateScore = new[,] {
            {  1 , 1,   1,   1 ,  1 ,  1 ,  1 ,  10 ,  10 ,  10 ,  -1 },
            { 1 ,  0 ,  0 ,  0 ,  0 ,  0 ,  0 ,  10 ,  20 ,  30 ,  -1 },
            { 1 ,  0  , 0  , 10 ,  0 ,  0 ,  0 ,  10 ,  30 ,  20 ,  30 },
            { 1 ,  0 ,  0 ,  0 ,  -1 ,  -1 ,  -1 ,  20,   -1 ,  30,   10 },
            { 1   ,0 ,  0 ,  -1 ,  -1 ,  20 ,  -1 ,  10 ,  -1 ,  10 ,  1 },
            { 1  , 0  , -1 ,  -1 ,  20 ,  20 ,  20 ,  -1 ,  -1 ,  0 ,  1 },
            { 1  , 0 ,  -1 ,  10,      -1,   20  , -1 ,  -1,   0 ,  0 ,  1 },
            { 1  , 30  , -1 ,  20 ,  10 ,  1 ,  -1 , 0,   0 ,  0 ,  1 },
            { 30 ,  20 ,   30 ,  10  , 0 ,  0 ,  0 ,  0  , 0 ,  0 ,  1 },
            { -1 , 30 ,  5 ,  0  , 0 ,  0 ,  0 ,  0 ,  0  , 0 ,  1 },
            { -1 ,  1 ,  1 ,  1 ,  1 ,  1 ,  1 ,  1 ,  1  , 1 ,  1 },
      };
    public int[,] EvaluateMoveStateScore = new[,] {


           { 0 ,  0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0,  1 },
           {   0 ,   0 , 0 , 0 , 0 , 0 , 0 , 0 , 2 , 50,  -1 },
           {   0 ,   0 , 0 , 0 , 0 , 0 , 0 , 0 , 50 , 2,  50},
           {   0 ,   0 , 0 , 0 , -1 , -1 , -1 , 2 , 50 , 10,  0},
           {   0 ,   0 , 0 , -1 , -1 , 2 , -1 , 0 , -1 , 0,  0},
           {   0 ,   0 , -1 , -1 , 2 , 2 , 2 , -1 , -1 , 0,  0},
           {   0 ,   0 , -1 , 0 , -1 , 2 , -1 , -1 , 0 , 0,  0},
           {   0 ,   2 , -1 , 2 , -1 , -1 , -1 , 0 , 0 , 0,  0},
           {   50 ,   2 , 50 , 0 , 0 , 0 , 0 , 0 , 0 , 0,  0},
           {   50 ,   50 , 50 , 50 , 25 , 0 , 0 , 0 , 0 , 0,  0},
            {  -1 ,   0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0,  0}
      };


    public int EvaluateBuildStates(int[,] ban, int N)
    {
        // ban = komaCalulator.AIScoreCheck(ban);
        txt.text = "";
        var myscore = 0;
        var enemyscore = 0;
        for (int n = 0; n < turnManager.BlueBridges.transform.childCount; n++)
        {
            myscore += EvaluateMoveStateScore[komaCalulator.BlueX[n], komaCalulator.BlueY[n]];
        }
        for (int n = 0; n < turnManager.RedBridges.transform.childCount; n++)
        {
            enemyscore += EvaluateMoveStateScore[komaCalulator.RedX[n], komaCalulator.RedY[n]];

        }
        // komaCalulator.
        // komaCalulator.AIAreaCheckBlue(ban);
        for (int y = 0; y < turnManager.BoardYMax; y++)
        {
            txt.text += "\n";
            for (int x = 0; x < turnManager.BoardXMax; x++)
            {

                var Banscore = EvaluateBuildStateScore[x, y];
                if (ban[x, y] == 10)
                {
                    myscore += bluekoma_point + Banscore;
                }
                else if (ban[x, y] == 30)
                {
                    myscore += jinti_point + Banscore;
                }
                if (ban[x, y] == -10)
                {
                    enemyscore += bluekoma_point + Banscore;
                }
                if (ban[x, y] == -30)
                {
                    enemyscore += jinti_point + Banscore;
                }
                if (ban[x, y] == 130)
                {
                    myscore += castle + Banscore;
                }
                if (ban[x, y] == -130)
                {
                    enemyscore += castle;
                }
                txt.text += ban[x, y].ToString() + "  ";
            }

        }

        return myscore - enemyscore;
    }


    public void AlphaBeta(int depth, int[,] ban, int N, bool Bule)
    {
        KomaIndex resultKomaIndex = null;
        var alpha = -1;
        var beta = 0;
        var score = 0;
        var Buildscore = 0;
        var canMovekoma = komaCalulator.GetCanMoveIndex(N, Bule);//Œš’z‚ª‚È‚¢
        foreach (var putkomaStateIndex in canMovekoma)
        {


            var Ban = ban;
            if (putkomaStateIndex.Build == true)
            {
                Ban = komaCalulator.Build(putkomaStateIndex.X, putkomaStateIndex.Y, ban, Bule);
                Buildscore = GetAllCanMoveSearch(Ban, depth - 1, alpha, beta, N, true);
                if (alpha < Buildscore)
                {
                    alpha = Buildscore;
                    resultKomaIndex = putkomaStateIndex;
                }
            }
            else
            {
                Ban = komaCalulator.Move(putkomaStateIndex.X, putkomaStateIndex.Y, ban, N, Bule);
                if (Bule)
                {

                    komaCalulator.BlueX[N] = putkomaStateIndex.X;
                    komaCalulator.BlueY[N] = putkomaStateIndex.Y;
                }
                else
                {
                    komaCalulator.RedX[N] = putkomaStateIndex.X;
                    komaCalulator.RedY[N] = putkomaStateIndex.Y;
                }
            }

            score = GetAllCanMoveSearch(Ban, depth - 1, alpha, beta, N, true);
            if (alpha < score)
            {
                alpha = score;
                resultKomaIndex = putkomaStateIndex;
            }
        }
        if (score > Buildscore)
        {
            if (Bule)
            {
                for (int n = 0; n < turnManager.BlueBridges.transform.childCount; n++)
                {
                    GameObject komaR = turnManager.BlueBridges.transform.GetChild(n).gameObject;
                    komaCalulator.bb[n] = komaR.GetComponent<BridgeButtonManager>();
                    komaCalulator.BlueX[N] = komaCalulator.bb[n].BoardX;
                    komaCalulator.BlueY[N] = komaCalulator.bb[n].BoardY;

                }
                GameObject koma = turnManager.BlueBridges.transform.GetChild(N).gameObject;
                koma.transform.position = turnManager.MoveBridge(resultKomaIndex.Y, resultKomaIndex.X);
                komaCalulator.BlueX[N] = resultKomaIndex.X;
                komaCalulator.BlueY[N] = resultKomaIndex.Y;
            }
            if (!Bule)
            {
                for (int n = 0; n < turnManager.RedBridges.transform.childCount; n++)
                {
                    GameObject komaR = turnManager.RedBridges.transform.GetChild(n).gameObject;
                    komaCalulator.bb[n] = komaR.GetComponent<BridgeButtonManager>();
                    komaCalulator.RedX[N] = komaCalulator.bb[n].BoardX;
                    komaCalulator.RedY[N] = komaCalulator.bb[n].BoardY;

                }
                GameObject koma = turnManager.RedBridges.transform.GetChild(N).gameObject;
                koma.transform.position = turnManager.MoveBridge(resultKomaIndex.Y, resultKomaIndex.X);
                komaCalulator.RedX[N] = resultKomaIndex.X;
                komaCalulator.RedY[N] = resultKomaIndex.Y;
            }


        }
        else
        {
            turnManager.BuildAndDestroyBridge(resultKomaIndex.Y, resultKomaIndex.X);
            for (int n = 0; n < turnManager.RedBridges.transform.childCount; n++)
            {
                GameObject komaR = turnManager.RedBridges.transform.GetChild(n).gameObject;
                GameObject komaB = turnManager.BlueBridges.transform.GetChild(n).gameObject;
                komaCalulator.bb[n] = komaR.GetComponent<BridgeButtonManager>();
                komaCalulator.RedX[N] = komaCalulator.bb[n].BoardX;
                komaCalulator.RedY[N] = komaCalulator.bb[n].BoardY;
                komaCalulator.bb[n] = komaB.GetComponent<BridgeButtonManager>();
                komaCalulator.BlueX[N] = komaCalulator.bb[n].BoardX;
                komaCalulator.BlueY[N] = komaCalulator.bb[n].BoardY;
            }
        }
    }
    public int GetAllCanMoveSearch(int[,] ban, int depth, int alpha, int beta, int N, bool First)
    {
        if (depth == 0) return EvaluateBuildStates(ban, N);
        var MaxScore = 0;
        komaCalulator.AIBlueTurn = !komaCalulator.AIBlueTurn;
        var canMovekoma = komaCalulator.BanCanMove(ban, N, komaCalulator.AIBlueTurn);                                                                         //  var canMovekoma = komaCalulator.BanCanBuild(ban,  N);
        foreach (var putkomaStateIndex in canMovekoma)
        {
            var Ban = ban;
            if (putkomaStateIndex.Build == true)
            {
                Ban = komaCalulator.Build(putkomaStateIndex.X, putkomaStateIndex.Y, ban, komaCalulator.AIBlueTurn);
               
            }
            else
            {
                Ban = komaCalulator.Move(putkomaStateIndex.X, putkomaStateIndex.Y, ban, N, komaCalulator.AIBlueTurn);
                if (komaCalulator.AIBlueTurn)
                {
                    komaCalulator.BlueX[N] = putkomaStateIndex.X;
                    komaCalulator.BlueY[N] = putkomaStateIndex.Y;
                }
                else
                {
                    komaCalulator.RedX[N] = putkomaStateIndex.X;
                    komaCalulator.RedY[N] = putkomaStateIndex.Y;
                }
               
            }
            N += 1;
            ban = Ban;
            var childMovekoma = komaCalulator.BanCanMove(ban, N, komaCalulator.AIBlueTurn);
            foreach (var child in childMovekoma)
            {
                if (child.Build == true)
                {
                    Ban = komaCalulator.Build(child.X, child.Y, ban, komaCalulator.AIBlueTurn);
                   
                }
                else
                {
                    Ban = komaCalulator.Move(child.X, child.Y, ban, N, komaCalulator.AIBlueTurn);
                    if (komaCalulator.AIBlueTurn)
                    {
                        komaCalulator.BlueX[N] = putkomaStateIndex.X;
                        komaCalulator.BlueY[N] = putkomaStateIndex.Y;
                    }
                    else
                    {
                        komaCalulator.RedX[N] = putkomaStateIndex.X;
                        komaCalulator.RedY[N] = putkomaStateIndex.Y;
                    }
                }
                if (N != turnManager.RedBridges.transform.childCount -1)
                {
                    N += 1;
                    var ChildKoma1 = komaCalulator.BanCanMove(ban, N , komaCalulator.AIBlueTurn);
                    foreach (var childKoma1 in childMovekoma)
                    {
                        if (childKoma1.Build == true)
                        {
                            Ban = komaCalulator.Build(childKoma1.X, childKoma1.Y, ban, komaCalulator.AIBlueTurn);

                        }
                        else
                        {
                            Ban = komaCalulator.Move(childKoma1.X, childKoma1.Y, ban, N, komaCalulator.AIBlueTurn);
                            if (komaCalulator.AIBlueTurn)
                            {

                                komaCalulator.BlueX[N] = putkomaStateIndex.X;
                                komaCalulator.BlueY[N] = putkomaStateIndex.Y;
                            }
                            else
                            {
                                komaCalulator.RedX[N] = putkomaStateIndex.X;
                                komaCalulator.RedY[N] = putkomaStateIndex.Y;
                            }
                        }
                     
                       if (N != turnManager.RedBridges.transform.childCount - 1)
                        {
                            /*  var ChildKoma2 = komaCalulator.BanCanMove(ban, N + 2, komaCalulator.AIBlueTurn);
                              foreach (var childKoma2 in childMovekoma)
                              {
                                  if (childKoma2.Build == true)
                                  {
                                      Ban = komaCalulator.Build(childKoma2.X, childKoma2.Y, ban, komaCalulator.AIBlueTurn);

                                  }
                                  else
                                  {
                                      Ban = komaCalulator.Move(putkomaStateIndex.X, putkomaStateIndex.Y, ban, N, komaCalulator.AIBlueTurn);
                                      if (komaCalulator.AIBlueTurn)
                                      {

                                          komaCalulator.BlueX[N] = putkomaStateIndex.X;
                                          komaCalulator.BlueY[N] = putkomaStateIndex.Y;
                                      }
                                      else
                                      {
                                          komaCalulator.RedX[N] = putkomaStateIndex.X;
                                          komaCalulator.RedY[N] = putkomaStateIndex.Y;
                                      }
                                  }
                                  if (N != turnManager.RedBridges.transform.childCount -1)
                                  {

                                  }
                                  else
                                  {
                                      var score = GetAllCanMoveSearch(Ban, depth - 1, alpha, beta, 0, true);
                                      if (score > beta)
                                      {
                                          MaxScore = beta;
                                      }

                                  }
                             */
                        }
                        else
                        {
                            var score = GetAllCanMoveSearch(Ban, depth - 1, alpha, beta, 0, true);
                            if(score > beta && F ==true) return score;

                            if (score > beta)
                            {
                                beta = score;
                                score = MaxScore;
                            }
                            alpha = Mathf.Max(alpha, score);
                            MaxScore = Mathf.Max(MaxScore, score);
                            Ban[childKoma1.X, childKoma1.Y] = 0;
                        }
                         F = true;

                    }
                       
                    }
                    else
                    {
                    var score = GetAllCanMoveSearch(Ban, depth - 1, alpha, beta, 0, true);
                    if (score > beta && F == true) return score;
                    /*    if (score > beta)
                        {
                            beta = score;
                            score = MaxScore;
                        }
                    */
                    alpha = Mathf.Max(alpha, score);
                    MaxScore = Mathf.Max(MaxScore, score);
                    Ban[child.X, child.Y] = 0;
                    }
                  F = true;



            }

        }
        return MaxScore;
    }
      
}


/*if (N == turnManager.RedBridges.transform.childCount - 1)
                 {
                     N = 0;
                     score = GetAllCanMoveSearch(Ban, depth - 1, alpha, beta, N, true);
                     alpha = Mathf.Max(alpha, score);
                     MaxScore = Mathf.Max(MaxScore, score);
                 }
                 else
                 {
                     score = GetAllCanMoveSearch(Ban, depth, alpha, beta, N + 1, false);
                     alpha = Mathf.Max(alpha, score);
                     MaxScore = Mathf.Max(MaxScore, score);
                 }
                 */
