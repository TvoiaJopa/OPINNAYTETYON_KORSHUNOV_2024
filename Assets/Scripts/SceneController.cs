using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadScene1()
    {
        SceneManager.LoadScene("Scene_1");
    }

    public void LoadScene2()
    {
        SceneManager.LoadScene("Scene_2");

    }

}
