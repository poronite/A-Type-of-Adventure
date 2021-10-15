using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenGraphicsScene : MonoBehaviour
{
    void Awake()
    {
        SceneManager.LoadSceneAsync("Graphics", LoadSceneMode.Additive);
    }
}
