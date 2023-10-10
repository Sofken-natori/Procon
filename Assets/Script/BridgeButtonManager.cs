using ServerConnector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BridgeButtonManager : MonoBehaviour
{

    [Header("移動用矢印オブジェクト"),SerializeField]GameObject MoveArrow;
    [Header("建造、破壊用矢印オブジェクト"),SerializeField]GameObject BuildAndDestroyArrow;
    [Header("建造、破壊選択ボタン"),SerializeField]GameObject BuildAndDestroySelectButton;
    [Header("移動選択ボタン"),SerializeField]GameObject MoveSelectButton;
    [Header("コマの色は青か"),SerializeField]bool BlueTurn;
    [Header("駒のX位置")]public int BoardX;
    [Header("駒のY位置")]public int BoardY;
    // 0:滞在,1:移動,2:建造,3:移動
    [Header("行動の種類")]public int ActionType;
    // 0:無方向,1左上,2上,3右上,4.右,5.右下,6.下,7.左下,8.左
    [Header("移動方向")]public int MoveDirection;
    
    
    bool CanMove = false;
    [System.NonSerialized]public int BridgeID = -1;
    public Button ButtonIntaract;
    public TurnManager TM;
    
    void Awake()
    {
        MoveArrow.SetActive(false);
        BuildAndDestroyArrow.SetActive(false);
        BuildAndDestroySelectButton.SetActive(false);
        MoveSelectButton.SetActive(false);
        ButtonIntaract = GetComponent<Button>();
    }

    void Update()
    {
        if ((BlueTurn && TM.BlueTurn || !BlueTurn && !TM.BlueTurn) && TM.UntapPhase)
        {
            // Myturn

            ButtonIntaract.interactable = true;
            TM.Bridgestandby();
        }
    }

    public void BridgeStartPosition()
    {
        this.transform.position = TM.MoveBridge(BoardY, BoardX);
    }


    public void ClickBridge()
    {
        BuildAndDestroySelectButton.SetActive(true);
        MoveSelectButton.SetActive(true);
        MoveArrow.SetActive(false);
        BuildAndDestroyArrow.SetActive(false);
    }

    public void MoveArrowVisible()
    {
        MoveSelectButton.SetActive(false);
        BuildAndDestroySelectButton.SetActive(false);
        MoveArrow.SetActive(true);
    }

    public void BuildAndDestroyArrowVisible()
    {
        MoveSelectButton.SetActive(false);
        BuildAndDestroySelectButton.SetActive(false);
        BuildAndDestroyArrow.SetActive(true);
    }

    public void MoveForwardBridge()
    {
        // Move the bridge forward
        MoveArrow.SetActive(false);
        BridgeRester();
        TM.isBridgeReseter(BoardY,BoardX);
        ButtonIntaract.interactable = false;
        CanMove = TM.CanMove(BoardY-1,BoardX);
        if(CanMove)
        {
            BoardY--;
        }
        ActionType = 1;
        MoveDirection = 2;
        this.transform.position = TM.MoveBridge(BoardY, BoardX);
    }

    public void MoveBackwardBridge()
    {
        // Move the bridge backward
        MoveArrow.SetActive(false);
        BridgeRester();
        TM.isBridgeReseter(BoardY,BoardX);
        ButtonIntaract.interactable = false;
        CanMove = TM.CanMove(BoardY+1,BoardX);
        if(CanMove)
        {
            BoardY++;
        }
        ActionType = 1;
        MoveDirection = 6;
        this.transform.position = TM.MoveBridge(BoardY, BoardX);
        
    }

    public void MoveRightBridge()
    {
        // Move the bridge right
        MoveArrow.SetActive(false);
        BridgeRester();
        TM.isBridgeReseter(BoardY,BoardX);
        ButtonIntaract.interactable = false;
        CanMove = TM.CanMove(BoardY,BoardX+1);
        if(CanMove)
        {
            BoardX++;
        }
        ActionType = 1;
        MoveDirection = 4;
        this.transform.position = TM.MoveBridge(BoardY, BoardX);
        
    }

    public void MoveLeftBridge()
    {
        // Move the bridge left
        MoveArrow.SetActive(false);
        BridgeRester();       //1   2
        TM.isBridgeReseter(BoardY,BoardX);
        ButtonIntaract.interactable = false;
        CanMove = TM.CanMove(BoardY,BoardX-1);
        if(CanMove)
        {
            BoardX--;
        }
        ActionType = 1;
        MoveDirection = 8;
        this.transform.position = TM.MoveBridge(BoardY, BoardX);
        
    }

    public void MoveLeftForwardBridge()
    {
        // Move the bridge left forward
        MoveArrow.SetActive(false);
        BridgeRester();
        TM.isBridgeReseter(BoardY,BoardX);
        ButtonIntaract.interactable = false;
        CanMove = TM.CanMove(BoardY-1,BoardX-1);
        if(CanMove)
        {
            BoardX--;
            BoardY--;
        }
        ActionType = 1;
        MoveDirection = 1;
        this.transform.position = TM.MoveBridge(BoardY, BoardX);
        
    }

    public void MoveRightForwardBridge()
    {
        // Move the bridge right forward
        MoveArrow.SetActive(false);
        BridgeRester();
        TM.isBridgeReseter(BoardY,BoardX);
        ButtonIntaract.interactable = false;
        CanMove = TM.CanMove(BoardY-1,BoardX+1);
        if(CanMove)
        {
            BoardX++;
            BoardY--;
        }
        ActionType = 1;
        MoveDirection = 3;
        this.transform.position = TM.MoveBridge(BoardY, BoardX);
        
    }

    public void MoveLeftBackwardBridge()
    {
        // Move the bridge left backward
        MoveArrow.SetActive(false);
        BridgeRester();
        TM.isBridgeReseter(BoardY,BoardX);
        ButtonIntaract.interactable = false;
        CanMove = TM.CanMove(BoardY+1,BoardX-1);
        if(CanMove)
        {
            BoardX--;
            BoardY++;
        }
        ActionType = 1;
        MoveDirection = 7;
        this.transform.position = TM.MoveBridge(BoardY, BoardX);
    }

    public void MoveRightBackwardBridge()
    {
        // Move the bridge right backward
        MoveArrow.SetActive(false);
        BridgeRester();
        TM.isBridgeReseter(BoardY,BoardX);
        ButtonIntaract.interactable = false;
        CanMove = TM.CanMove(BoardY+1,BoardX+1);
        if(CanMove)
        {
            BoardX++;
            BoardY++;
        }
        ActionType = 1;
        MoveDirection = 5;
        this.transform.position = TM.MoveBridge(BoardY, BoardX);
        
    }

    public void BuildAndDestroyForward()
    {
        // Build and destroy the bridge forward
        BuildAndDestroyArrow.SetActive(false);
        BridgeRester();
        ButtonIntaract.interactable = false;
        TM.BuildAndDestroyBridge(BoardY-1, BoardX);
        
    }

    public void BuildAndDestroyBackward()
    {
        // Build and destroy the bridge backward
        BuildAndDestroyArrow.SetActive(false);
        BridgeRester();
        ButtonIntaract.interactable = false;
        TM.BuildAndDestroyBridge(BoardY+1, BoardX);
    }

    public void BuildAndDestroyRight()
    {
        // Build and destroy the bridge right
        BuildAndDestroyArrow.SetActive(false);
        BridgeRester();
        ButtonIntaract.interactable = false;
        TM.BuildAndDestroyBridge(BoardY, BoardX+1);
    }

    public void BuildAndDestroyLeft()
    {
        // Build and destroy the bridge left
        BuildAndDestroyArrow.SetActive(false);
        BridgeRester();
        ButtonIntaract.interactable = false;
        TM.BuildAndDestroyBridge(BoardY, BoardX-1);
    }

    public void BridgeRester()
    {
        ButtonIntaract.interactable = false;
        TM.BridgeRest();
    }

    public void BridgeApplyer(int x,int y)
    {
        BoardX = x;
        BoardY = y;
        this.transform.position = TM.MoveBridge(BoardY,BoardX);
    }

    /// <summary>
    /// 駒の行動と方向を返す
    /// 行動の種類は0:滞在,1:移動,2:建造,3:移動
    /// 行動の方向は0:無方向,1左上,2上,3右上,4.右,5.右下,6.下,7.左下,8.左
    /// </summary>
    /// <returns></returns>
    public Move GetMove()
    {
        Move move = new Move();
        move.dir = MoveDirection;
        move.type = ActionType;
        return move;
    }
}