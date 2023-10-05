using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeitherWallChacker : MonoBehaviour
{
    public bool BlueWall = false;
    public bool RedWall = false;

    void Update()
    {
        if (BlueWall == true)
        {
            Debug.Log(this.gameObject.name + "BlueWall");
        }
        else if (RedWall == true)
        {
            Debug.Log(this.gameObject.name + "RedWall");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(this.gameObject.tag + "TriggerEnter");
        if (other.gameObject.tag == "BlueWall")
        {
            RedWall = false;
            BlueWall = true;
        }
        else if (other.gameObject.tag == "RedWall")
        {
            RedWall = true;
            BlueWall = false;
        }
    }

    

    void OnTriggerExit(Collider other)
    {

        Debug.Log(this.gameObject.name + "TriggerExit");
        if(other.gameObject.tag == "BlueWall" || other.gameObject.tag == "RedWall")
        {
            BlueWall = false;
            RedWall = false;
        }
    }
}
