using System;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	internal static class WeightUtility
	{
		public static float NormalizeMixer(Playable mixer)
		{
			float result;
			if (!mixer.IsValid<Playable>())
			{
				result = 0f;
			}
			else
			{
				int inputCount = mixer.GetInputCount<Playable>();
				float num = 0f;
				for (int i = 0; i < inputCount; i++)
				{
					num += mixer.GetInputWeight(i);
				}
				if (num > Mathf.Epsilon && num < 1f)
				{
					for (int j = 0; j < inputCount; j++)
					{
						mixer.SetInputWeight(j, mixer.GetInputWeight(j) / num);
					}
				}
				result = Mathf.Clamp01(num);
			}
			return result;
		}
	}
}
