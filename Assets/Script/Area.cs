using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{

    [Header("TurnManager"), SerializeField]TurnManager TM;
    [Header("赤陣地マーカー"), SerializeField]GameObject RedAreaMarker;
    [Header("青陣地マーカー"), SerializeField]GameObject BlueAreaMarker;
    [Header("城マーカー"), SerializeField]GameObject CastleMarker;
    [Header("池マーカー"), SerializeField]GameObject PondMarker;


    bool BlueArea = false;
    bool RedArea = false;
    bool BlueWall = false;
    bool RedWall = false;
    bool castle = false;
    bool pond = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
