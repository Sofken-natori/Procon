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
    [HideInInspector]public int BoardX;
    [HideInInspector]public int BoardY;
    [HideInInspector]public int Seed;
    
    bool CanMove = false;
    GameObject Board;
    Button ButtonIntaract;
    TurnManager TM;
    
    
    void Awake()
    {
        ButtonIntaract = GetComponent<Button>();
        MoveArrow.SetActive(false);
        BuildAndDestroyArrow.SetActive(false);
        BuildAndDestroySelectButton.SetActive(false);
        MoveSelectButton.SetActive(false);
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond*Seed);
        ButtonIntaract = GetComponent<Button>();
        Board = GameObject.Find("UserInterface/Board");
        TM = Board.GetComponent<TurnManager>();
        BridgeStartPosition();
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
        Seed = (int)transform.position.x + (int)transform.position.y;
        BoardX = UnityEngine.Random.Range(0, 15);
        BoardY = UnityEngine.Random.Range(0, 15);
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
        this.transform.position = TM.MoveBridge(BoardY, BoardX);
        
    }

    public void MoveLeftBridge()
    {
        // Move the bridge left
        MoveArrow.SetActive(false);
        BridgeRester();
        TM.isBridgeReseter(BoardY,BoardX);
        ButtonIntaract.interactable = false;
        CanMove = TM.CanMove(BoardY,BoardX-1);
        if(CanMove)
        {
            BoardX--;
        }
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
}
