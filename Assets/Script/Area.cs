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


    [HideInInspector]public bool BlueArea = false;
    [HideInInspector]public bool RedArea = false;
    [HideInInspector]public bool BlueWall = false;
    [HideInInspector]public bool RedWall = false;
    [HideInInspector]public bool castle = false;
    [HideInInspector]public bool pond = false;
    [HideInInspector]public bool Bridge = false;

    TurnManager TM;

    // Start is called before the first frame update
    void Start()
    {
        TM = transform.parent.parent.GetComponent<TurnManager>();
        BlueAreaMarker.SetActive(false);
        RedAreaMarker.SetActive(false);
        BlueWallMarker.SetActive(false);
        RedWallMarker.SetActive(false);
        CastleMarker.SetActive(false);
        PondMarker.SetActive(false);
    }

    // Update is called once per frame
    void Update()
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
        else if(BlueWall)
        {
            BlueWallMarker.SetActive(true);
            Debug.Log("青城壁");
        }
        else if(RedWall)
        {
            RedWallMarker.SetActive(true);
            Debug.Log("赤城壁");
        }
        else if(castle)
        {
            CastleMarker.SetActive(true);
            Debug.Log("城");
        }
        else if(pond)
        {
            PondMarker.SetActive(true);
            Debug.Log("池");
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
