using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class Area : MonoBehaviour
{

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


    public bool BlueArea = false;
    public bool RedArea = false;
    public bool BlueWall = false;
    public bool RedWall = false;
    public bool castle = false;
    public bool pond = false;
    public bool Bridge = false;

    TurnManager TM;
    int BlueWalli;
    int BlueWallj;
    int RedWalli;
    int RedWallj;
    int RedMaxi;
    int RedMaxj;
    int BlueMaxi;
    int BlueMaxj;
    int RedMini;
    int RedMinj;
    int BlueMini;
    int BlueMinj;
    int ThisPosX;
    int ThisPosY;
    (int x, int y) PointPos;

    //囲い判定用
    public bool BlueWallisDown = false;
    public bool BlueWallisUp = false;
    public bool BlueWallisLeft = false;
    public bool BlueWallisRight = false;
    public bool BlueAreaVerticalSiege = false;
    public bool BlueAreaHorizontalSiege = false;
    public bool BlueAreaLeak = true;

    public bool RedWallisDown = false;
    public bool RedWallisUp = false;
    public bool RedWallisLeft = false;
    public bool RedWallisRight = false;
    public bool RedAreaVerticalSiege = false;
    public bool RedAreaHorizontalSiege = false;
    public bool RedAreaLeak = true;

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

    // Update is called once per frame
    void Update()
    {
        #region 城壁判定
        if(!Scapegoat){
        //包囲判定
        
        if(FWC.BlueWall || FWC.BlueWallisUp)
        {
            BlueWallisUp = true;
        }
        else
        {
            BlueWallisUp = false;
        }

        if(BWC.BlueWall || BWC.BlueWallisDown)
        {
            BlueWallisDown = true;
        }
        else
        {
            BlueWallisDown = false;
        }

        if(LWC.BlueWall || LWC.BlueWallisLeft)
        {
            BlueWallisLeft = true;
        }
        else
        {
            BlueWallisLeft = false;
        }

        if(RWC.BlueWall || RWC.BlueWallisRight)
        {
            BlueWallisRight = true;
        }
        else
        {
            BlueWallisRight = false;
        }

        if(BlueWallisDown && BlueWallisUp)
        {
            BlueAreaVerticalSiege = true;
        }
        else
        {
            BlueAreaVerticalSiege = false;
        }

        if(BlueWallisLeft && BlueWallisRight)
        {
            BlueAreaHorizontalSiege = true;
        }
        else
        {
            BlueAreaHorizontalSiege = false;
        }



        if(BlueAreaHorizontalSiege && BlueAreaVerticalSiege || BlueWall || RedWall)
        {
            BlueAreaLeak = false;

        }
        else
        {
            BlueAreaLeak = true;
        }

        if(!CheckAroundArea("blue") && !BlueWall && !RedWall)
        {
            BlueAreaLeak = true;
        }
        else
        {
            BlueAreaLeak = false;
        }





        if(FWC.RedWall || FWC.RedWallisUp)
        {
            RedWallisUp = true;
        }
        else
        {
            RedWallisUp = false;
        }

        if(BWC.RedWall || BWC.RedWallisDown)
        {
            RedWallisDown = true;
        }
        else
        {
            RedWallisDown = false;
        }

        if(LWC.RedWall || LWC.RedWallisLeft)
        {
            RedWallisLeft = true;
        }
        else
        {
            RedWallisLeft = false;
        }

        if(RWC.RedWall || RWC.RedWallisRight)
        {
            RedWallisRight = true;
        }
        else
        {
            RedWallisRight = false;
        }

        if(RedWallisDown && RedWallisUp)
        {
            RedAreaVerticalSiege = true;
        }
        else
        {
            RedAreaVerticalSiege = false;
        }

        if(RedWallisLeft && RedWallisRight)
        {
            RedAreaHorizontalSiege = true;
        }
        else
        {
            RedAreaHorizontalSiege = false;
        }

        if(RedAreaHorizontalSiege && RedAreaVerticalSiege || BlueWall || RedWall)
        {
            RedAreaLeak = false;

        }
        else
        {
            RedAreaLeak = true;
        }

        if(!CheckAroundArea("red") && !BlueWall && !RedWall)
        {
            RedAreaLeak = true;
        }
        else
        {
            RedAreaLeak = false;
        }
        
    }
    #endregion
    }

    void LateUpdate()
    {
        if(!Scapegoat){
        if(!RedWall && !BlueWall)
        {
            if(FWC.BlueAreaLeak || BWC.BlueAreaLeak || LWC.BlueAreaLeak || RWC.BlueAreaLeak)
            {
                BlueAreaLeak = true;
            }
            else
            {
                BlueAreaLeak = false;
            }

            if(BlueAreaVerticalSiege && BlueAreaHorizontalSiege && !BlueAreaLeak)
            {
                BlueArea = true;
            }
            else
            {
                BlueArea = false;
            }


            if(FWC.RedAreaLeak || BWC.RedAreaLeak || LWC.RedAreaLeak || RWC.RedAreaLeak)
            {
                RedAreaLeak = true;
            }
            else
            {
                RedAreaLeak = false;
            }

            if(RedAreaVerticalSiege && RedAreaHorizontalSiege && !RedAreaLeak)
            {
                RedArea = true;
            }
            else
            {
                RedArea = false;
            }
        }

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

    bool CheckAroundArea(string turn)
    {
        if(turn == "blue"){
            if(((FWC.BlueAreaHorizontalSiege && FWC.BlueAreaVerticalSiege) || FWC.BlueWall) && ((BWC.BlueAreaHorizontalSiege && BWC.BlueAreaVerticalSiege) || BWC.BlueWall) && ((LWC.BlueAreaHorizontalSiege && LWC.BlueAreaVerticalSiege) || LWC.BlueWall) && ((RWC.BlueAreaHorizontalSiege && RWC.BlueAreaVerticalSiege) || RWC.BlueWall))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        else if(turn == "red")
        {
            if(((FWC.RedAreaHorizontalSiege && FWC.RedAreaVerticalSiege) || FWC.RedWall) && ((BWC.RedAreaHorizontalSiege && BWC.RedAreaVerticalSiege) || BWC.RedWall) && ((LWC.RedAreaHorizontalSiege && LWC.RedAreaVerticalSiege) || LWC.RedWall) && ((RWC.RedAreaHorizontalSiege && RWC.RedAreaVerticalSiege) || RWC.RedWall))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        else 
        {
            Debug.Log("turn is not blue or red");
            return false;
        }

    }
  }