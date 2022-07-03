using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

namespace wOUShLab
{
	public sealed class BlurRenderer : PostProcessEffectRenderer<Blur>
	{
		public override void Render(PostProcessRenderContext context)
		{
			if (context.camera.name == "SceneCamera" || context.camera.name == "Preview Scene Camera")
			{
				return;
			}
			var sheet = context.propertySheets.Get(Shader.Find("Hidden/wOUShLab/Blur"));
			sheet.properties.SetFloat("_BlurAmount", settings.amount);
			context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0, RenderBufferLoadAction.Load);
		}
	}
}

