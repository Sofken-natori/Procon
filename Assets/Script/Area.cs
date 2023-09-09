using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Area : MonoBehaviour
{
    #region デバック用壁出現ツール
    [Header("青城壁")]public bool BlueWall = false;
    [Header("赤城壁")]public bool RedWall = false;
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
    [Header("上壁判定"),SerializeField]Area FWC;
    [Header("下壁判定"),SerializeField]Area BWC;
    [Header("左壁判定"),SerializeField]Area LWC;
    [Header("右壁判定"),SerializeField]Area RWC;
    [Header("このスクリプト止める"),SerializeField]bool Scapegoat = false;
    #endregion

    #region 変更できない変数
    [HideInInspector]public bool BlueArea = false;
    [HideInInspector]public bool RedArea = false;
    [HideInInspector]public bool castle = false;
    [HideInInspector]public bool pond = false;
    [HideInInspector]public bool Bridge = false;

    TurnManager TM;
    int i;
    int ThisPosX;
    int ThisPosY;
    (int x, int y) PointPos;
    #endregion


    

    // Start is called before the first frame update
    void Start()
    {
        if(!Scapegoat){
        //初期化
        TM = transform.parent.parent.GetComponent<TurnManager>();
        BlueAreaMarker.SetActive(false);
        RedAreaMarker.SetActive(false);
        BlueWallMarker.SetActive(false);
        RedWallMarker.SetActive(false);
        CastleMarker.SetActive(false);
        PondMarker.SetActive(false);

        
        //周囲のAreaを取得
        ThisPosX = int.Parse(this.gameObject.name);
        ThisPosY = int.Parse(this.transform.parent.gameObject.name);

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
    
}