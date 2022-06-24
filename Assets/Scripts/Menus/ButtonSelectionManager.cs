using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectionManager : MonoBehaviour
{
    [SerializeField]
    private GameObject lastSelectedButtonUI;

    private RectTransform pointer;

    private void Start()
    {
        pointer = GameObject.FindGameObjectWithTag("Pointer")?.GetComponent<RectTransform>();
    }

    private void OnDisable()
    {
        EventSystem.current?.SetSelectedGameObject(null);
    }

    private void Update()
    {
        ButtonSelection();
    }

    private void ButtonSelection()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            lastSelectedButtonUI = EventSystem.current.currentSelectedGameObject;

            if (pointer != null)
                pointer.position = new Vector3(pointer.position.x, lastSelectedButtonUI.transform.position.y, pointer.position.z);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(lastSelectedButtonUI);
        }
    }
}
