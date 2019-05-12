using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	/// <summary>
	///   <para>The animation component is used to play back animations.</para>
	/// </summary>
	public sealed class Animation : Behaviour, IEnumerable
	{
		/// <summary>
		///   <para>The default animation.</para>
		/// </summary>
		public extern AnimationClip clip { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Should the default animation clip (the Animation.clip property) automatically start playing on startup?</para>
		/// </summary>
		public extern bool playAutomatically { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>How should time beyond the playback range of the clip be treated?</para>
		/// </summary>
		public extern WrapMode wrapMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Stops all playing animations that were started with this Animation.</para>
		/// </summary>
		public void Stop()
		{
			Animation.INTERNAL_CALL_Stop(this);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Stop(Animation self);

		/// <summary>
		///   <para>Stops an animation named name.</para>
		/// </summary>
		/// <param name="name"></param>
		public void Stop(string name)
		{
			this.Internal_StopByName(name);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_StopByName(string name);

		/// <summary>
		///   <para>Rewinds the animation named name.</para>
		/// </summary>
		/// <param name="name"></param>
		public void Rewind(string name)
		{
			this.Internal_RewindByName(name);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_RewindByName(string name);

		/// <summary>
		///   <para>Rewinds all animations.</para>
		/// </summary>
		public void Rewind()
		{
			Animation.INTERNAL_CALL_Rewind(this);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Rewind(Animation self);

		/// <summary>
		///   <para>Samples animations at the current state.</para>
		/// </summary>
		public void Sample()
		{
			Animation.INTERNAL_CALL_Sample(this);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Sample(Animation self);

		/// <summary>
		///   <para>Are we playing any animations?</para>
		/// </summary>
		public extern bool isPlaying { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Is the animation named name playing?</para>
		/// </summary>
		/// <param name="name"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsPlaying(string name);

		public AnimationState this[string name]
		{
			get
			{
				return this.GetState(name);
			}
		}

		[ExcludeFromDocs]
		public bool Play()
		{
			PlayMode mode = PlayMode.StopSameLayer;
			return this.Play(mode);
		}

		/// <summary>
		///   <para>Plays an animation without any blending.</para>
		/// </summary>
		/// <param name="mode"></param>
		/// <param name="animation"></param>
		public bool Play([DefaultValue("PlayMode.StopSameLayer")] PlayMode mode)
		{
			return this.PlayDefaultAnimation(mode);
		}

		/// <summary>
		///   <para>Plays an animation without any blending.</para>
		/// </summary>
		/// <param name="mode"></param>
		/// <param name="animation"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool Play(string animation, [DefaultValue("PlayMode.StopSameLayer")] PlayMode mode);

		/// <summary>
		///   <para>Plays an animation without any blending.</para>
		/// </summary>
		/// <param name="mode"></param>
		/// <param name="animation"></param>
		[ExcludeFromDocs]
		public bool Play(string animation)
		{
			PlayMode mode = PlayMode.StopSameLayer;
			return this.Play(animation, mode);
		}

		/// <summary>
		///   <para>Fades the animation with name animation in over a period of time seconds and fades other animations out.</para>
		/// </summary>
		/// <param name="animation"></param>
		/// <param name="fadeLength"></param>
		/// <param name="mode"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void CrossFade(string animation, [DefaultValue("0.3F")] float fadeLength, [DefaultValue("PlayMode.StopSameLayer")] PlayMode mode);

		/// <summary>
		///   <para>Fades the animation with name animation in over a period of time seconds and fades other animations out.</para>
		/// </summary>
		/// <param name="animation"></param>
		/// <param name="fadeLength"></param>
		/// <param name="mode"></param>
		[ExcludeFromDocs]
		public void CrossFade(string animation, float fadeLength)
		{
			PlayMode mode = PlayMode.StopSameLayer;
			this.CrossFade(animation, fadeLength, mode);
		}

		/// <summary>
		///   <para>Fades the animation with name animation in over a period of time seconds and fades other animations out.</para>
		/// </summary>
		/// <param name="animation"></param>
		/// <param name="fadeLength"></param>
		/// <param name="mode"></param>
		[ExcludeFromDocs]
		public void CrossFade(string animation)
		{
			PlayMode mode = PlayMode.StopSameLayer;
			float fadeLength = 0.3f;
			this.CrossFade(animation, fadeLength, mode);
		}

		/// <summary>
		///   <para>Blends the animation named animation towards targetWeight over the next time seconds.</para>
		/// </summary>
		/// <param name="animation"></param>
		/// <param name="targetWeight"></param>
		/// <param name="fadeLength"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Blend(string animation, [DefaultValue("1.0F")] float targetWeight, [DefaultValue("0.3F")] float fadeLength);

		/// <summary>
		///   <para>Blends the animation named animation towards targetWeight over the next time seconds.</para>
		/// </summary>
		/// <param name="animation"></param>
		/// <param name="targetWeight"></param>
		/// <param name="fadeLength"></param>
		[ExcludeFromDocs]
		public void Blend(string animation, float targetWeight)
		{
			float fadeLength = 0.3f;
			this.Blend(animation, targetWeight, fadeLength);
		}

		/// <summary>
		///   <para>Blends the animation named animation towards targetWeight over the next time seconds.</para>
		/// </summary>
		/// <param name="animation"></param>
		/// <param name="targetWeight"></param>
		/// <param name="fadeLength"></param>
		[ExcludeFromDocs]
		public void Blend(string animation)
		{
			float fadeLength = 0.3f;
			float targetWeight = 1f;
			this.Blend(animation, targetWeight, fadeLength);
		}

		/// <summary>
		///   <para>Cross fades an animation after previous animations has finished playing.</para>
		/// </summary>
		/// <param name="animation"></param>
		/// <param name="fadeLength"></param>
		/// <param name="queue"></param>
		/// <param name="mode"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern AnimationState CrossFadeQueued(string animation, [DefaultValue("0.3F")] float fadeLength, [DefaultValue("QueueMode.CompleteOthers")] QueueMode queue, [DefaultValue("PlayMode.StopSameLayer")] PlayMode mode);

		/// <summary>
		///   <para>Cross fades an animation after previous animations has finished playing.</para>
		/// </summary>
		/// <param name="animation"></param>
		/// <param name="fadeLength"></param>
		/// <param name="queue"></param>
		/// <param name="mode"></param>
		[ExcludeFromDocs]
		public AnimationState CrossFadeQueued(string animation, float fadeLength, QueueMode queue)
		{
			PlayMode mode = PlayMode.StopSameLayer;
			return this.CrossFadeQueued(animation, fadeLength, queue, mode);
		}

		/// <summary>
		///   <para>Cross fades an animation after previous animations has finished playing.</para>
		/// </summary>
		/// <param name="animation"></param>
		/// <param name="fadeLength"></param>
		/// <param name="queue"></param>
		/// <param name="mode"></param>
		[ExcludeFromDocs]
		public AnimationState CrossFadeQueued(string animation, float fadeLength)
		{
			PlayMode mode = PlayMode.StopSameLayer;
			QueueMode queue = QueueMode.CompleteOthers;
			return this.CrossFadeQueued(animation, fadeLength, queue, mode);
		}

		/// <summary>
		///   <para>Cross fades an animation after previous animations has finished playing.</para>
		/// </summary>
		/// <param name="animation"></param>
		/// <param name="fadeLength"></param>
		/// <param name="queue"></param>
		/// <param name="mode"></param>
		[ExcludeFromDocs]
		public AnimationState CrossFadeQueued(string animation)
		{
			PlayMode mode = PlayMode.StopSameLayer;
			QueueMode queue = QueueMode.CompleteOthers;
			float fadeLength = 0.3f;
			return this.CrossFadeQueued(animation, fadeLength, queue, mode);
		}

		/// <summary>
		///   <para>Plays an animation after previous animations has finished playing.</para>
		/// </summary>
		/// <param name="animation"></param>
		/// <param name="queue"></param>
		/// <param name="mode"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern AnimationState PlayQueued(string animation, [DefaultValue("QueueMode.CompleteOthers")] QueueMode queue, [DefaultValue("PlayMode.StopSameLayer")] PlayMode mode);

		/// <summary>
		///   <para>Plays an animation after previous animations has finished playing.</para>
		/// </summary>
		/// <param name="animation"></param>
		/// <param name="queue"></param>
		/// <param name="mode"></param>
		[ExcludeFromDocs]
		public AnimationState PlayQueued(string animation, QueueMode queue)
		{
			PlayMode mode = PlayMode.StopSameLayer;
			return this.PlayQueued(animation, queue, mode);
		}

		/// <summary>
		///   <para>Plays an animation after previous animations has finished playing.</para>
		/// </summary>
		/// <param name="animation"></param>
		/// <param name="queue"></param>
		/// <param name="mode"></param>
		[ExcludeFromDocs]
		public AnimationState PlayQueued(string animation)
		{
			PlayMode mode = PlayMode.StopSameLayer;
			QueueMode queue = QueueMode.CompleteOthers;
			return this.PlayQueued(animation, queue, mode);
		}

		/// <summary>
		///   <para>Adds a clip to the animation with name newName.</para>
		/// </summary>
		/// <param name="clip"></param>
		/// <param name="newName"></param>
		public void AddClip(AnimationClip clip, string newName)
		{
			this.AddClip(clip, newName, int.MinValue, int.MaxValue);
		}

		/// <summary>
		///   <para>Adds clip to the only play between firstFrame and lastFrame. The new clip will also be added to the animation with name newName.</para>
		/// </summary>
		/// <param name="addLoopFrame">Should an extra frame be inserted at the end that matches the first frame? Turn this on if you are making a looping animation.</param>
		/// <param name="clip"></param>
		/// <param name="newName"></param>
		/// <param name="firstFrame"></param>
		/// <param name="lastFrame"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void AddClip(AnimationClip clip, string newName, int firstFrame, int lastFrame, [DefaultValue("false")] bool addLoopFrame);

		/// <summary>
		///   <para>Adds clip to the only play between firstFrame and lastFrame. The new clip will also be added to the animation with name newName.</para>
		/// </summary>
		/// <param name="addLoopFrame">Should an extra frame be inserted at the end that matches the first frame? Turn this on if you are making a looping animation.</param>
		/// <param name="clip"></param>
		/// <param name="newName"></param>
		/// <param name="firstFrame"></param>
		/// <param name="lastFrame"></param>
		[ExcludeFromDocs]
		public void AddClip(AnimationClip clip, string newName, int firstFrame, int lastFrame)
		{
			bool addLoopFrame = false;
			this.AddClip(clip, newName, firstFrame, lastFrame, addLoopFrame);
		}

		/// <summary>
		///   <para>Remove clip from the animation list.</para>
		/// </summary>
		/// <param name="clip"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RemoveClip(AnimationClip clip);

		/// <summary>
		///   <para>Remove clip from the animation list.</para>
		/// </summary>
		/// <param name="clipName"></param>
		public void RemoveClip(string clipName)
		{
			this.RemoveClip2(clipName);
		}

		/// <summary>
		///   <para>Get the number of clips currently assigned to this animation.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetClipCount();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void RemoveClip2(string clipName);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool PlayDefaultAnimation(PlayMode mode);

		[Obsolete("use PlayMode instead of AnimationPlayMode.")]
		public bool Play(AnimationPlayMode mode)
		{
			return this.PlayDefaultAnimation((PlayMode)mode);
		}

		[Obsolete("use PlayMode instead of AnimationPlayMode.")]
		public bool Play(string animation, AnimationPlayMode mode)
		{
			return this.Play(animation, (PlayMode)mode);
		}

		public void SyncLayer(int layer)
		{
			Animation.INTERNAL_CALL_SyncLayer(this, layer);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SyncLayer(Animation self, int layer);

		public IEnumerator GetEnumerator()
		{
			return new Animation.Enumerator(this);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern AnimationState GetState(string name);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern AnimationState GetStateAtIndex(int index);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int GetStateCount();

		public AnimationClip GetClip(string name)
		{
			AnimationState state = this.GetState(name);
			if (state)
			{
				return state.clip;
			}
			return null;
		}

		/// <summary>
		///   <para>When turned on, animations will be executed in the physics loop. This is only useful in conjunction with kinematic rigidbodies.</para>
		/// </summary>
		public extern bool animatePhysics { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>When turned on, Unity might stop animating if it thinks that the results of the animation won't be visible to the user.</para>
		/// </summary>
		[Obsolete("Use cullingType instead")]
		public extern bool animateOnlyIfVisible { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Controls culling of this Animation component.</para>
		/// </summary>
		public extern AnimationCullingType cullingType { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>AABB of this Animation animation component in local space.</para>
		/// </summary>
		public Bounds localBounds
		{
			get
			{
				Bounds result;
				this.INTERNAL_get_localBounds(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_localBounds(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_localBounds(out Bounds value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_localBounds(ref Bounds value);

		private sealed class Enumerator : IEnumerator
		{
			private Animation m_Outer;

			private int m_CurrentIndex = -1;

			internal Enumerator(Animation outer)
			{
				this.m_Outer = outer;
			}

			public object Current
			{
				get
				{
					return this.m_Outer.GetStateAtIndex(this.m_CurrentIndex);
				}
			}

			public bool MoveNext()
			{
				int stateCount = this.m_Outer.GetStateCount();
				this.m_CurrentIndex++;
				return this.m_CurrentIndex < stateCount;
			}

			public void Reset()
			{
				this.m_CurrentIndex = -1;
			}
		}
	}
}
