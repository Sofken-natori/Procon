using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Koma;
//using Koma.state;
using koma.hantei;

namespace KomaKeisan
{
    public class komaK : MonoBehaviour
    {
        // Start is called before the first frame update


        // Update is called once per frame



        /*  public static State GetKomaState(State komaState)
          {
              return komaState == State.Red ? State.Blue : State.Red;

          }
          public static State[,] GetPutkomaState(State[,] komaStates, State putState, int putX, int putY, List<KomaIndex> turnKomaIndex = null)
          {
              if (komaStates == null || komaStates[putX, putY] != State.Empty) return komaStates;
              turnKomaIndex ??= GetTurnKomaIndex(komaStates, putState, putX, putY);
              if (turnKomaIndex.Count == 0) return komaStates;
              var putkomaStates = komaStates.Clone() as State[,];
              if (putkomaStates != null)
              {
                  putkomaStates[putX, putY] = putState;

                  //Debug.Log("a");


              }
              return putkomaStates;



          }
          public static List<KomaIndex> GetAllCanPutKomaIndex(State[,] komaStates, State putState)
          {
              var canPutKoma = new List<KomaIndex>();
              for (var x = 0; x < komaStates.GetLength(0); x++)
              {
                  for (var y = 0; y < komaStates.GetLength(1); y++)
                  {
                      // 置けるストーンなら追加
                      if (GetTurnKomaIndex(komaStates, putState, x, y).Count > 0)
                      {
                          canPutKoma.Add(new KomaIndex(x, y));
                      }
                  }
              }
              return canPutKoma;
          }

          public static List<KomaIndex> GetTurnKomaIndex(State[,] komaStates, State putState, int putX, int putZ)
          {
              // 既にストーンが置かれていたら空で返却
              var turnKomaIndex = new List<KomaIndex>();
              if (komaStates == null || komaStates[putX, putZ] != State.Empty) return turnKomaIndex;
              public static List<StoneIndex> GetTurnStonesIndex(StoneState[,] stoneStates, StoneState putState, int putX, int putZ)
              {
                  // 既にストーンが置かれていたら空で返却
                  var turnStonesIndex = new List<StoneIndex>();
                  if (stoneStates == null || stoneStates[putX, putZ] != StoneState.Empty) return turnStonesIndex;

                  // 8方向分のストーンを調べて返却する

                  return turnKomaIndex;
          }

        */

      /*  public static State[,] GetPutkomaState(State[,] komaStates, State putState, int putX, int putY, List<KomaIndex> turnKomaIndex = null)
        {
            if (komaStates == null || komaStates[putX, putY] != State.Empty) return komaStates;
            turnKomaIndex ??= GetTurnKomaIndex(komaStates, putState, putX, putY);
            if (turnKomaIndex.Count == 0) return komaStates;
            var putkomaStates = komaStates.Clone() as State[,];
            if (putkomaStates != null)
            {
                putkomaStates[putX, putY] = putState;

                //Debug.Log("a");


            }
            return putkomaStates;



        }
       
        */
    }
}
    