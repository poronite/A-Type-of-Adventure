using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

namespace wOUShLab
{
    [UnityEngine.Rendering.PostProcessing.PostProcess(typeof(BlurRenderer), PostProcessEvent.AfterStack,
        "Custom/Blur")]
    public sealed class Blur : PostProcessEffectSettings
    {
        [Range(0f, 100f)] public IntParameter amount = new IntParameter { value = 30 };

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            return enabled.value;
        }
    }
}

