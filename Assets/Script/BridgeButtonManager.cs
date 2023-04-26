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
        ButtonIntaract.interactable = false;
        BoardY--;
        this.transform.position = TM.MoveBridge(BoardY, BoardX);
    }

    public void MoveBackwardBridge()
    {
        // Move the bridge backward
        MoveArrow.SetActive(false);
        ButtonIntaract.interactable = false;
        BoardY++;
        this.transform.position = TM.MoveBridge(BoardY, BoardX);
    }

    public void MoveRightBridge()
    {
        // Move the bridge right
        MoveArrow.SetActive(false);
        ButtonIntaract.interactable = false;
        BoardX++;
        this.transform.position = TM.MoveBridge(BoardY, BoardX);
    }

    public void MoveLeftBridge()
    {
        // Move the bridge left
        MoveArrow.SetActive(false);
        ButtonIntaract.interactable = false;
        BoardX--;
        this.transform.position = TM.MoveBridge(BoardY, BoardX);
    }

    public void MoveLeftForwardBridge()
    {
        // Move the bridge left forward
        MoveArrow.SetActive(false);
        ButtonIntaract.interactable = false;
        BoardX--;
        BoardY--;
        this.transform.position = TM.MoveBridge(BoardY, BoardX);
    }

    public void MoveRightForwardBridge()
    {
        // Move the bridge right forward
        MoveArrow.SetActive(false);
        ButtonIntaract.interactable = false;
        BoardX++;
        BoardY--;
        this.transform.position = TM.MoveBridge(BoardY, BoardX);
    }

    public void MoveLeftBackwardBridge()
    {
        // Move the bridge left backward
        MoveArrow.SetActive(false);
        ButtonIntaract.interactable = false;
        BoardX--;
        BoardY++;
        this.transform.position = TM.MoveBridge(BoardY, BoardX);
    }

    public void MoveRightBackwardBridge()
    {
        // Move the bridge right backward
        MoveArrow.SetActive(false);
        ButtonIntaract.interactable = false;
        BoardX++;
        BoardY++;
        this.transform.position = TM.MoveBridge(BoardY, BoardX);
    }

    public void BuildAndDestroyForward()
    {
        // Build and destroy the bridge forward
        BuildAndDestroyArrow.SetActive(false);
        ButtonIntaract.interactable = false;
        TM.BuildAndDestroyBridge(BoardY, BoardX);
    }

    public void BuildAndDestroyBackward()
    {
        // Build and destroy the bridge backward
        BuildAndDestroyArrow.SetActive(false);
        ButtonIntaract.interactable = false;
        TM.BuildAndDestroyBridge(BoardY, BoardX);
    }

    public void BuildAndDestroyRight()
    {
        // Build and destroy the bridge right
        BuildAndDestroyArrow.SetActive(false);
        ButtonIntaract.interactable = false;
        TM.BuildAndDestroyBridge(BoardY, BoardX);
    }

    public void BuildAndDestroyLeft()
    {
        // Build and destroy the bridge left
        BuildAndDestroyArrow.SetActive(false);
        ButtonIntaract.interactable = false;
        TM.BuildAndDestroyBridge(BoardY, BoardX);
    }

}
