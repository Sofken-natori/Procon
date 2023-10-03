using Koma;
using state;
using System.Reflection;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Alpha : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    GameObject Board;
    GameObject TM;
    KomaCalulator komaCalulator;
    public TurnManager turnManager;
    
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
            { 1 ,  0  , 0  , 0 ,  0 ,  0 ,  0 ,  10 ,  30 ,  20 ,  30 },
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
           {   0 ,   0 , 0 , 0 , 0 , 0 , 0 , 0 , 2 , -1,  -1 },
           {   0 ,   0 , 0 , 0 , 0 , 0 , 0 , 0 , -1 , 2,  0},
           {   0 ,   0 , 0 , 0 , -1 , -1 , -1 , 2 , -1 , 2,  0},
           {   0 ,   0 , 0 , -1 , -1 , 2 , -1 , 0 , -1 , 0,  0},
           {   0 ,   0 , -1 , -1 , 2 , 2 , 2 , -1 , -1 , 0,  0},
           {   0 ,   0 , -1 , 0 , -1 , 2 , -1 , -1 , 0 , 0,  0},
           {   0 ,   2 , -1 , 2 , -1 , -1 , -1 , 0 , 0 , 0,  0},
           {   0 ,   2 , -1 , 0 , 0 , 0 , 0 , 0 , 0 , 0,  0},
           {   -1 ,   -1 , 2 , 0 , 0 , 0 , 0 , 0 , 0 , 0,  0},
            {  -1 ,   0 , 0 , 0 , 0 , 0 , 0 , 0 , 0 , 0,  0}
      };


    public int EvaluateBuildStates(int[,] ban)
    {
      // ban = komaCalulator.AIScoreCheck(ban);
    
        var myscore = 0;
        var enemyscore = 0;
     //ban = komaCalulator.AIBanState();
        komaCalulator.AIAreaCheckBlue(ban);
        for (int y = 0; y < turnManager.BoardYMax; y++)
        {
            for (int x = 0; x < turnManager.BoardXMax; x++)
            {
                var Banscore = EvaluateBuildStateScore[x, y];
                if (ban[x, y] == 10)
                {
                    myscore += bluekoma_point;
                }
                else if (ban[x, y] == 30)
                {
                    myscore += jinti_point;
                }
                if (ban[x, y] == -10)
                {
                    enemyscore = bluekoma_point;
                }
                if (ban[x, y] == -30)
                {
                    enemyscore = jinti_point;
                }
                if (ban[x, y] == 130)
                {
                    myscore += castle;
                }
                if (ban[x, y] == -130)
                {
                    enemyscore += castle;
                }
            }
          //  Debug.Log(myscore);
        }
        return myscore ;
    }
    public int EvaluateMoveStates(  int[,] ban , int N)
    {
            var Banscore = EvaluateMoveStateScore[komaCalulator.X[N], komaCalulator.Y[N]];
            var myscore = 0;      
                myscore += Banscore;
                

        return myscore;
    }
 
    public void AlphaBeta(int depth, int[,] ban,  int N)
    {
        KomaIndex resultKomaIndex = null;
        var alpha = 1; 
        var beta = 0; 
        var score = 0;
        var Buildscore = 0;
        var canPutKomaIndex = komaCalulator.GetCanBuild(N);
        var canMovekoma = komaCalulator.GetCanMoveIndex(N);
        foreach (var putkomaStateIndex in canPutKomaIndex)
        {
            var Ban = komaCalulator.Build(putkomaStateIndex.X , putkomaStateIndex.Y , ban ,true);
            Ban = ban;
            komaCalulator.X[N] = putkomaStateIndex.X;
            komaCalulator.Y[N] = putkomaStateIndex.Y;
            Buildscore = GetAllCanEnemySearch(Ban, depth - 1,  alpha, beta, N, false);
            //敵スコアと味方スコア
            if (alpha < Buildscore)
            {
                alpha = Buildscore;
                resultKomaIndex = putkomaStateIndex;
            }

        }
        foreach (var putkomaStateIndex in canMovekoma)
        {
            var Ban = komaCalulator.Move(putkomaStateIndex.X, putkomaStateIndex.Y, ban , N);
             score = GetAllCanEnemySearch(Ban, depth - 1, alpha, beta, N, false);
            if (alpha < score)
            {
                alpha = score;
                resultKomaIndex = putkomaStateIndex;
            }
             //  Debug.Log(score);
             //   Debug.Log(Buildscore);

        }
        if (score > Buildscore)
        {
            GameObject koma = Board.transform.GetChild(N).gameObject;
            koma.transform.position = turnManager.MoveBridge(resultKomaIndex.Y, resultKomaIndex.X);
            komaCalulator.X[N] = resultKomaIndex.X;
            komaCalulator.Y[N] = resultKomaIndex.Y;
        }
        if ( Buildscore > score )
        {
            
            turnManager.BuildAndDestroyBridge(resultKomaIndex.Y, resultKomaIndex.X);
        }

     //   return resultKomaIndex;
    }
    public int GetAllCanMoveSearch(int[,] ban, int depth, int alpha, int beta , int N , bool Build ,bool One)
    {
        if (depth == 0 && true) return EvaluateBuildStates( ban);
        if (depth == 0 && false) return EvaluateMoveStates(ban , N);
        var MaxScore = int.MinValue;
        komaCalulator.AIBlueTurn =! komaCalulator.AIBlueTurn;
        var canPutKomaIndex = komaCalulator.BanCanMove(ban , N);
        var canMovekoma = komaCalulator.BanCanBuild(ban,  N);
        if (komaCalulator.AIBlueTurn)
        {
            N = 0;
            foreach (var putkomaStateIndex in canPutKomaIndex)
            {
                var Ban = komaCalulator.Move(putkomaStateIndex.X, putkomaStateIndex.Y, ban, N);
                Ban = ban;
                komaCalulator.X[N] = putkomaStateIndex.X;
                komaCalulator.Y[N] = putkomaStateIndex.Y;
                if (N != turnManager.PieceNumber)
                {
                    var score = GetAllCanMoveSearch(Ban, depth, alpha, beta, N + 1, false, false); 
                }
                else
                {
                    var score = GetAllCanEnemySearch(Ban, depth - 1, alpha, beta, N, false);
                }
                /* if (score >= beta) return score;
                 alpha = Mathf.Max(alpha, score);
                 MaxScore = Mathf.Max(MaxScore, score);
                */
            }
         /*   foreach (var putkomaStateIndex in canMovekoma)
            {
                ban = komaCalulator.Build(putkomaStateIndex.Y, putkomaStateIndex.X, ban, false);
                var score = GetAllCanMoveSearch(ban, depth - 1, alpha, beta, N, true);
                if (score >= beta) return score;
                alpha = Mathf.Max(alpha, score);
                MaxScore = Mathf.Max(MaxScore, score);
            }
         */

        }
        else
        {
           
                N = 0;
                foreach (var putkomaStateIndex in canPutKomaIndex)
                {

                    var Ban = komaCalulator.Move(putkomaStateIndex.X, putkomaStateIndex.Y, ban, N);
                    Ban = ban;
                    komaCalulator.X[N] = putkomaStateIndex.X;
                    komaCalulator.Y[N] = putkomaStateIndex.Y;
                    if (N != turnManager.PieceNumber)
                    {
                        var score = GetAllCanEnemySearch(Ban, depth, alpha, beta, N + 1, false);
                    }
                    else
                    {
                        var score = GetAllCanMoveSearch(Ban, depth - 1, alpha, beta, N, false, true);
                    }
                    /* if (score >= beta) return score;
                     alpha = Mathf.Max(alpha, score);
                     MaxScore = Mathf.Max(MaxScore, score);
                    */
                }
                foreach (var putkomaStateIndex in canMovekoma)
                {
                    ban = komaCalulator.Build(putkomaStateIndex.Y, putkomaStateIndex.X, ban, false);
                    var score = GetAllCanMoveSearch(ban, depth - 1, alpha, beta, N, true, true);
                    if (score >= beta) return score;
                    alpha = Mathf.Max(alpha, score);
                    MaxScore = Mathf.Max(MaxScore, score);
                }

            
        }
       
        return MaxScore;
    }
    public int GetAllCanEnemySearch(int[,] ban, int depth, int alpha, int beta, int N , bool Build)
    {
        if (depth == 0 && true) return EvaluateBuildStates( ban);
        if (depth == 0 && false) return EvaluateMoveStates(ban ,N);
        var MaxScore = int.MinValue;
        komaCalulator.AIBlueTurn =! komaCalulator.AIBlueTurn;
        if (komaCalulator.AIBlueTurn)
        {
               N = 0;
             var canPutKomaIndex = komaCalulator.BanCanMove(ban, N);
                var canMovekoma = komaCalulator.BanCanBuild(ban, N);
                foreach (var putkomaStateIndex in canPutKomaIndex)
                {
                    
                    var Ban = komaCalulator.Move(putkomaStateIndex.X, putkomaStateIndex.Y, ban, N);
                    Ban = ban;
                    komaCalulator.X[N] = putkomaStateIndex.X;
                    komaCalulator.Y[N] = putkomaStateIndex.Y;
                if (N != turnManager.PieceNumber)
                {
                    var score = GetAllCanEnemySearch(Ban, depth , alpha, beta, N +1, false);
                }
                else
                {
                    var score = GetAllCanMoveSearch(Ban, depth - 1, alpha, beta, N, false, true);
                }
                   /* if (score >= beta) return score;
                    alpha = Mathf.Max(alpha, score);
                    MaxScore = Mathf.Max(MaxScore, score);
                   */
                }
                foreach (var putkomaStateIndex in canMovekoma)
                {
                    ban = komaCalulator.Build(putkomaStateIndex.Y, putkomaStateIndex.X, ban, false);
                    var score = GetAllCanMoveSearch(ban, depth - 1, alpha, beta, N, true, true);
                    if (score >= beta) return score;
                    alpha = Mathf.Max(alpha, score);
                    MaxScore = Mathf.Max(MaxScore, score);
                }
            
        }
        else
        {
            N = turnManager.PieceNumber;
            for (; N < turnManager.PieceNumber *2; N++)
            {
                var canPutKomaIndex = komaCalulator.BanCanMove( ban, N);
                var canMovekoma = komaCalulator.BanCanBuild(ban,  N);
                foreach (var putkomaStateIndex in canPutKomaIndex)
                { 
                    var   Ban =   komaCalulator.Move(putkomaStateIndex.X, putkomaStateIndex.Y, ban, N);
                    var score = GetAllCanMoveSearch(ban, depth - 1,  alpha, beta, N, false, true);
                    if (score >= beta) return score;
                    alpha = Mathf.Max(alpha, score);
                    MaxScore = Mathf.Max(MaxScore, score);
                }
                foreach (var putkomaStateIndex in canMovekoma)
                {
                    ban = komaCalulator.Build(putkomaStateIndex.Y, putkomaStateIndex.X, ban, false);
                    var score = GetAllCanMoveSearch(ban, depth - 1, alpha, beta, N, true, true);
                    if (score >= beta) return score;
                    alpha = Mathf.Max(alpha, score);
                    MaxScore = Mathf.Max(MaxScore, score);
                }
            }

        }
        return MaxScore;

    }
}
