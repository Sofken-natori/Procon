using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    [Header("生成する駒の数"), SerializeField] int PieceNumber = 1;
    [Header("赤い駒のプレハブ"), SerializeField] GameObject RedBridge;
    [Header("青い駒のプレハブ"), SerializeField] GameObject BlueBridge;
    [HideInInspector]public bool BlueTurn = false;
    [HideInInspector]public bool UntapPhase = false;
    Button RedButton;
    Button BlueButton;
    Transform square;
    GameObject Board;
    int BridgeActCount = 0;
    int BridgestandbyCount = 0;
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
            BlueTurn = !BlueTurn;
            BridgeActCount = 0;
            UntapPhase = true;
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

    public void BuildAndDestroyBridge()
    {
        BridgeActCount++;
    }

    public Vector2 MoveBridge(int x,int y)
    {
        // Set the bridge position
        square = this.transform.GetChild(x).GetChild(y);
        BridgeActCount++;
        return square.position;
    }
}
