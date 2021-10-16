using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenGraphicsScene : MonoBehaviour
{
    void Awake()
    {
        StartCoroutine(LoadGraphicsScene());
    }

    IEnumerator LoadGraphicsScene()
    {
        AsyncOperation loadedLevel = SceneManager.LoadSceneAsync("Graphics", LoadSceneMode.Additive);
        while (!loadedLevel.isDone)
        {
            yield return loadedLevel;
        }

        GameObject.FindGameObjectWithTag("Player").GetComponent<Adventure>().StartAdventure();
    }
}
