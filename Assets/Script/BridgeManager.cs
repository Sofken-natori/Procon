using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeManager : MonoBehaviour
{
    [Header("TurnManager"), SerializeField]TurnManager TM;

    int i = 0;
    int childCount;
    int[] BridgeX;
    int[] BridgeY;


    void Start()
    {
        childCount = this.transform.childCount;
        BridgeX = new int[childCount];
        BridgeY = new int[childCount];
        for (int i = 0; i < BridgeX.Length; i++)
        {
            BridgeX[i] = -1;
        }

        for (int i = 0; i < BridgeY.Length; i++)
        {
            BridgeY[i] = -1;
        }

        while (i < childCount)
        {
            while(ContainsNumber(BridgeX, this.transform.GetChild(i).GetComponent<BridgeButtonManager>().BoardX) && ContainsNumber(BridgeY, this.transform.GetChild(i).GetComponent<BridgeButtonManager>().BoardY))
            {
                this.transform.GetChild(i).GetComponent<BridgeButtonManager>().BridgePosition();
            }
            BridgeX[i] = this.transform.GetChild(i).GetComponent<BridgeButtonManager>().BoardX;
            
            string X = "BridgeX:";
            foreach (int n in BridgeX)
            {
                X += n.ToString() + " ";
            }
            Debug.Log(X);
            
            BridgeY[i] = this.transform.GetChild(i).GetComponent<BridgeButtonManager>().BoardY;
            
            string Y = "BridgeY:";
            foreach (int n in BridgeY)
            {
                Y += n.ToString() + " ";
            }
            Debug.Log(Y);
            
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool ContainsNumber(int[] array, int number)
    {
        foreach (int n in array)
        {
            if (n == number)
            {
                return true;
            }
        }
        return false;
    }

    
}
