using Koma;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Monte : MonoBehaviour
{
    KomaCalulator kc;
    //  Scene scene;
  
    Area area;
    public static Scene instance;
   
    public BridgeButtonManager bg;
    public BridgeButtonManager bg_Red;
    TurnManager tm;
  
    public int NowTurn = 0;
    public GameObject koma;
    public GameObject koma_Bridge;
    public GameObject koma_Red;
    //  public KomaCalulator kc
    GameObject Board;
    //GameObject gameobject;
    // public static Monte instance;
    private void Start()
    {

        Board = GameObject.Find("UserInterface/Board");
        koma_Bridge = GameObject.Find("BoardBridge");
        kc = Board.GetComponent<KomaCalulator>();
        tm = Board.GetComponent<TurnManager>();
        area = Board.GetComponent<Area>();
        
        
    }
     void Update()
    {
        NowTurn = tm.NowTurn;
    }
    public void MathAreaChengh()
    {

    }
    
    public int MonteCarlo(int X)
    {
       
     
        int BlueWinCount = 0;       
       for (int m = 0; m < X; m++)
        {
            for (int n = 0; n < tm.PieceNumber * 2; n++)
            {
                koma = koma_Bridge.transform.GetChild(n).gameObject;
                kc.bb[n] = koma.GetComponent<BridgeButtonManager>();
                      kc.X[n] = kc.bb[n].BoardX;
                      kc.Y[n] = kc.bb[n].BoardY;
            }
            int[,] Ban = kc.AIBanState();
            for (NowTurn = 0; NowTurn <tm.MaxTurnNumber;)
            {
                int N = 0;
                kc.PlayerCount = 0;
                for (; kc.PlayerCount < tm.PieceNumber; N++)// 
                {
                    Ban = kc.Randam(N);
                }
                NowTurn++;
                kc.AIBlueTurn = !kc.AIBlueTurn;
            }
            var win = kc.AIScoreCheck(Ban);
            if (win)
            {
                BlueWinCount++;
            }
         

        }

        return BlueWinCount;
    }
    public KomaIndex MonteCarloSearch(int N)
    {
        KomaIndex koma = null;
        int Max = -1;
        int BuildMax = -1;
        var  CanMove = kc.GetCanMoveIndex(N);
        var CanBuild = kc.GetCanBuild(N);
        foreach (var Can in CanMove )
        {
         
            var winCount = MonteCarlo(10);
            if (winCount > Max)
            {
                Max = winCount;
                 koma = Can;
            }
        }
        foreach(var Can in CanBuild)
        {

            var winCount = MonteCarlo(10);
            if (winCount > Max)
            {
                BuildMax = winCount;
                koma = Can;
               
            }
        }
       
        if(BuildMax > Max)
        {
            tm.BuildAndDestroyBridge(koma.Y, koma.X);
        }
        else
        {
            KomaIndex Index;
            Index = koma;
            GameObject komas = koma_Bridge.transform.GetChild(N).gameObject;
            kc.bb[N] = komas.GetComponent<BridgeButtonManager>();
            komas.GetComponent<BridgeButtonManager>();//  7        9
            komas.transform.position = tm.MoveBridge(Index.Y, Index.X);
            for(int n = 0;  n < tm.PieceNumber * 2; n++)
            {
                GameObject komar = koma_Bridge.transform.GetChild(n).gameObject;
                BridgeButtonManager[] bbb = new BridgeButtonManager[12];
                bbb[n] = komar.GetComponent<BridgeButtonManager>();
                kc.X[n] = bbb[n].BoardX;
                kc.Y[n] = bbb[n].BoardY;
            }
            kc.X[N] = Index.X;
            kc.Y[N] = Index.Y;
            kc.bb[N].BoardX = Index.X;
            kc.bb[N].BoardY = Index.Y;
        }
        return koma;
        
     
    }
  
  
    
  
}
