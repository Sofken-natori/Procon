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
    [Header("シード値"),SerializeField]int Seed;
    
    
    GameObject Board;
    int BoardX;
    int BoardY;
    
    void Start()
    {
        Board = GameObject.Find("UserInterface/Board");
        MoveArrow.SetActive(false);
        BuildAndDestroyArrow.SetActive(false);
        BuildAndDestroySelectButton.SetActive(false);
        MoveSelectButton.SetActive(false);
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond*Seed);
        BoardX = UnityEngine.Random.Range(0, 15);
        BoardY = UnityEngine.Random.Range(0, 15);
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
        Debug.Log("MoveForwardBridge");
        MoveArrow.SetActive(false);
    }

    public void MoveBackwardBridge()
    {
        // Move the bridge backward
        Debug.Log("MoveBackwardBridge");
        MoveArrow.SetActive(false);
    }

    public void MoveRightBridge()
    {
        // Move the bridge right
        Debug.Log("MoveRightBridge");
        MoveArrow.SetActive(false);
    }

    public void MoveLeftBridge()
    {
        // Move the bridge left
        Debug.Log("MoveLeftBridge");
        MoveArrow.SetActive(false);
    }

    public void MoveLeftForwardBridge()
    {
        // Move the bridge left forward
        Debug.Log("MoveLeftForwardBridge");
        MoveArrow.SetActive(false);
    }

    public void MoveRightForwardBridge()
    {
        // Move the bridge right forward
        Debug.Log("MoveRightForwardBridge");
        MoveArrow.SetActive(false);
    }

    public void MoveLeftBackwardBridge()
    {
        // Move the bridge left backward
        Debug.Log("MoveLeftBackwardBridge");
        MoveArrow.SetActive(false);
    }

    public void MoveRightBackwardBridge()
    {
        // Move the bridge right backward
        Debug.Log("MoveRightBackwardBridge");
        MoveArrow.SetActive(false);
    }

    public void BuildAndDestroyForward()
    {
        // Build and destroy the bridge forward
        Debug.Log("BuildAndDestroyForward");
        BuildAndDestroyArrow.SetActive(false);
    }

    public void BuildAndDestroyBackward()
    {
        // Build and destroy the bridge backward
        Debug.Log("BuildAndDestroyBackward");
        BuildAndDestroyArrow.SetActive(false);
    }

    public void BuildAndDestroyRight()
    {
        // Build and destroy the bridge right
        Debug.Log("BuildAndDestroyRight");
        BuildAndDestroyArrow.SetActive(false);
    }

    public void BuildAndDestroyLeft()
    {
        // Build and destroy the bridge left
        Debug.Log("BuildAndDestroyLeft");
        BuildAndDestroyArrow.SetActive(false);
    }

}
