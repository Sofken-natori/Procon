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
    Button RedButton;
    Button BlueButton;
    Transform square;
    GameObject Board;
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
        
    }

    public void MoveBridge()
    {
        
    }

    public void BuildAndDestroyBridge()
    {
        
    }

    public Vector2 StartBridgePosition(int x,int y)
    {
        // Set the bridge position
        square = this.transform.GetChild(x).GetChild(y);
        return square.position;
    }
}
