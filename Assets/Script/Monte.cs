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
            for (int n = 0; n < tm.BlueBridges.transform.childCount; n++)
            {
                GameObject komaR = tm.RedBridges.transform.GetChild(n).gameObject;
                GameObject komaB = tm.BlueBridges.transform.GetChild(n).gameObject;
                kc.bb[n] = komaR.GetComponent<BridgeButtonManager>();
                kc.RedX[n] = kc.bb[n].BoardX;
                kc.RedY[n] = kc.bb[n].BoardY;
                kc.bb[n] = komaB.GetComponent<BridgeButtonManager>();
                kc.BlueX[n] = kc.bb[n].BoardX;
                kc.BlueY[n] = kc.bb[n].BoardY;
            }

            int[,] Ban = kc.AIBanState();
            
            for (NowTurn = 0; NowTurn <tm.MaxTurnNumber;)
            {
                int N = 0;
                kc.PlayerCount = 0;
                for (; kc.PlayerCount < tm.RedBridges.transform.childCount; N++)// 
                {
                    Ban = kc.Randam(N , kc.AIBlueTurn);
                }
                NowTurn++;
                kc.AIBlueTurn = !kc.AIBlueTurn;
            }

            var win = kc.AIScoreCheck(Ban);
            
            if (win)
            {
                BlueWinCount++;
            }
            
            for (int n = 0; n < tm.BlueBridges.transform.childCount; n++)
            {
                GameObject komaR = tm.RedBridges.transform.GetChild(n).gameObject;
                GameObject komaB = tm.BlueBridges.transform.GetChild(n).gameObject;
                kc.bb[n] = komaR.GetComponent<BridgeButtonManager>();
                kc.RedX[n] = kc.bb[n].BoardX;
                kc.RedY[n] = kc.bb[n].BoardY;
                kc.bb[n] = komaB.GetComponent<BridgeButtonManager>();
                kc.BlueX[n] = kc.bb[n].BoardX;
                kc.BlueY[n] = kc.bb[n].BoardY;
            }

       }

        return BlueWinCount;
    }
    public KomaIndex MonteCarloSearch(int N, bool Blue)
    {
        KomaIndex koma = null;
        KomaIndex komaB = null;
        int Max = -1;
        int BuildMax = -1;
        var  CanMove = kc.GetCanMoveIndex(N ,Blue);
        var CanBuild = kc.GetCanBuild(N, Blue);
       
        foreach (var Can in CanMove )
        {
            Debug.Log(Can.X);
            Debug.Log(Can.Y);
            var winCount = MonteCarlo(50);
            if (winCount > Max)
            {
                Max = winCount;
                 koma = Can;
            }
        }
        foreach(var Can in CanBuild)
        {
            var winCount = MonteCarlo(50);
            if (winCount > BuildMax)
            {
                BuildMax = winCount;
                komaB = Can;
               
            }
        }
       
        if(BuildMax > Max)
        {
            tm.BuildAndDestroyBridge(komaB.Y, komaB.X, N);
            for (int n = 0; n < tm.BlueBridges.transform.childCount; n++)
            {
                GameObject komar = tm.BlueBridges.transform.GetChild(n).gameObject;
                BridgeButtonManager[] bbb = new BridgeButtonManager[12];
                bbb[n] = komar.GetComponent<BridgeButtonManager>();
                kc.BlueX[n] = bbb[n].BoardX;
                kc.BlueY[n] = bbb[n].BoardY;
            }
            for (int n = 0; n < tm.RedBridges.transform.childCount; n++)
            {
                GameObject komar = tm.RedBridges.transform.GetChild(n).gameObject;
                BridgeButtonManager[] bbb = new BridgeButtonManager[12];
                bbb[n] = komar.GetComponent<BridgeButtonManager>();
                kc.RedX[n] = bbb[n].BoardX;
                kc.RedY[n] = bbb[n].BoardY;
            }
        }
        else
        {
            GameObject komas = tm.BlueBridges.transform.GetChild(N).gameObject;
            kc.bb[N] = komas.GetComponent<BridgeButtonManager>();
            komas.GetComponent<BridgeButtonManager>();//  ê‘î≈Ç‡çÏÇÈ
            komas.transform.position = tm.MoveBridge(koma.Y, koma.X, N);
            for(int n = 0;  n < tm.BlueBridges.transform.childCount; n++)
            {
                GameObject komar = tm.BlueBridges.transform.GetChild(n).gameObject;
                BridgeButtonManager[] bbb = new BridgeButtonManager[12];
                bbb[n] = komar.GetComponent<BridgeButtonManager>();
                kc.BlueX[n] = bbb[n].BoardX;
                kc.BlueY[n] = bbb[n].BoardY;
            }
            for(int n = 0; n < tm.RedBridges.transform.childCount; n++)
            {
                GameObject komar = tm.RedBridges.transform.GetChild(n).gameObject;
                BridgeButtonManager[] bbb = new BridgeButtonManager[12];
                bbb[n] = komar.GetComponent<BridgeButtonManager>();
                kc.RedX[n] = bbb[n].BoardX;
                kc.RedY[n] = bbb[n].BoardY;
            }
            if (Blue)
            {
                kc.BlueX[N] = koma.X;
                kc.BlueY[N] = koma.Y;
            }
            else
            {
                kc.RedX[N] = koma.X;
                kc.RedX[N] = koma.X;
            }
            kc.bb[N].BoardX = koma.X;
            kc.bb[N].BoardY = koma.Y;
        }
        return koma;
    }
}
