using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	/// <summary>
	///   <para>The AnimationState gives full control over animation blending.</para>
	/// </summary>
	public sealed class AnimationState : TrackedReference
	{
		/// <summary>
		///   <para>Enables / disables the animation.</para>
		/// </summary>
		public extern bool enabled { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The weight of animation.</para>
		/// </summary>
		public extern float weight { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Wrapping mode of the animation.</para>
		/// </summary>
		public extern WrapMode wrapMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The current time of the animation.</para>
		/// </summary>
		public extern float time { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The normalized time of the animation.</para>
		/// </summary>
		public extern float normalizedTime { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The playback speed of the animation. 1 is normal playback speed.</para>
		/// </summary>
		public extern float speed { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The normalized playback speed.</para>
		/// </summary>
		public extern float normalizedSpeed { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The length of the animation clip in seconds.</para>
		/// </summary>
		public extern float length { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern int layer { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The clip that is being played by this animation state.</para>
		/// </summary>
		public extern AnimationClip clip { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Adds a transform which should be animated. This allows you to reduce the number of animations you have to create.</para>
		/// </summary>
		/// <param name="mix"></param>
		/// <param name="recursive"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void AddMixingTransform(Transform mix, [DefaultValue("true")] bool recursive);

		/// <summary>
		///   <para>Adds a transform which should be animated. This allows you to reduce the number of animations you have to create.</para>
		/// </summary>
		/// <param name="mix"></param>
		/// <param name="recursive"></param>
		[ExcludeFromDocs]
		public void AddMixingTransform(Transform mix)
		{
			bool recursive = true;
			this.AddMixingTransform(mix, recursive);
		}

		/// <summary>
		///   <para>Removes a transform which should be animated.</para>
		/// </summary>
		/// <param name="mix"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RemoveMixingTransform(Transform mix);

		/// <summary>
		///   <para>The name of the animation.</para>
		/// </summary>
		public extern string name { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Which blend mode should be used?</para>
		/// </summary>
		public extern AnimationBlendMode blendMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
