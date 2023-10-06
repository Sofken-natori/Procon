using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C13 : MonoBehaviour
{
    int[,] EvaluateMoveStateScore = new[,] {
    {0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  -1, -1},
{0, 0,  0,  -1, 2,  2,  0,  0,  0,  0,  0,  -1, 0},
{0, 0,  -1, 0,  0,  0,  0,  0,  0,  -1, -1, 0,  0},
{0, 2,  0,  0,  0,  0,  0,  0,  2,  -1, 2,  0,  0},
{0, 2,  0,  0,  0,  -1, -1, -1, 2,  -1, 2,  0,  0},
{0, 0,  0,  0,  -1, -1, 2,  -1, 0,  -1, 0,  0,  0},
{0, 0,  0,  -1, -1, 2,  2,  2,  -1, -1, 0,  0,  0},
{0, 0,  0,  -1, 0,  -1, 2,  -1, -1, 0,  0,  0,  0},
{0, 0,  2,  -1, 2,  -1, -1, -1, 0,  0,  0,  2,  0},
{0, 0,  2,  -1, 2,  0,  0,  0,  0,  0,  0,  2,  0},
{0, 0,  -1, -1, 0,  0,  0,  0,  0,  0,  -1, 0,  0},
{0, -1, 0,  0,  0,  0,  0,  2,  2,  -1, 0,  0,  0},
{-1,    -1, 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},

            };
    int[,] EvaluateBuildStateScore = new[,] {
          {0,   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  -1, -1},
{0, 0,  0,  -1, 2,  2,  0,  0,  0,  0,  0,  -1, 0},
{0, 0,  -1, 0,  0,  0,  0,  0,  0,  -1, -1, 0,  0},
{0, 2,  0,  0,  0,  0,  0,  0,  2,  -1, 2,  0,  0},
{0, 2,  0,  0,  0,  -1, -1, -1, 2,  -1, 2,  0,  0},
{0, 0,  0,  0,  -1, -1, 2,  -1, 0,  -1, 0,  0,  0},
{0, 0,  0,  -1, -1, 2,  2,  2,  -1, -1, 0,  0,  0},
{0, 0,  0,  -1, 0,  -1, 2,  -1, -1, 0,  0,  0,  0},
{0, 0,  2,  -1, 2,  -1, -1, -1, 0,  0,  0,  2,  0},
{0, 0,  2,  -1, 2,  0,  0,  0,  0,  0,  0,  2,  0},
{0, 0,  -1, -1, 0,  0,  0,  0,  0,  0,  -1, 0,  0},
{0, -1, 0,  0,  0,  0,  0,  2,  2,  -1, 0,  0,  0},
{ -1,    -1, 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0},


            };
  
}
