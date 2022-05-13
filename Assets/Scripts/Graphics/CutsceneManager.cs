using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer cutsceneVideoPlayer;

    [SerializeField]
    private CanvasGroup cutsceneLoadingScreen;

    private VideoClip cutsceneVideo;

    private VideoClip cutsceneVideoLoop;



    private void Start()
    {
        //when cutscene reaches the end make sure to loop with a loop version of the cutscene
        cutsceneVideoPlayer.loopPointReached += LoopVideo;
    }


    //play cutscene
    public IEnumerator PlayVideo(VideoClip videoClip, VideoClip videoClipLoop)
    {
        yield return StartCoroutine(FadeCutsceneLoadingScreen(1f, 1));

        cutsceneVideo = videoClip;
        cutsceneVideoLoop = videoClipLoop;

        cutsceneVideoPlayer.clip = cutsceneVideo;

        cutsceneVideoPlayer.Play();

        yield return StartCoroutine(FadeCutsceneLoadingScreen(1f, 0));
    }


    //loop the cutscene with a loop version of the cutscene
    private void LoopVideo(VideoPlayer videoPlayer)
    {
        if (videoPlayer.clip == cutsceneVideo)
        {
            Debug.Log("Looping video");

            StartCoroutine(LoopVideoCoroutine());
        }
    }

    private IEnumerator LoopVideoCoroutine()
    {
        yield return StartCoroutine(FadeCutsceneLoadingScreen(0.3f, 1));

        cutsceneVideoPlayer.clip = cutsceneVideoLoop;

        cutsceneVideoPlayer.Play();

        yield return StartCoroutine(FadeCutsceneLoadingScreen(0.3f, 0));
    }


    public IEnumerator StopVideo()
    {
        yield return StartCoroutine(FadeCutsceneLoadingScreen(1f, 1));

        cutsceneVideoPlayer.Stop();

        cutsceneVideoPlayer.clip = null;

        yield return StartCoroutine(FadeCutsceneLoadingScreen(1f, 0));
    }

    IEnumerator FadeCutsceneLoadingScreen(float duration, float finalAlpha)
    {
        float time = 0f;

        while (time < duration)
        {
            cutsceneLoadingScreen.alpha = Mathf.Lerp(cutsceneLoadingScreen.alpha, finalAlpha, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        cutsceneLoadingScreen.alpha = finalAlpha;
    }
}
