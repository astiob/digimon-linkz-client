using System;
using System.Collections;
using UnityEngine;

public static class AnimationPlayIgnoreTimeScale
{
	public static IEnumerator Play(this Animation animation, string clipName, bool ignoreTimeScale, Action onComplete = null)
	{
		if (ignoreTimeScale)
		{
			AnimationState currState = animation[clipName];
			bool isPlaying = true;
			float progressTime = 0f;
			float timeAtLastFrame = 0f;
			float timeAtCurrentFrame = 0f;
			bool inReversePlaying = false;
			float deltaTime = 0f;
			animation.Play(clipName);
			timeAtLastFrame = Time.realtimeSinceStartup;
			while (isPlaying)
			{
				timeAtCurrentFrame = Time.realtimeSinceStartup;
				deltaTime = timeAtCurrentFrame - timeAtLastFrame;
				timeAtLastFrame = timeAtCurrentFrame;
				progressTime += deltaTime;
				currState.normalizedTime = ((!inReversePlaying) ? (progressTime / currState.length) : (1f - progressTime / currState.length));
				animation.Sample();
				if (progressTime >= currState.length)
				{
					WrapMode wrapMode = currState.wrapMode;
					switch (wrapMode)
					{
					case WrapMode.Default:
						isPlaying = false;
						break;
					default:
						if (wrapMode != WrapMode.ClampForever)
						{
							isPlaying = false;
						}
						break;
					case WrapMode.Loop:
						progressTime = 0f;
						break;
					case WrapMode.PingPong:
						progressTime = 0f;
						inReversePlaying = !inReversePlaying;
						break;
					}
				}
				yield return new WaitForEndOfFrame();
			}
			yield return null;
			if (onComplete != null)
			{
				onComplete();
			}
		}
		else if (onComplete != null)
		{
			animation.Play(clipName);
		}
		yield break;
	}
}
