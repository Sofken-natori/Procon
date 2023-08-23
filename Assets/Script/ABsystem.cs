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
        //�z��ŕ]���֐����쐬
          private static readonly int[,] EvaluatekomaStateScore = new[,] {
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
        
          //�T���p�[�g
          public static KomaIndex SearchAlphaBeta(State[,] komaState, State putkomaState, int depth, bool isPrevPassed = false)
          {
              //   var random = new Random();
              //�T��������
              KomaIndex resultKomaIndex = null;

              // �u�����Ƃ��\�ȃX�g�[����S�Ē��ׂ�
              var alpha = int.MinValue + 1;
              var beta = int.MaxValue;

              var canPutkomaIndex = komaK.GetAllCanPutKomaIndex(komaState, putkomaState);
              // �u�����Ƃ��\�ȋ�𒲂ׂ�
              //foreach�ł��ׂĂ̑w��T��
              foreach (var putkomaStateIndex in canPutkomaIndex)
              {
                  // ���̊K�w�̏�Ԃ𒲂ׂ�
                  var putkomaStates = komaK.GetPutkomaState(komaState, putkomaState, putkomaStateIndex.X, putkomaStateIndex.Y);
                  var score = -1 * AlphaBetaScore(putkomaStates, komaK.GetKomaState(putkomaState), depth - 1, -beta, -alpha);

                  // �ő�X�R�A�̏ꍇ�A�X�R�A�ƊY���C���f�b�N�X��ێ�
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

              // �t�m�[�h�ŕ]���֐�����
              if (depth == 0) return EvaluateStoneStates(komaStates, putkomaState);

              // �u�����Ƃ��\�ȃX�g�[����S�Ē��ׂ�
              var maxScore = int.MinValue;
              var canPutkomaIndex = komaK.GetAllCanPutKomaIndex(komaStates, putkomaState);
              foreach (var putKomaIndex in canPutkomaIndex)
              {

                  // ���̊K�w�̏�Ԃ𒲂ׂ�
                  var putKomaStates = komaK.GetPutkomaState(komaStates, putkomaState, putKomaIndex.X, putKomaIndex.Y);
                  var score = -1 * AlphaBetaScore(putKomaStates, komaK.GetKomaState(putkomaState), depth - 1, -beta, -alpha);

                  // NegaMax�l���T���͈͂̏������̏ꍇ�͎}���
                  if (score >= beta) return score;

                  // alpha�l�AmaxScore���X�V
                  alpha = Mathf.Max(alpha, score);
                  maxScore = Mathf.Max(maxScore, score);

              }
              if (maxScore == int.MinValue)
              {
                  //   if (isPrevPassed) return EvaluateStoneStates(komaStates, putkomaState);
                  // �X�g�[����Ԃ͂��̂܂܂ŁA���̊K�w�̏�Ԃ𒲂ׂ�
                  return -1 * AlphaBetaScore(komaStates, komaK.GetKomaState(putkomaState), depth - 1, -beta, -alpha, true);

              }
              return maxScore;

          }
          

    }
}