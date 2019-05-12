using System;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	[TrackClipType(typeof(AudioPlayableAsset))]
	[TrackClipType(typeof(AudioClip))]
	[TrackMediaType(TimelineAsset.MediaType.Audio)]
	[Serializable]
	public class AudioTrack : TrackAsset
	{
		public TimelineClip CreateClip(AudioClip clip)
		{
			TimelineClip result;
			if (clip == null)
			{
				result = null;
			}
			else
			{
				TimelineClip timelineClip = base.CreateDefaultClip();
				AudioPlayableAsset audioPlayableAsset = timelineClip.asset as AudioPlayableAsset;
				if (audioPlayableAsset != null)
				{
					audioPlayableAsset.clip = clip;
				}
				timelineClip.duration = (double)clip.length;
				timelineClip.displayName = clip.name;
				result = timelineClip;
			}
			return result;
		}

		internal override Playable OnCreatePlayableGraph(PlayableGraph graph, GameObject go, IntervalTree<RuntimeElement> tree)
		{
			AudioMixerPlayable audioMixerPlayable = AudioMixerPlayable.Create(graph, base.clips.Length, false);
			for (int i = 0; i < base.clips.Length; i++)
			{
				TimelineClip timelineClip = base.clips[i];
				PlayableAsset playableAsset = timelineClip.asset as PlayableAsset;
				if (!(playableAsset == null))
				{
					float num = 0.1f;
					AudioPlayableAsset audioPlayableAsset = timelineClip.asset as AudioPlayableAsset;
					if (audioPlayableAsset != null)
					{
						num = audioPlayableAsset.bufferingTime;
					}
					Playable playable = playableAsset.CreatePlayable(graph, go);
					if (playable.IsValid<Playable>())
					{
						tree.Add(new ScheduleRuntimeClip(timelineClip, playable, audioMixerPlayable, (double)num, 0.1));
						graph.Connect<Playable, AudioMixerPlayable>(playable, 0, audioMixerPlayable, i);
						playable.SetSpeed(timelineClip.timeScale);
						playable.SetDuration(timelineClip.extrapolatedDuration);
						audioMixerPlayable.SetInputWeight(playable, 1f);
					}
				}
			}
			return audioMixerPlayable;
		}

		internal override void OnCreateClipFromAsset(Object asset, TimelineClip newClip)
		{
			if (asset is AudioClip)
			{
				AudioPlayableAsset audioPlayableAsset = ScriptableObject.CreateInstance<AudioPlayableAsset>();
				audioPlayableAsset.clip = (asset as AudioClip);
				newClip.asset = audioPlayableAsset;
				newClip.duration = audioPlayableAsset.duration;
				newClip.displayName = (asset as AudioClip).name;
			}
		}

		public override IEnumerable<PlayableBinding> outputs
		{
			get
			{
				yield return new PlayableBinding
				{
					sourceObject = this,
					streamName = base.name,
					streamType = DataStreamType.Audio,
					sourceBindingType = typeof(AudioSource)
				};
				yield break;
			}
		}
	}
}
