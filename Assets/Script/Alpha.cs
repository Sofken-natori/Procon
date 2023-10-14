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
    [Header("ï]âøä÷êî"), SerializeField] C11 c; 
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


    public int EvaluateBuildStates(int[,] ban)
    {
        // ban = komaCalulator.AIScoreCheck(ban);
      
        var myscore = 0;
        var enemyscore = 0;
        for (int n = 0; n < turnManager.BlueBridges.transform.childCount ; n++)
        {
            myscore += EvaluateMoveStateScore[komaCalulator.BlueX[n], komaCalulator.BlueY[n]];
        }
        for (int n = 0; n < turnManager.RedBridges.transform.childCount ; n++)
        {
            enemyscore += EvaluateMoveStateScore[komaCalulator.RedX[n], komaCalulator.RedY[n]];

        }
        // komaCalulator.
        komaCalulator.AIAreaCheckBlue(ban);
        komaCalulator.AIAreaCheckRed(ban);
        for (int y = 0; y < turnManager.BoardYMax; y++)
        {
            
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
                else if (ban[x, y] == -10)
                {
                    enemyscore += bluekoma_point + Banscore;
                }
                else if (ban[x, y] == -30)
                {
                    enemyscore += jinti_point + Banscore;
                }
                else if (ban[x, y] == 130)
                {
                    myscore += castle + Banscore;
                }
                else if (ban[x, y] == -130)
                {
                    enemyscore += castle;
                }
                else if (ban[x, y] == 15)
                {
                    enemyscore += jinti_point + Banscore;
                    myscore += jinti_point + Banscore;
                }
                
               
            }

        }
     // Debug.Log(myscore);
        return myscore - enemyscore;
    }

    public void Update()
    {
      
    }
    public void AlphaBeta(int depth, int[,] ban, int N, bool Bule)
    {
      
        KomaIndex resultKomaIndex = null;
        var alpha = -1;
        var beta = 0;
        var score = 0;
        var Buildscore = 0;
        var canMovekoma = komaCalulator.GetCanMoveIndex(N, Bule);
        foreach (var putkomaStateIndex in canMovekoma)
        {
            int X = 0;
            var Ban = ban;
            if (putkomaStateIndex.Build == true)
            {
                
                Ban = komaCalulator.Build(putkomaStateIndex.X, putkomaStateIndex.Y, ban, Bule);
                Buildscore = GetAllCanMoveSearch(Ban, depth - 0, alpha, beta, N, true);
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

            score = GetAllCanMoveSearch(Ban, depth - 1, alpha, beta, 0, true);
            if (alpha < score)
            {
                alpha = score;
                resultKomaIndex = putkomaStateIndex;
            }
            if(putkomaStateIndex.Build == true)
            {
                Ban[putkomaStateIndex.X, putkomaStateIndex.Y] = X;
            }
        }
        
        if (score > Buildscore)
        {
            
            if (Bule)
            {
                for (int n = 0; n < turnManager.BlueBridges.transform.childCount; n++)
                {
                    GameObject komaR = turnManager.RedBridges.transform.GetChild(n).gameObject;
                    GameObject komaB = turnManager.BlueBridges.transform.GetChild(n).gameObject;
                    komaCalulator.bb[n] = komaR.GetComponent<BridgeButtonManager>();
                    komaCalulator.RedX[n] = komaCalulator.bb[n].BoardX;
                    komaCalulator.RedY[n] = komaCalulator.bb[n].BoardY;
                    komaCalulator.bb[n] = komaB.GetComponent<BridgeButtonManager>();
                    komaCalulator.BlueX[n] = komaCalulator.bb[n].BoardX;
                    komaCalulator.BlueY[n] = komaCalulator.bb[n].BoardY;

                }
                GameObject koma = turnManager.BlueBridges.transform.GetChild(N).gameObject;
                koma.transform.position = turnManager.MoveBridge(resultKomaIndex.Y, resultKomaIndex.X,N);
                komaCalulator.BlueX[N] = resultKomaIndex.X;
                komaCalulator.BlueY[N] = resultKomaIndex.Y;
                komaCalulator.bb[N].BoardX = resultKomaIndex.X;
                komaCalulator.bb[N].BoardY = resultKomaIndex.Y;
            }
            if (!Bule)
            {
                for (int n = 0; n < turnManager.RedBridges.transform.childCount; n++)
                {
                    GameObject komaR = turnManager.RedBridges.transform.GetChild(n).gameObject;
                    GameObject komaB = turnManager.BlueBridges.transform.GetChild(n).gameObject;
                    komaCalulator.bb[n] = komaR.GetComponent<BridgeButtonManager>();
                    komaCalulator.RedX[n] = komaCalulator.bb[n].BoardX;
                    komaCalulator.RedY[n] = komaCalulator.bb[n].BoardY;
                    komaCalulator.bb[n] = komaB.GetComponent<BridgeButtonManager>();
                    komaCalulator.BlueX[n] = komaCalulator.bb[n].BoardX;
                    komaCalulator.BlueY[n] = komaCalulator.bb[n].BoardY;
           
                }
                GameObject koma = turnManager.RedBridges.transform.GetChild(N).gameObject;
                komaCalulator.bbb[N] = koma.GetComponent<BridgeButtonManager>();
                koma.transform.position = turnManager.MoveBridge(resultKomaIndex.Y, resultKomaIndex.X,N);
                komaCalulator.RedX[N] = resultKomaIndex.X;
                komaCalulator.RedY[N] = resultKomaIndex.Y;
                komaCalulator.bbb[N].BoardX = resultKomaIndex.X;
                komaCalulator.bbb[N].BoardY = resultKomaIndex.Y;
            }


        }
        else
        {
            turnManager.BuildAndDestroyBridge(resultKomaIndex.Y, resultKomaIndex.X, N);
            for (int n = 0; n < turnManager.RedBridges.transform.childCount - 1; n++)
            {
                GameObject komaR = turnManager.RedBridges.transform.GetChild(n).gameObject;
                GameObject komaB = turnManager.BlueBridges.transform.GetChild(n).gameObject;
                komaCalulator.bb[n] = komaR.GetComponent<BridgeButtonManager>();
                komaCalulator.RedX[n] = komaCalulator.bb[n].BoardX;
                komaCalulator.RedY[n] = komaCalulator.bb[n].BoardY;
                komaCalulator.bb[n] = komaB.GetComponent<BridgeButtonManager>();
                komaCalulator.BlueX[n] = komaCalulator.bb[n].BoardX;
                komaCalulator.BlueY[n] = komaCalulator.bb[n].BoardY;

            }
        }
    
    }
    public int GetAllCanMoveSearch(int[,] ban, int depth, int alpha, int beta, int N, bool First)
    {
        
        if (depth == 0) return EvaluateBuildStates(ban);
        var MaxScore = 0;
        komaCalulator.AIBlueTurn = !komaCalulator.AIBlueTurn;
        var canMovekoma = komaCalulator.BanCanMove(ban, N, komaCalulator.AIBlueTurn);                                                                         //  var canMovekoma = komaCalulator.BanCanBuild(ban,  N);
        foreach (var putkomaStateIndex in canMovekoma)
        {
            int A = 0;
            var Ban = ban;
            if (putkomaStateIndex.Build == true)
            {
                
                Ban = komaCalulator.Build(putkomaStateIndex.X, putkomaStateIndex.Y, ban, komaCalulator.AIBlueTurn );
               
            }
            else
            {
                Ban = komaCalulator.Move(putkomaStateIndex.X, putkomaStateIndex.Y, ban, N, komaCalulator.AIBlueTurn );
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
            ban = Ban;
            var childMovekoma = komaCalulator.BanCanMove(ban, N +1, komaCalulator.AIBlueTurn);
            foreach (var child in childMovekoma)
            {
               
                
                int B = 0;
                if (child.Build == true)
                {
                    Ban = komaCalulator.Build(child.X, child.Y, ban, komaCalulator.AIBlueTurn);
                   
                }
                else
                {
                    Ban = komaCalulator.Move(child.X, child.Y, ban, N +1, komaCalulator.AIBlueTurn);
                    if (komaCalulator.AIBlueTurn)
                    {
                        komaCalulator.BlueX[N + 1] = child.X;
                        komaCalulator.BlueY[N + 1] = child.Y;
                    }
                    else
                    {
                        komaCalulator.RedX[N + 1] = child.X;
                        komaCalulator.RedY[N + 1] = child.Y;
                    }
                }
                ban = Ban;
                if (N +1 != turnManager.RedBridges.transform.childCount -1)
                {

                    var ChildKoma1 = komaCalulator.BanCanMove(ban, N +2 , komaCalulator.AIBlueTurn);
                    foreach (var childKoma1 in ChildKoma1)
                    {
                        Debug.Log(childKoma1.X);
                        Debug.Log(childKoma1.Y);
                        int C = 0;
                        if (childKoma1.Build == true)
                        {
                            Ban = komaCalulator.Build(childKoma1.X, childKoma1.Y, ban, komaCalulator.AIBlueTurn);

                        }
                        else
                        {
                            Ban = komaCalulator.Move(childKoma1.X, childKoma1.Y, ban, N +2, komaCalulator.AIBlueTurn);
                            if (komaCalulator.AIBlueTurn)
                            {

                                komaCalulator.BlueX[N +2] = childKoma1.X;
                                komaCalulator.BlueY[N +2] = childKoma1.Y;
                            }
                            else
                            {
                                komaCalulator.RedX[N +2] = childKoma1.X;
                                komaCalulator.RedY[N+2] = childKoma1.Y;
                            }
                        }
                        ban = Ban;
                        if (N + 2 != turnManager.RedBridges.transform.childCount - 1)
                        {
                          
                            var ChildKoma3 = komaCalulator.BanCanMove(ban, N + 3, komaCalulator.AIBlueTurn);
                            foreach (var childKoma3 in ChildKoma3)
                            {
                                int D = 0;
                                if (childKoma3.Build == true)
                                {
                                    Ban = komaCalulator.Build(childKoma3.X, childKoma3.Y, ban, komaCalulator.AIBlueTurn);
                                }
                                else
                                {
                                    Ban = komaCalulator.Move(childKoma3.X, childKoma3.Y, ban, N +3, komaCalulator.AIBlueTurn);
                                    if (komaCalulator.AIBlueTurn)
                                    {
                                        komaCalulator.BlueX[N + 3] = childKoma3.X;
                                        komaCalulator.BlueY[N + 3] = childKoma3.Y;
                                    }
                                    else
                                    {
                                        komaCalulator.RedX[N + 3] = childKoma3.X;
                                        komaCalulator.RedY[N + 3] = childKoma3.Y;
                                    }
                                }
                                ban = Ban;
                                if (N + 3 != turnManager.RedBridges.transform.childCount - 1)
                                {
                                    var ChildKoma4 = komaCalulator.BanCanMove(ban, N + 4, komaCalulator.AIBlueTurn);
                                    foreach (var childKoma4 in ChildKoma4)
                                    {
                                        int E = 0;
                                        if (childKoma4.Build == true)
                                        {
                                            Ban = komaCalulator.Build(childKoma4.X, childKoma4.Y, ban, komaCalulator.AIBlueTurn);
                                        }
                                        else
                                        {
                                            Ban = komaCalulator.Move(childKoma4.X, childKoma4.Y, ban, N +4, komaCalulator.AIBlueTurn);
                                            if (komaCalulator.AIBlueTurn)
                                            {
                                                komaCalulator.BlueX[N + 4] = childKoma4.X;
                                                komaCalulator.BlueY[N + 4] = childKoma4.Y;
                                            }
                                            else
                                            {
                                                komaCalulator.RedX[N + 4] = childKoma4.X;
                                                komaCalulator.RedY[N + 4] = childKoma4.Y;
                                            }
                                        }
                                        ban = Ban;
                                        if (N + 4 != turnManager.RedBridges.transform.childCount - 1)
                                        {
                                            var ChildKoma5 = komaCalulator.BanCanMove(ban, N + 5, komaCalulator.AIBlueTurn);
                                            foreach (var childKoma5 in ChildKoma5)
                                            {
                                                int F = 0;
                                                if (childKoma5.Build == true)
                                                {
                                                    Ban = komaCalulator.Build(childKoma5.X, childKoma5.Y, ban, komaCalulator.AIBlueTurn);
                                                }
                                                else
                                                {
                                                    Ban = komaCalulator.Move(childKoma5.X, childKoma5.Y, ban, N +5, komaCalulator.AIBlueTurn);
                                                    if (komaCalulator.AIBlueTurn)
                                                    {
                                                        komaCalulator.BlueX[N + 5] = childKoma5.X;
                                                        komaCalulator.BlueY[N + 5] = childKoma5.Y;
                                                    }
                                                    else
                                                    {
                                                        komaCalulator.RedX[N + 5] = childKoma5.X;
                                                        komaCalulator.RedY[N + 5] = childKoma5.Y;
                                                    }
                                                }
                                                ban = Ban;
                                                var score = GetAllCanMoveSearch(Ban, depth - 1, alpha, beta, 0, true);

                                                if (score > beta)
                                                {
                                                    MaxScore = score;
                                                }
                                                if (childKoma5.Build)
                                                {
                                                    Ban[childKoma5.X, childKoma5.Y] = F;
                                                }
                                            }
                                        }

                                        else
                                        {
                                            var score = GetAllCanMoveSearch(Ban, depth - 1, alpha, beta, 0, true);
                                            if (score > beta)
                                            {
                                                MaxScore = beta;
                                            }

                                        }
                                        if (childKoma4.Build)
                                        {
                                            Ban[childKoma4.X, childKoma4.Y] = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    var score = GetAllCanMoveSearch(Ban, depth - 1, alpha, beta, 0, true);
                                    if (score > beta)
                                    {
                                        MaxScore = score;
                                    }

                                }
                                if (childKoma3.Build)
                                {
                                    Ban[childKoma3.X, childKoma3.Y] = 0;
                                }
                            }
                            
                        }
                        else
                        {
                            var score = GetAllCanMoveSearch(Ban, depth - 1, alpha, beta, 0, true);
                            if (childKoma1.Build == true)
                            {
                                F = true;
                                Ban[childKoma1.X, childKoma1.Y] = 0;
                            }
                            if (score > beta && F == true) return score;

                            if (score > beta)
                            {
                                beta = score;
                                score = MaxScore;
                            }
                            alpha = Mathf.Max(alpha, score);
                            MaxScore = Mathf.Max(MaxScore, score);
                           
                        }
                       
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
                   
                    }
                  F = true;
                if (child.Build == true)
                {
                    Ban[child.X, child.Y] = B;
                }
               
            }
            Ban[putkomaStateIndex.X, putkomaStateIndex.Y] = A;
        }
        return MaxScore;
    }
      
}


