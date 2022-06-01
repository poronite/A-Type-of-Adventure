using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace ATOA
{
    public static class ATOA_Utilities
    {
        public static IEnumerator FadeLoadingScreen(float targetValue, float duration, CanvasGroup uiElement)
        {
            float startValue = uiElement.alpha;
            float time = 0f;

            while (time < duration)
            {
                uiElement.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            //this is here to guarantee that the alpha is 1 (or 0) instead of a very close float value
            uiElement.alpha = targetValue;
        }

        //remove last used word and generate a new word
        public static string GenerateWord(List<string> wordList, string lastGeneratedWord)
        {
            int newWordIndex;

            //if I don't make a clone the function will end up removing the words from the original list lol
            List<string> wordListClone = new List<string>(wordList);

            //reset random
            Random.InitState((int)Time.time);

            //remove last used word
            if (lastGeneratedWord != string.Empty)
                wordListClone.Remove(lastGeneratedWord);

            //select new word to generate
            newWordIndex = Random.Range(0, wordListClone.Count);

            return wordListClone[newWordIndex];
        }

        public static IEnumerator VignetteLerp(PostProcessVolume volume, float duration, bool active, float finalIntensity)
        {
            if (volume.profile.TryGetSettings(out Vignette effect))
            {
                Debug.Log("vignette found");
            }
            
            effect.intensity.overrideState = true;

            float time = 0f;

            //if vignette is to be activated activate here
            if (active)
                effect.enabled.value = true;

            //change vignette intensity over time
            while (time < duration)
            {
                effect.intensity.value = Mathf.Lerp(effect.intensity.value, finalIntensity, time / duration);

                Debug.Log("Vignette Intensity: " + effect.intensity.value);

                time += Time.deltaTime;

                yield return null;
            }

            //if vignette is to be deactivated deactivate here
            if (!active)
                effect.enabled.value = false;
        }
    }
}

