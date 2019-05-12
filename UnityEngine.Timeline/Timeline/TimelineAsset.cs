using System;
using System.Collections.Generic;
using UnityEngine.Playables;

namespace UnityEngine.Timeline
{
	[Serializable]
	public class TimelineAsset : PlayableAsset, ISerializationCallbackReceiver, ITimelineClipAsset, IPropertyPreview
	{
		[HideInInspector]
		[SerializeField]
		private int m_NextId;

		[HideInInspector]
		[SerializeField]
		private List<ScriptableObject> m_Tracks;

		[HideInInspector]
		[SerializeField]
		private double m_FixedDuration;

		[HideInInspector]
		[NonSerialized]
		private TrackAsset[] m_CacheOutputTracks;

		[HideInInspector]
		[NonSerialized]
		private List<TrackAsset> m_CacheRootTracks;

		[HideInInspector]
		[NonSerialized]
		private List<TrackAsset> m_CacheFlattenedTracks;

		[HideInInspector]
		[SerializeField]
		private TimelineAsset.EditorSettings m_EditorSettings = new TimelineAsset.EditorSettings();

		[SerializeField]
		private TimelineAsset.DurationMode m_DurationMode;

		public TimelineAsset.EditorSettings editorSettings
		{
			get
			{
				return this.m_EditorSettings;
			}
		}

		public override double duration
		{
			get
			{
				double result;
				if (this.m_DurationMode == TimelineAsset.DurationMode.BasedOnClips)
				{
					result = this.CalculateDuration();
				}
				else
				{
					result = this.m_FixedDuration;
				}
				return result;
			}
		}

		public double fixedDuration
		{
			get
			{
				DiscreteTime lhs = (DiscreteTime)this.m_FixedDuration;
				double result;
				if (lhs <= 0)
				{
					result = 0.0;
				}
				else
				{
					result = (double)lhs.OneTickBefore();
				}
				return result;
			}
			set
			{
				this.m_FixedDuration = Math.Max(0.0, value);
			}
		}

		public TimelineAsset.DurationMode durationMode
		{
			get
			{
				return this.m_DurationMode;
			}
			set
			{
				this.m_DurationMode = value;
			}
		}

		public override IEnumerable<PlayableBinding> outputs
		{
			get
			{
				foreach (TrackAsset outputTracks in this.GetOutputTracks())
				{
					foreach (PlayableBinding output in outputTracks.outputs)
					{
						yield return output;
					}
				}
				yield break;
			}
		}

		public ClipCaps clipCaps
		{
			get
			{
				ClipCaps clipCaps = ClipCaps.All;
				foreach (TrackAsset trackAsset in this.GetRootTracks())
				{
					foreach (TimelineClip timelineClip in trackAsset.clips)
					{
						clipCaps &= timelineClip.clipCaps;
					}
				}
				return clipCaps;
			}
		}

		public int outputTrackCount
		{
			get
			{
				this.GetOutputTracks();
				return this.m_CacheOutputTracks.Length;
			}
		}

		public int rootTrackCount
		{
			get
			{
				this.UpdateRootTrackCache();
				return this.m_CacheRootTracks.Count;
			}
		}

		private void OnValidate()
		{
			this.editorSettings.fps = TimelineAsset.GetValidFramerate(this.editorSettings.fps);
		}

		private static float GetValidFramerate(float framerate)
		{
			return Mathf.Clamp(framerate, TimelineAsset.EditorSettings.kMinFps, TimelineAsset.EditorSettings.kMaxFps);
		}

		public TrackAsset GetRootTrack(int index)
		{
			this.UpdateRootTrackCache();
			return this.m_CacheRootTracks[index];
		}

		public IEnumerable<TrackAsset> GetRootTracks()
		{
			this.UpdateRootTrackCache();
			return this.m_CacheRootTracks;
		}

		public TrackAsset GetOutputTrack(int index)
		{
			this.UpdateOutputTrackCache();
			return this.m_CacheOutputTracks[index];
		}

		public IEnumerable<TrackAsset> GetOutputTracks()
		{
			this.UpdateOutputTrackCache();
			return this.m_CacheOutputTracks;
		}

		private void UpdateRootTrackCache()
		{
			if (this.m_CacheRootTracks == null)
			{
				if (this.m_Tracks == null)
				{
					this.m_CacheRootTracks = new List<TrackAsset>();
				}
				else
				{
					this.m_CacheRootTracks = new List<TrackAsset>(this.m_Tracks.Count);
					for (int i = 0; i < this.m_Tracks.Count; i++)
					{
						TrackAsset trackAsset = this.m_Tracks[i] as TrackAsset;
						if (trackAsset != null)
						{
							this.m_CacheRootTracks.Add(trackAsset);
						}
					}
				}
			}
		}

		private void UpdateOutputTrackCache()
		{
			if (this.m_CacheOutputTracks == null)
			{
				List<TrackAsset> list = new List<TrackAsset>();
				foreach (TrackAsset trackAsset in this.flattenedTracks)
				{
					if (trackAsset != null && trackAsset.mediaType != TimelineAsset.MediaType.Group && !trackAsset.isSubTrack)
					{
						list.Add(trackAsset);
					}
				}
				this.m_CacheOutputTracks = list.ToArray();
			}
		}

		internal IEnumerable<TrackAsset> flattenedTracks
		{
			get
			{
				if (this.m_CacheFlattenedTracks == null)
				{
					this.m_CacheFlattenedTracks = new List<TrackAsset>(this.m_Tracks.Count * 2);
					this.UpdateRootTrackCache();
					this.m_CacheFlattenedTracks.AddRange(this.m_CacheRootTracks);
					for (int i = 0; i < this.m_CacheRootTracks.Count; i++)
					{
						TimelineAsset.AddSubTracksRecursive(this.m_CacheRootTracks[i], ref this.m_CacheFlattenedTracks);
					}
				}
				return this.m_CacheFlattenedTracks;
			}
		}

		internal List<ScriptableObject> trackObjects
		{
			get
			{
				return this.m_Tracks;
			}
		}

		internal void AddTrackInternal(TrackAsset track)
		{
			this.m_Tracks.Add(track);
			track.parent = this;
			this.Invalidate();
		}

		internal void RemoveTrack(TrackAsset track)
		{
			this.m_Tracks.Remove(track);
			this.Invalidate();
			TrackAsset trackAsset = track.parent as TrackAsset;
			if (trackAsset != null)
			{
				trackAsset.RemoveSubTrack(track);
			}
		}

		internal int GenerateNewId()
		{
			this.m_NextId++;
			return base.GetInstanceID().GetHashCode().CombineHash(this.m_NextId.GetHashCode());
		}

		public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
		{
			bool autoRebalance = false;
			bool createOutputs = graph.GetPlayableCount() == 0;
			ScriptPlayable<TimelinePlayable> playable = TimelinePlayable.Create(graph, this.GetOutputTracks(), go, autoRebalance, createOutputs);
			return (!playable.IsValid<ScriptPlayable<TimelinePlayable>>()) ? Playable.Null : playable;
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			this.Invalidate();
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		private void __internalAwake()
		{
			if (this.m_Tracks == null)
			{
				this.m_Tracks = new List<ScriptableObject>();
			}
			for (int i = this.m_Tracks.Count - 1; i >= 0; i--)
			{
				TrackAsset trackAsset = this.m_Tracks[i] as TrackAsset;
				if (trackAsset != null)
				{
					trackAsset.parent = this;
				}
			}
		}

		public void GatherProperties(PlayableDirector director, IPropertyCollector driver)
		{
			IEnumerable<TrackAsset> outputTracks = this.GetOutputTracks();
			foreach (TrackAsset trackAsset in outputTracks)
			{
				trackAsset.GatherProperties(director, driver);
			}
		}

		internal void Invalidate()
		{
			this.m_CacheRootTracks = null;
			this.m_CacheOutputTracks = null;
			this.m_CacheFlattenedTracks = null;
		}

		private double CalculateDuration()
		{
			IEnumerable<TrackAsset> flattenedTracks = this.flattenedTracks;
			DiscreteTime lhs = new DiscreteTime(0);
			foreach (TrackAsset trackAsset in flattenedTracks)
			{
				if (!trackAsset.muted)
				{
					lhs = DiscreteTime.Max(lhs, (DiscreteTime)trackAsset.end);
				}
			}
			double result;
			if (lhs <= 0)
			{
				result = 0.0;
			}
			else
			{
				result = (double)lhs.OneTickBefore();
			}
			return result;
		}

		private static void AddSubTracksRecursive(TrackAsset track, ref List<TrackAsset> allTracks)
		{
			if (!(track == null))
			{
				allTracks.AddRange(track.GetChildTracks());
				foreach (TrackAsset track2 in track.GetChildTracks())
				{
					TimelineAsset.AddSubTracksRecursive(track2, ref allTracks);
				}
			}
		}

		public TrackAsset CreateTrack(Type type, TrackAsset parent, string name)
		{
			if (parent != null && parent.timelineAsset != this)
			{
				throw new InvalidOperationException("Addtrack cannot parent to a track not in the Timeline");
			}
			if (!typeof(TrackAsset).IsAssignableFrom(type))
			{
				throw new InvalidOperationException("Supplied type must be a track asset");
			}
			if (parent != null)
			{
				if (!TimelineCreateUtilities.ValidateParentTrack(parent, type))
				{
					throw new InvalidOperationException("Cannot assign a child of type " + type.Name + "to a parent of type " + parent.GetType().Name);
				}
			}
			PlayableAsset masterAsset = (!(parent != null)) ? this : parent;
			string text = name;
			if (string.IsNullOrEmpty(text))
			{
				text = type.Name;
			}
			string text2;
			if (parent != null)
			{
				text2 = TimelineCreateUtilities.GenerateUniqueActorName(parent.subTracksObjects, text);
			}
			else
			{
				text2 = TimelineCreateUtilities.GenerateUniqueActorName(this.trackObjects, text);
			}
			TrackAsset trackAsset = this.AllocateTrack(parent, text2, type);
			if (trackAsset != null)
			{
				trackAsset.name = text2;
				TimelineCreateUtilities.SaveAssetIntoObject(trackAsset, masterAsset);
			}
			return trackAsset;
		}

		public T CreateTrack<T>(TrackAsset parent, string name) where T : TrackAsset, new()
		{
			return (T)((object)this.CreateTrack(typeof(T), parent, name));
		}

		public bool DeleteClip(TimelineClip clip)
		{
			bool result;
			if (clip == null || clip.parentTrack == null)
			{
				result = false;
			}
			else if (this != clip.parentTrack.timelineAsset)
			{
				Debug.LogError("Cannot delete a clip from this timeline");
				result = false;
			}
			else
			{
				if (clip.curves != null)
				{
					TimelineUndo.PushDestroyUndo(this, clip.parentTrack, clip.curves, "Delete Curves");
				}
				if (clip.asset != null)
				{
					this.DeleteRecordedAnimation(clip);
					TimelineUndo.PushDestroyUndo(this, clip.parentTrack, clip.asset, "Delete Clip Asset");
				}
				TrackAsset parentTrack = clip.parentTrack;
				parentTrack.RemoveClip(clip);
				parentTrack.CalculateExtrapolationTimes();
				result = true;
			}
			return result;
		}

		public bool DeleteTrack(TrackAsset track)
		{
			bool result;
			if (track.timelineAsset != this)
			{
				result = false;
			}
			else
			{
				TrackAsset x = track.parent as TrackAsset;
				if (x != null)
				{
				}
				IEnumerable<TrackAsset> childTracks = track.GetChildTracks();
				foreach (TrackAsset track2 in childTracks)
				{
					this.DeleteTrack(track2);
				}
				this.DeleteRecordingClip(track);
				List<TimelineClip> list = new List<TimelineClip>(track.clips);
				foreach (TimelineClip clip in list)
				{
					this.DeleteClip(clip);
				}
				this.RemoveTrack(track);
				TimelineUndo.PushDestroyUndo(this, this, track, "Delete Track");
				result = true;
			}
			return result;
		}

		internal TrackAsset AllocateTrack(TrackAsset trackAssetParent, string trackName, Type trackType)
		{
			if (trackAssetParent != null && trackAssetParent.timelineAsset != this)
			{
				throw new InvalidOperationException("Addtrack cannot parent to a track not in the Timeline");
			}
			if (!typeof(TrackAsset).IsAssignableFrom(trackType))
			{
				throw new InvalidOperationException("Supplied type must be a track asset");
			}
			TrackAsset trackAsset = (TrackAsset)ScriptableObject.CreateInstance(trackType);
			trackAsset.name = trackName;
			if (trackAssetParent != null)
			{
				trackAssetParent.AddChild(trackAsset);
			}
			else
			{
				this.AddTrackInternal(trackAsset);
			}
			return trackAsset;
		}

		private void DeleteRecordingClip(TrackAsset track)
		{
			AnimationTrack animationTrack = track as AnimationTrack;
			if (!(animationTrack == null) && !(animationTrack.animClip == null))
			{
				TimelineUndo.PushDestroyUndo(this, track, animationTrack.animClip, "Delete Track");
			}
		}

		private void DeleteRecordedAnimation(TimelineClip clip)
		{
			if (clip != null)
			{
				if (clip.curves != null)
				{
					TimelineUndo.PushDestroyUndo(this, clip.parentTrack, clip.curves, "Delete Parameters");
				}
				if (clip.recordable)
				{
					AnimationPlayableAsset animationPlayableAsset = clip.asset as AnimationPlayableAsset;
					if (!(animationPlayableAsset == null) && !(animationPlayableAsset.clip == null))
					{
						TimelineUndo.PushDestroyUndo(this, animationPlayableAsset, animationPlayableAsset.clip, "Delete Recording");
					}
				}
			}
		}

		public enum MediaType
		{
			Animation,
			Audio,
			Texture,
			[Obsolete("Use Texture MediaType instead. (UnityUpgradable) -> UnityEngine.Timeline.TimelineAsset/MediaType.Texture", false)]
			Video = 2,
			Script,
			Hybrid,
			Group
		}

		public enum DurationMode
		{
			BasedOnClips,
			FixedLength
		}

		[Serializable]
		public class EditorSettings
		{
			internal static readonly float kMinFps = (float)TimeUtility.kFrameRateEpsilon;

			internal static readonly float kMaxFps = 1000f;

			internal static readonly float kDefaultFps = 60f;

			[HideInInspector]
			[SerializeField]
			private float m_Framerate = TimelineAsset.EditorSettings.kDefaultFps;

			public float fps
			{
				get
				{
					return this.m_Framerate;
				}
				set
				{
					this.m_Framerate = TimelineAsset.GetValidFramerate(value);
				}
			}
		}
	}
}
