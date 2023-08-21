using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Koma;
using koma.hantei;
using KomaKeisan;
//using System.Threading.Tasks;
//using UniRx.Async;





namespace AI
{
   public class ABsystem : MonoBehaviour
    {
        //配列で評価関数を作成
       /*   private static readonly int[,] EvaluatekomaStateScore = new[,] {
              {  0, 1,   0,  1,  0,  1, 0,  1, 0, 1 , 0, 1, 0, 1, 0 },
              {  1, 0,   1,  0,  1,  0, 1,  0, 1, 0 , 1, 0, 1, 0, 1 },
              {  0, 1,   0,  1,  0,  1, 0,  1, 0, 1 , 0, 1, 0, 1, 0 },
              {  1, 0,   1,  0,  1,  0, 1,  0, 1, 0 , 1, 0, 1, 0, 1 },
              {  0, 1,   0,  1,  0,  1, 0,  1, 0, 1 , 0, 1, 0, 1, 0 },
              {  1, 0,   1,  0,  1,  0, 1,  0, 1, 0 , 1, 0, 1, 0, 1 },
              {  0, 1,   0,  1,  0,  1, 0,  1, 0, 1 , 0, 1, 0, 1, 0 },
              {  1, 0,   1,  0,  1,  0, 1,  0, 1, 0 , 1, 0, 1, 0, 1 },
              {  0, 1,   0,  1,  0,  1, 0,  1, 0, 1 , 0, 1, 0, 1, 0 }, 
              {  1, 0,   1,  0,  1,  0, 1,  0, 1, 0 , 1, 0, 1, 0, 1 },
              {  0, 1,   0,  1,  0,  1, 0,  1, 0, 1 , 0, 1, 0, 1, 0 },
              {  1, 0,   1,  0,  1,  0, 1,  0, 1, 0 , 1, 0, 1, 0, 1 },
              {  0, 1,   0,  1,  0,  1, 0,  1, 0, 1 , 0, 1, 0, 1, 0 },
              {  1, 0,   1,  0,  1,  0, 1,  0, 1, 0 , 1, 0, 1, 0, 1 },
              {  0, 1,   0,  1,  0,  1, 0,  1, 0, 1 , 0, 1, 0, 1, 0 },
      };

          public static int EvaluateStoneStates(State[,] komaStates, State putkomaState)
          {

              var myscore = 0;
              var enemyscore = 0;


              for (var x = 0; x < komaStates.GetLength(0); x++)
              {
                  for (var y = 0; y < komaStates.GetLength(0); y++)
                  {
                      var score = EvaluatekomaStateScore[x, y];
                      if (komaStates[x, y] == State.Blue)
                      {
                          myscore += score;
                      }
                      else if (komaStates[x, y] == State.Red)
                      {
                          enemyscore += score;
                      }
                  }
              }
              return myscore - enemyscore;
          }
        
          //探索パート
          public static KomaIndex SearchAlphaBeta(State[,] komaState, State putkomaState, int depth, bool isPrevPassed = false)
          {
              //   var random = new Random();
              //探索した駒
              KomaIndex resultKomaIndex = null;

              // 置くことが可能なストーンを全て調べる
              var alpha = int.MinValue + 1;
              var beta = int.MaxValue;

              var canPutkomaIndex = komaK.GetAllCanPutKomaIndex(komaState, putkomaState);
              // 置くことが可能な駒を調べる
              //foreachですべての層を探索
              foreach (var putkomaStateIndex in canPutkomaIndex)
              {
                  // 次の階層の状態を調べる
                  var putkomaStates = komaK.GetPutkomaState(komaState, putkomaState, putkomaStateIndex.X, putkomaStateIndex.Y);
                  var score = -1 * AlphaBetaScore(putkomaStates, komaK.GetKomaState(putkomaState), depth - 1, -beta, -alpha);

                  // 最大スコアの場合、スコアと該当インデックスを保持
                  if (alpha < score)
                  {
                      alpha = score;
                      resultKomaIndex = putkomaStateIndex;
                  }
                  return resultKomaIndex;
              }
              return resultKomaIndex;

          }


          public static int AlphaBetaScore(State[,] komaStates, State putkomaState, int depth, int alpha, int beta, bool isPrevPassed = false)
          {

              // 葉ノードで評価関数を実
              if (depth == 0) return EvaluateStoneStates(komaStates, putkomaState);

              // 置くことが可能なストーンを全て調べる
              var maxScore = int.MinValue;
              var canPutkomaIndex = komaK.GetAllCanPutKomaIndex(komaStates, putkomaState);
              foreach (var putKomaIndex in canPutkomaIndex)
              {

                  // 次の階層の状態を調べる
                  var putKomaStates = komaK.GetPutkomaState(komaStates, putkomaState, putKomaIndex.X, putKomaIndex.Y);
                  var score = -1 * AlphaBetaScore(putKomaStates, komaK.GetKomaState(putkomaState), depth - 1, -beta, -alpha);

                  // NegaMax値が探索範囲の上限より上の場合は枝狩り
                  if (score >= beta) return score;

                  // alpha値、maxScoreを更新
                  alpha = Mathf.Max(alpha, score);
                  maxScore = Mathf.Max(maxScore, score);

              }
              if (maxScore == int.MinValue)
              {
                  //   if (isPrevPassed) return EvaluateStoneStates(komaStates, putkomaState);
                  // ストーン状態はそのままで、次の階層の状態を調べる
                  return -1 * AlphaBetaScore(komaStates, komaK.GetKomaState(putkomaState), depth - 1, -beta, -alpha, true);

              }
              return maxScore;

          }
          */

    }
}