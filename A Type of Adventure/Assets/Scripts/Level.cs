using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField]
    private string PlotText = "";

    private void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Typing>().SetText(PlotText);

    }
}
