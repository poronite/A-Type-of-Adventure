using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField]
    private float fadeInDuration = 0.5f;

    [SerializeField]
    private float fadeOutDuration = 0.5f;

    [SerializeField]
    private VideoPlayer cutsceneVideoPlayer;

    [SerializeField]
    private VideoPlayer cutsceneVideoPlayerLoop;

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
        yield return StartCoroutine(FadeCutsceneLoadingScreen(fadeInDuration, 1));

        cutsceneVideo = videoClip;
        cutsceneVideoPlayer.clip = cutsceneVideo;
        cutsceneVideoPlayer.Prepare();

        cutsceneVideoLoop = videoClipLoop;
        cutsceneVideoPlayerLoop.clip = cutsceneVideoLoop;
        cutsceneVideoPlayerLoop.Prepare();

        while (!cutsceneVideoPlayer.isPrepared && !cutsceneVideoPlayerLoop.isPrepared)
        {
            yield return null;
        }

        cutsceneVideoPlayer.Play();

        yield return StartCoroutine(FadeCutsceneLoadingScreen(fadeOutDuration, 0));
    }


    //loop the cutscene with a loop version of the cutscene
    private void LoopVideo(VideoPlayer videoPlayer)
    {
        if (videoPlayer.clip == cutsceneVideo)
        {
            Debug.Log("Looping video");

            LoopVideo();
        }
    }

    private void LoopVideo()
    {
        cutsceneVideoPlayerLoop.Play();

        cutsceneVideoPlayer.Stop();
    }

    public void PauseVideo()
    {
        if (cutsceneVideoPlayer.isPlaying)
        {
            cutsceneVideoPlayer.Pause();
        }
        else if (cutsceneVideoPlayerLoop.isPlaying)
        {
            cutsceneVideoPlayerLoop.Pause();
        }
    }

    public void ResumeVideo()
    {
        if (cutsceneVideoPlayer.isPaused)
        {
            cutsceneVideoPlayer.Play();
        }
        else if (cutsceneVideoPlayerLoop.isPaused)
        {
            cutsceneVideoPlayerLoop.Play();
        }
    }


    public IEnumerator StopVideo()
    {
        yield return StartCoroutine(FadeCutsceneLoadingScreen(fadeInDuration, 1));

        cutsceneVideoPlayer.Stop();
        cutsceneVideoPlayerLoop.Stop();

        cutsceneVideoPlayer.clip = null;
        cutsceneVideoPlayerLoop.clip = null;

        yield return StartCoroutine(FadeCutsceneLoadingScreen(fadeOutDuration, 0));
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
