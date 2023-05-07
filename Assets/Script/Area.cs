using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


    [HideInInspector]public bool BlueArea = false;
    [HideInInspector]public bool RedArea = false;
    [HideInInspector]public bool BlueWall = false;
    [HideInInspector]public bool RedWall = false;
    [HideInInspector]public bool castle = false;
    [HideInInspector]public bool pond = false;
    [HideInInspector]public bool Bridge = false;

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
    bool movefoward = false;
    bool moveback = false;
    bool moveleft = false;
    bool moveright = false;
    bool DeadEnd = false;

    //仮想壁用
    bool ImaginaryBlueWall = false;
    bool ImaginaryRedWall = false;

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
        if(!Scapegoat){
        //周囲に壁がないかを検知
        if((FWC.BlueWall || BWC.BlueWall) && (LWC.BlueWall || RWC.BlueWall) && !(FWC.BlueWall && BWC.BlueWall) && !(LWC.BlueWall && RWC.BlueWall) )
        {
            ImaginaryBlueWall = true;
            Debug.Log("(" + ThisPosX + "," + ThisPosY + ")" + "ImaginaryBlueWall On");
        }
        else
        {
            ImaginaryBlueWall = false;
        }

        if((FWC.RedWall || BWC.RedWall) && (LWC.RedWall || RWC.RedWall) && !(FWC.RedWall && BWC.RedWall) && !(LWC.RedWall && RWC.RedWall))
        {
            ImaginaryRedWall = true;
            Debug.Log("(" + ThisPosX + "," + ThisPosY + ")" + "ImaginaryRedWall On");
        }
        else
        {
            ImaginaryRedWall = false;
        }

        }
    }

    public void SwitchArea(int x,int y)
    {
        if(!Scapegoat){
        /*まず上下1マスに壁があるかを確認する行動を繰り返す。
          壁がなくなった場合、今度は横で同じ操作を行う。
          また横方向に壁がなくなった場合縦でいなじことをやる。
          といったように操作を繰り返し行い、
          元の場所に戻ってくる、もしくは縦に進めない状況で、横にも進めなかった場合
          元の場所に帰ってきた場合、i,jを取得しforで中を塗りつぶす
          行き詰まった状態の場合、何もせず終わる
          以上のようなスクリプトをかくといい感じかも*/

        /*追記:自分周辺縦横4マスのうち縦どちらか1マス、横どちらか1マスが同じ陣営の壁だった場合
          その陣営の仮想壁としてフラグを立てて上の処理に組み込んでみる*/

        
        
        moveback = true;
        movefoward = false;
        moveleft = false;
        moveright = false;
        DeadEnd = false;
        
        if(BlueWall)
        {

            

            BlueWalli = x;
            BlueWallj = y;
            BlueMaxi = x;
            BlueMaxj = y;
            BlueMini = x;
            BlueMinj = y;

            while(!DeadEnd)
            {

                PointPos = NeutherWallChecker(BlueWalli,BlueWallj);

                BlueWalli = PointPos.x;
                BlueWallj = PointPos.y;

                if(BlueWalli == x && BlueWallj == y && !DeadEnd)
                {
                    Debug.Log("青包囲");
                    AreaSiege();
                    break;
                }
            }
            
            
        }

        else if(RedWall)
        {

            RedWalli = x;
            RedWallj = y;
            RedMaxi = x;
            RedMaxj = y;
            RedMini = x;
            RedMinj = y;

            while(!DeadEnd)
            {

                NeutherWallChecker(RedWalli,RedWallj);

                RedWalli = PointPos.x;
                RedWallj = PointPos.y;

                if(RedWalli == x && RedWallj == y && !DeadEnd)
                {
                    Debug.Log("赤包囲");
                    AreaSiege();
                    break;
                }

            }
        }
        
        if (!BlueWall && !RedWall)
        {
            if(BlueArea)
            {
                BlueAreaMarker.SetActive(true);
                Debug.Log("青陣地");
            }
            else if(RedArea)
            {
                RedAreaMarker.SetActive(true);
                Debug.Log("赤陣地");
            }
        }
        
        else if(BlueWall)
        {
            BlueWallMarker.SetActive(true);
        }
        else if(RedWall)
        {
            RedWallMarker.SetActive(true);
        }

        else if(castle)
        {
            CastleMarker.SetActive(true);
        }

        else if(pond)
        {
            PondMarker.SetActive(true);
        }

        else
        {
            BlueAreaMarker.SetActive(false);
            RedAreaMarker.SetActive(false);
            BlueWallMarker.SetActive(false);
            RedWallMarker.SetActive(false);
            CastleMarker.SetActive(false);
            PondMarker.SetActive(false);
        }
    }
    }

    (int , int) NeutherWallChecker(int x,int y)
    {
        
        //まず可能な限り一つ前に進んでいた方向に進む
        if(TM.BlueTurn)
        {
            if((TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().FWC.BlueWall || TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().FWC.ImaginaryBlueWall) && movefoward)
            {
                Debug.Log("前壁あり");
                movefoward = true;
                moveback = false;
                moveleft = false;
                moveright = false;
                DeadEnd = false;
                return (x-1,y);
            }

            else if((TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().BWC.BlueWall || TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().BWC.ImaginaryBlueWall) && moveback)
            {
                Debug.Log("後ろ壁あり");
                moveback = true;
                movefoward = false;
                moveleft = false;
                moveright = false;
                DeadEnd = false;
                return (x+1,y);
            }

            else if((TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().LWC.BlueWall || TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().LWC.ImaginaryBlueWall) && moveleft)
            {
                Debug.Log("左壁あり");
                moveleft = true;
                movefoward = false;
                moveback = false;
                moveright = false;
                DeadEnd = false;
                return (x,y-1);
            }

            else if((TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().RWC.BlueWall || TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().RWC.ImaginaryBlueWall) && moveright)
            {
                Debug.Log("右壁あり");
                moveright = true;
                movefoward = false;
                moveback = false;
                moveleft = false;
                DeadEnd = false;
                return (x,y+1);
            }


            
            else if((TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().FWC.BlueWall || TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().FWC.ImaginaryBlueWall) && !moveback)
            {
                Debug.Log("前壁あり");
                movefoward = true;
                moveback = false;
                moveleft = false;
                moveright = false;
                DeadEnd = false;
                return (x-1,y);
            }

            else if((TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().BWC.BlueWall || TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().BWC.ImaginaryBlueWall) && !movefoward)
            {
                Debug.Log("後ろ壁あり");
                moveback = true;
                movefoward = false;
                moveleft = false;
                moveright = false;
                DeadEnd = false;
                return (x+1,y);
            }

            else if((TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().RWC.BlueWall || TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().RWC.ImaginaryBlueWall) && !moveright)
            {
                Debug.Log("左壁あり");
                moveleft = true;
                movefoward = false;
                moveback = false;
                moveright = false;
                DeadEnd = false;
                return (x,y-1);
            }

            else if((TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().RWC.BlueWall || TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().RWC.ImaginaryBlueWall) && !moveleft)
            {
                Debug.Log("右壁あり");
                moveright = true;
                movefoward = false;
                moveback = false;
                moveleft = false;
                DeadEnd = false;
                return (x,y+1);
            }

            else
            {
                Debug.Log("壁なし");
                DeadEnd = true;
                return (x,y);
            }
        }

        else
        {
            if((TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().FWC.RedWall || TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().FWC.ImaginaryRedWall) && movefoward)
            {
                Debug.Log("前壁あり");
                movefoward = true;
                moveback = false;
                moveleft = false;
                moveright = false;
                DeadEnd = false;
                return (x-1,y);
            }

            else if((TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().BWC.RedWall || TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().BWC.ImaginaryRedWall) && moveback)
            {
                Debug.Log("後ろ壁あり");
                moveback = true;
                movefoward = false;
                moveleft = false;
                moveright = false;
                DeadEnd = false;
                return (x+1,y);
            }

            else if((TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().LWC.RedWall || TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().LWC.ImaginaryRedWall) && moveleft)
            {
                Debug.Log("左壁あり");
                moveleft = true;
                movefoward = false;
                moveback = false;
                moveright = false;
                DeadEnd = false;
                return (x,y-1);
            }

            else if((TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().RWC.RedWall || TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().RWC.ImaginaryRedWall) && moveright)
            {
                Debug.Log("右壁あり");
                moveright = true;
                movefoward = false;
                moveback = false;
                moveleft = false;
                DeadEnd = false;
                return (x,y+1);
            }


            
            else if((TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().FWC.RedWall || TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().FWC.ImaginaryRedWall) && !moveback)
            {
                Debug.Log("前壁あり");
                movefoward = true;
                moveback = false;
                moveleft = false;
                moveright = false;
                DeadEnd = false;
                return (x-1,y);
            }

            else if((TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().BWC.RedWall || TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().BWC.ImaginaryRedWall) && !movefoward)
            {
                Debug.Log("後ろ壁あり");
                moveback = true;
                movefoward = false;
                moveleft = false;
                moveright = false;
                DeadEnd = false;
                return (x+1,y);
            }

            else if((TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().RWC.RedWall || TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().RWC.ImaginaryRedWall) && !moveright)
            {
                Debug.Log("左壁あり");
                moveleft = true;
                movefoward = false;
                moveback = false;
                moveright = false;
                DeadEnd = false;
                return (x,y-1);
            }

            else if((TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().RWC.RedWall || TM.transform.GetChild(x).GetChild(y).GetComponent<Area>().RWC.ImaginaryRedWall) && !moveleft)
            {
                Debug.Log("右壁あり");
                moveright = true;
                movefoward = false;
                moveback = false;
                moveleft = false;
                DeadEnd = false;
                return (x,y+1);
            }

            else
            {
                Debug.Log("壁なし");
                DeadEnd = true;
                return (x,y);
            }
        }
    }


    void AreaSiege()
    {

        if(TM.BlueTurn)
        {
            for(int i = BlueMini; i <= BlueMaxi; i++)
            {
                for(int j = BlueMinj; j <= BlueMaxj; j++)
                {
                    TM.transform.GetChild(i).GetChild(j).GetComponent<Area>().BlueArea = true;
                }
            }
        }
        
        else
        {
            for(int i = RedMini; i <= RedMaxi; i++)
            {
                for(int j = RedMinj; j <= RedMaxj; j++)
                {
                    TM.transform.GetChild(i).GetChild(j).GetComponent<Area>().RedArea = true;
                }
            }
        }

    }
}
