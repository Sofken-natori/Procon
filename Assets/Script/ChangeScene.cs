using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    [Header("Scene Name"), SerializeField]string sceneName;

    public void ChangeSceneByName()
    {
        SceneManager.LoadScene(sceneName);
    }
}
