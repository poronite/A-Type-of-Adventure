using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncounterController : MonoBehaviour
{
    private CanvasGroup cutsceneLoadingScreen;

    private Image cutsceneBackground; 
    private Transform cutsceneParent;
    private Transform currentCutscene;

    private RectTransform[] comicStripes;

    private int currentStripe;
    private Vector3 previousStripePosition;
    private float transitionDuration;

    public void EncounterTriggered(EncountersTemplate encounter)
    {
        switch (encounter.EncounterType)
        {
            case EncounterType.Gameplay:
                break;
            case EncounterType.Cutscene:

                if (encounter.EndOfCutscene)
                {
                    EndCutscene();
                    break;
                }
                transitionDuration = encounter.TransitionDuration;

                if (encounter.NewCutscene)
                {
                    cutsceneParent = GameObject.FindGameObjectWithTag("Cutscenes").transform;

                    currentCutscene = Instantiate(encounter.CutscenePrefab, cutsceneParent, false).transform;
                    SetCutscene();
                }

                ChangeStripe();
                break;
            default:
                break;
        }
    }


    private void SetCutscene()
    {
        cutsceneLoadingScreen = GameObject.FindGameObjectWithTag("CutsceneLoadingScreen").GetComponent<CanvasGroup>();
        cutsceneBackground = GameObject.FindGameObjectWithTag("CutsceneBackground").GetComponent<Image>();

        StartCoroutine(FadeCutsceneLoadingScreen(1f, 1));

        currentStripe = -1;

        comicStripes = new RectTransform[currentCutscene.childCount];

        for (int i = 0; i < currentCutscene.childCount; i++)
        {
            comicStripes[i] = currentCutscene.GetChild(i).GetComponent<RectTransform>();
        }

        cutsceneBackground.enabled = true;

        StartCoroutine(FadeCutsceneLoadingScreen(1f, 0));
    }


    private void EndCutscene()
    {
        StartCoroutine(FadeCutsceneLoadingScreen(1f, 1));

        cutsceneBackground.enabled = false;
        Destroy(currentCutscene.gameObject);

        StartCoroutine(FadeCutsceneLoadingScreen(1f, 0));
    }


    IEnumerator FadeCutsceneLoadingScreen(float duration, float finalAlpha)
    {
        float time = 0f;

        while (time < duration)
        {
            cutsceneLoadingScreen.alpha = Mathf.Lerp(cutsceneLoadingScreen.alpha, finalAlpha, time/duration);
            time += Time.deltaTime;
            yield return null;
        }

        cutsceneLoadingScreen.alpha = finalAlpha;
    }


    private void ChangeStripe()
    {
        currentStripe += 1;

        StartCoroutine(MoveStripe(previousStripePosition));

        previousStripePosition = comicStripes[currentStripe].transform.position;
    }


    IEnumerator MoveStripe(Vector3 previousStripeNewPosition)
    {
        Debug.Log(currentStripe);

        float time = 0f;

        while (time < transitionDuration)
        {
            if (currentStripe == 0)
            {
                comicStripes[currentStripe].position = Vector3.Lerp(comicStripes[currentStripe].position, Vector3.zero, (time / transitionDuration));
            }

            if (currentStripe > 0 && currentStripe < comicStripes.Length)
            {
                comicStripes[currentStripe - 1].position = Vector3.Lerp(comicStripes[currentStripe - 1].position, -previousStripeNewPosition, (time / transitionDuration));

                comicStripes[currentStripe].position = Vector3.Lerp(comicStripes[currentStripe].position, Vector3.zero, (time / transitionDuration));
                
            }

            if (currentStripe == comicStripes.Length)
            {
                comicStripes[currentStripe - 1].position = Vector3.Lerp(comicStripes[currentStripe - 1].position, -previousStripeNewPosition, (time / transitionDuration));
            }

            time += Time.deltaTime;

            yield return null;
        }

        if (currentStripe == 0)
        {
            comicStripes[currentStripe].position = Vector3.zero;
        }

        if (currentStripe > 0 && currentStripe < comicStripes.Length - 1)
        {
            comicStripes[currentStripe - 1].position = -previousStripeNewPosition;

            comicStripes[currentStripe].position = Vector3.zero;

        }

        if (currentStripe == comicStripes.Length - 1)
        {
            comicStripes[currentStripe - 1].position = -previousStripeNewPosition;
        }
    }
}
