using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    [Header("最大ターン数"), SerializeField] int MaxTurnNumber = 1;
    [Header("生成する駒の数"), SerializeField] int PieceNumber = 1;
    [Header("縦のマス数")]public int BoardXMax = 15;
    [Header("横のマス数")]public int BoardYMax = 15;
    [Header("赤い駒のプレハブ"), SerializeField] GameObject RedBridge;
    [Header("青い駒のプレハブ"), SerializeField] GameObject BlueBridge;
    [HideInInspector]public bool BlueTurn = false;
    [HideInInspector]public bool UntapPhase = false;
    [HideInInspector]public bool TurnEnd = false;

    Area area;
    Button RedButton;
    Button BlueButton;
    Transform square;
    GameObject Board;
    int BridgeActCount = 0;
    int BridgestandbyCount = 0;
    int NowTurn = 0;
    int i = 1;


    
    void Awake()
    {
       Board = GameObject.Find("BoardBridge");
       while(i <= PieceNumber)
       {
            // Instantiate the piece
            RedBridge = Instantiate(RedBridge, new Vector2(i,i), Quaternion.identity, Board.transform);
            BlueBridge = Instantiate(BlueBridge, new Vector2(i*3,i*3), Quaternion.identity, Board.transform);
            i++;
       }
    }

    // Update is called once per frame
    void Update()
    {
        if(BridgeActCount >= PieceNumber)
        {
            if(!BlueTurn)
            {
                NowTurn++;
            }
            Debug.Log("TurnChange");
            BlueTurn = !BlueTurn;
            BridgeActCount = 0;
            UntapPhase = true;
        }

        if(NowTurn >= MaxTurnNumber)
        {
            Debug.Log("GameSet");
        }
    }

    public void Bridgestandby()
    {
        BridgestandbyCount++;
        if(BridgestandbyCount >= PieceNumber)
        {
            UntapPhase = false;
            BridgestandbyCount = 0;
        }
    }

    public void BuildAndDestroyBridge(int x,int y)
    {
        BridgeActCount++;
        area = this.transform.GetChild(x).GetChild(y).GetComponent<Area>();
        if(area.RedWall || area.BlueWall)
        {
                area.RedWall = false;
                area.BlueWall = false;
                area.RedAreaLeak = true;
                area.BlueAreaLeak  = true;
        }
        
        else if(BlueTurn)
        {
            area.BlueWall = true;
            area.BlueAreaLeak = false;
        }

        else
        {
            area.RedWall = true;
            area.RedAreaLeak = false;
        }
    }

    public bool CanMove(int x,int y)
    {
        area = this.transform.GetChild(x).GetChild(y).GetComponent<Area>();
        if(area.RedWall || area.BlueWall || area.pond || area.Bridge)
        {
            return false;
        }

        else
        {
            return true;
        }
    }

    public Vector2 MoveBridge(int x,int y)
    {
        // Set the bridge position
        square = this.transform.GetChild(x).GetChild(y);
        area = square.GetComponent<Area>();
        BridgeActCount++;
        area.Bridge = true;
        return square.position;
    }
}
