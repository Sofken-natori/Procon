using ServerConnector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Area : MonoBehaviour
{
    #region デバック用壁出現ツール
    [Header("青城壁")]public bool BlueWall = false;
    [Header("赤城壁")]public bool RedWall = false;
    [Header("池")]public bool pond = false;
    [Header("城")]public bool castle = false;
    [Header("デバックモード")]public bool DebugMode = false;
    #endregion

    #region 囲い判定用
    [Header("囲い判定用変数")]
    public bool BlueAreaLeak = false;
    public bool RedAreaLeak = false;
    #endregion


    #region 変更できる変数
    [Header("赤陣地マーカー"), SerializeField]GameObject RedAreaMarker;
    [Header("青陣地マーカー"), SerializeField]GameObject BlueAreaMarker;
    [Header("赤城壁マーカー"), SerializeField]GameObject RedWallMarker;
    [Header("青城壁マーカー"), SerializeField]GameObject BlueWallMarker;
    [Header("城マーカー"), SerializeField]GameObject CastleMarker;
    [Header("池マーカー"), SerializeField]GameObject PondMarker;
    [Header("青いコマの置き場所"), SerializeField]public GameObject BlueBridges;
    [Header("赤いコマの置き場所"), SerializeField]public GameObject RedBridges;
    [Header("上壁判定"),SerializeField]Area FWC;
    [Header("下壁判定"),SerializeField]Area BWC;
    [Header("左壁判定"),SerializeField]Area LWC;
    [Header("右壁判定"),SerializeField]Area RWC;
    [Header("このスクリプト止める"),SerializeField]bool Scapegoat = false;
    #endregion

    #region 変更できない変数
    [HideInInspector]public bool BlueArea = false;
    [HideInInspector]public bool RedArea = false;
    [HideInInspector]public bool Bridge = false;
    [HideInInspector]public TurnManager TM;

    
    int i;
    int ThisPosX;
    int ThisPosY;
    (int x, int y) PointPos;
    #endregion


    

    // Start is called before the first frame update
    void Awake()
    {
      
        if (!Scapegoat){
        //初期化
        BlueAreaMarker.SetActive(false);
        RedAreaMarker.SetActive(false);
        BlueWallMarker.SetActive(false);
        RedWallMarker.SetActive(false);
        CastleMarker.SetActive(false);
        PondMarker.SetActive(false);

        
        //周囲のAreaを取得
        ThisPosX = int.Parse(this.gameObject.name);
        ThisPosY = int.Parse(this.transform.parent.gameObject.name);

        TM = transform.parent.parent.GetComponent<TurnManager>();

        if(ThisPosY > 0)
        {
            FWC = TM.transform.GetChild(ThisPosY - 1).GetChild(ThisPosX).GetComponent<Area>();
        }

        if(ThisPosY < TM.BoardYMax - 1)
        {
            BWC = TM.transform.GetChild(ThisPosY + 1).GetChild(ThisPosX).GetComponent<Area>();
        }
        
        if(ThisPosX > 0)
        {
            LWC = TM.transform.GetChild(ThisPosY).GetChild(ThisPosX - 1).GetComponent<Area>();
        }
        
        if(ThisPosX < TM.BoardXMax - 1)
        {
            RWC = TM.transform.GetChild(ThisPosY).GetChild(ThisPosX + 1).GetComponent<Area>();
        }

        }
    }
   
    void Update()
    {
        if(!Scapegoat)
        {
            #region 城壁反映
            if (!BlueWall && !RedWall)
            {
                if(BlueArea)
                {
                    BlueAreaMarker.SetActive(true);
                }
                else if(RedArea)
                {
                    RedAreaMarker.SetActive(true);
                }

                else
                {
                    BlueAreaMarker.SetActive(false);
                    RedAreaMarker.SetActive(false);
                }
            }
            
            if(BlueWall)
            {
                BlueWallMarker.SetActive(true);
            }

            else
            {
                BlueWallMarker.SetActive(false);
            }

            if(RedWall)
            {
                RedWallMarker.SetActive(true);
            }

            else
            {
                RedWallMarker.SetActive(false);
            }

            if(castle)
            {
                CastleMarker.SetActive(true);
            }

            else
            {
                CastleMarker.SetActive(false);
            }

            if(pond)
            {
                PondMarker.SetActive(true);
            }

            else
            {
                PondMarker.SetActive(false);
            }
                #endregion
        }
    }

    public void AreaDeployer(string AreaType)
    {
        switch(AreaType)
        {
            case "0":
                pond = false;
                castle = false;
                return;

            case "1":
                pond = true;
                return;

            case "2":
                castle = true;
                return;

            case "a":
                TM.BridgeDeployer(true,ThisPosY,ThisPosX);
                Debug.Log("BridgeDeployed");
                return;
                
            case "b":
                TM.BridgeDeployer(false,ThisPosY,ThisPosX);
                Debug.Log("BridgeDeployed");
                return;
            
            default:
                return;
        }
    }

    public void AreaLeak(bool isBlue)
    {
        if(isBlue)
        {
            BlueAreaLeak = true;
            if(!BlueWall)
            {
                if(!FWC.BlueAreaLeak)
                {
                    FWC.AreaLeak(true);
                }

                if(!BWC.BlueAreaLeak)
                {
                    BWC.AreaLeak(true);
                }

                if(!LWC.BlueAreaLeak)
                {
                    LWC.AreaLeak(true);
                }

                if(!RWC.BlueAreaLeak)
                {
                    RWC.AreaLeak(true);
                }
            }
        }
        
        else
        {
            
            RedAreaLeak = true;
            if(!RedWall)
            {
                if(!FWC.RedAreaLeak)
                {
                    FWC.AreaLeak(false);
                }

                if(!BWC.RedAreaLeak)
                {
                    BWC.AreaLeak(false);
                }

                if(!LWC.RedAreaLeak)
                {
                    LWC.AreaLeak(false);
                }

                if(!RWC.RedAreaLeak)
                {
                    RWC.AreaLeak(false);
                }
            }
        }
    }

    public void SiegeAreaChecker(bool isBlue)
    {
        if(isBlue)
        {
            if(!BlueAreaLeak)
            {
                BlueArea = true;

                if(RedAreaLeak)
                {
                    RedArea = false;
                }
            }
        }

        else
        {
            if(!RedAreaLeak)
            {
                RedArea = true;

                if(BlueAreaLeak)
                {
                    BlueArea = false;
                }
            }
        }
    }

    public void AreaLeakReseter(bool isBlue)
    {
        if(isBlue)
        {
            BlueAreaLeak = false;
        }

        else
        {
            RedAreaLeak = false;
        }
    }

    public (int,int) AddScore(int AreaScore,int WallScore,int CastleScore)
    {
        (int Blue,int Red) ScoreIndex = (0,0);

        if(BlueArea)
        {
            ScoreIndex.Blue += AreaScore;

            if(castle)
            {
                ScoreIndex.Blue += CastleScore;
            }
        }

        if(RedArea)
        {
            ScoreIndex.Red += AreaScore;

            if(castle)
            {
                ScoreIndex.Red += CastleScore;
            }
        }

        if(BlueWall)
        {
            ScoreIndex.Blue += WallScore;
        }

        if(RedWall)
        {
            ScoreIndex.Red += WallScore;
        }

        return ScoreIndex;
    }

    public void AreaApply(NowBoard board)
    {
        switch (board.structures[ThisPosX,ThisPosY])
        {
            case 0:
                pond = false;
                castle = false;
                break;
            case 1:
                pond = true;
                castle = false;
                break;
            case 2:
                pond = false;
                castle = true;
                break;
            default:
                Debug.LogWarning("予期しないもの拾った 拾ったやつ:" + board.walls[ThisPosY, ThisPosX]);
                break;
        }

        switch(board.masons[ThisPosX,ThisPosY])
        {
            case 0:
                break;
            default:
                if (board.masons[ThisPosX,ThisPosY] > 0)
                {
                    BridgeButtonManager bbm = BlueBridges.transform.GetChild(board.masons[ThisPosX, ThisPosY] - 1).GetComponent<BridgeButtonManager>();
                    bbm.BridgeApplyer(ThisPosX,ThisPosY);
                    bbm.BridgeID = board.masons[ThisPosX, ThisPosY];
                    break;
                }

                else
                {
                    BridgeButtonManager bbm = RedBridges.transform.GetChild((board.masons[ThisPosX,ThisPosY] * -1) - 1).GetComponent<BridgeButtonManager>();
                    bbm.BridgeApplyer(ThisPosX, ThisPosY);
                    bbm.BridgeID = board.masons[ThisPosX, ThisPosY];
                    break;
                }
        }       

        switch(board.walls[ThisPosY, ThisPosX])
        {
            case 0:
                BlueWall = false;
                RedWall = false;
                break;
            case 1:
                BlueWall = true;
                RedWall = false;
                break;
            case 2:
                BlueWall = false;
                RedWall = true;
                break;
            default:
                Debug.LogWarning("予期しないもの拾った 拾ったやつ:" + board.walls[ThisPosY, ThisPosX]);
                break;

        }

        switch(board.territories[ThisPosY, ThisPosX])
        {
            case 0:
                BlueArea = false;
                RedArea = false;
                break;
            case 1:
                BlueArea = true;
                RedArea = false;
                break;
            case 2:
                BlueArea = false;
                RedArea = true;
                break;
            case 3:
                BlueArea = true;
                RedArea = true;
                break;
            default:
                Debug.LogWarning("予期しないもの拾った 拾ったやつ:" + board.territories[ThisPosY, ThisPosX]);
                break;
        }
        return;
    }
}