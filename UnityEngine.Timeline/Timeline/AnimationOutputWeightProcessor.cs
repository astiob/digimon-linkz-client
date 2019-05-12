using System;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	internal class AnimationOutputWeightProcessor : ITimelineEvaluateCallback
	{
		private AnimationPlayableOutput m_Output;

		private AnimationLayerMixerPlayable m_LayerMixer;

		private readonly List<AnimationOutputWeightProcessor.WeightInfo> m_Mixers = new List<AnimationOutputWeightProcessor.WeightInfo>();

		public AnimationOutputWeightProcessor(AnimationPlayableOutput output)
		{
			this.m_Output = output;
			this.FindMixers();
		}

		private void FindMixers()
		{
			this.m_Mixers.Clear();
			this.m_LayerMixer = AnimationLayerMixerPlayable.Null;
			Playable sourcePlayable = this.m_Output.GetSourcePlayable<AnimationPlayableOutput>();
			int sourceInputPort = this.m_Output.GetSourceInputPort<AnimationPlayableOutput>();
			if (sourcePlayable.IsValid<Playable>() && sourceInputPort >= 0 && sourceInputPort < sourcePlayable.GetInputCount<Playable>())
			{
				Playable input = sourcePlayable.GetInput(sourceInputPort).GetInput(0);
				if (input.IsValid<Playable>() && input.IsPlayableOfType<AnimationLayerMixerPlayable>())
				{
					this.m_LayerMixer = (AnimationLayerMixerPlayable)input;
					int inputCount = this.m_LayerMixer.GetInputCount<AnimationLayerMixerPlayable>();
					for (int i = 0; i < inputCount; i++)
					{
						this.FindMixers(this.m_LayerMixer, i, this.m_LayerMixer.GetInput(i));
					}
				}
			}
		}

		private void FindMixers(Playable parent, int port, Playable node)
		{
			if (node.IsValid<Playable>())
			{
				Type playableType = node.GetPlayableType();
				if (playableType == typeof(AnimationMixerPlayable) || playableType == typeof(AnimationLayerMixerPlayable))
				{
					int inputCount = node.GetInputCount<Playable>();
					for (int i = 0; i < inputCount; i++)
					{
						this.FindMixers(node, i, node.GetInput(i));
					}
					AnimationOutputWeightProcessor.WeightInfo item = new AnimationOutputWeightProcessor.WeightInfo
					{
						parentMixer = parent,
						mixer = node,
						port = port,
						modulate = (playableType == typeof(AnimationLayerMixerPlayable))
					};
					this.m_Mixers.Add(item);
				}
				else
				{
					int inputCount2 = node.GetInputCount<Playable>();
					for (int j = 0; j < inputCount2; j++)
					{
						this.FindMixers(parent, port, node.GetInput(j));
					}
				}
			}
		}

		public void Evaluate()
		{
			for (int i = 0; i < this.m_Mixers.Count; i++)
			{
				AnimationOutputWeightProcessor.WeightInfo weightInfo = this.m_Mixers[i];
				float num = (!weightInfo.modulate) ? 1f : weightInfo.parentMixer.GetInputWeight(weightInfo.port);
				weightInfo.parentMixer.SetInputWeight(weightInfo.port, num * WeightUtility.NormalizeMixer(weightInfo.mixer));
			}
			this.m_Output.SetWeight(WeightUtility.NormalizeMixer(this.m_LayerMixer));
		}

		private struct WeightInfo
		{
			public Playable mixer;

			public Playable parentMixer;

			public int port;

			public bool modulate;
		}
	}
}
