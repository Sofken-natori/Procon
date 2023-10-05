using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    
    GameObject gb;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(333);
       // DontDestroyOnLoad(this.gameObject);

        //DontDestroyOnLoad(this.gameObject);
       // gb = GameObject.Find("UserInterface/Board").GetComponent<Scene>();
    }
    private void Awake()
    {
       // DontDestroyOnLoad(this.gameObject);
    }
    public void NotDestroy()
    {
       // DontDestroyOnLoad(this);
     
      
    }
}
