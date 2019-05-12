using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Class for ProceduralMaterial handling.</para>
	/// </summary>
	public sealed class ProceduralMaterial : Material
	{
		internal ProceduralMaterial() : base(null)
		{
		}

		/// <summary>
		///   <para>Get an array of descriptions of all the ProceduralProperties this ProceduralMaterial has.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern ProceduralPropertyDescription[] GetProceduralPropertyDescriptions();

		/// <summary>
		///   <para>Checks if the ProceduralMaterial has a ProceduralProperty of a given name.</para>
		/// </summary>
		/// <param name="inputName"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasProceduralProperty(string inputName);

		/// <summary>
		///   <para>Get a named Procedural boolean property.</para>
		/// </summary>
		/// <param name="inputName"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetProceduralBoolean(string inputName);

		/// <summary>
		///   <para>Checks if a given ProceduralProperty is visible according to the values of this ProceduralMaterial's other ProceduralProperties and to the ProceduralProperty's visibleIf expression.</para>
		/// </summary>
		/// <param name="inputName">The name of the ProceduralProperty whose visibility is evaluated.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsProceduralPropertyVisible(string inputName);

		/// <summary>
		///   <para>Set a named Procedural boolean property.</para>
		/// </summary>
		/// <param name="inputName"></param>
		/// <param name="value"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetProceduralBoolean(string inputName, bool value);

		/// <summary>
		///   <para>Get a named Procedural float property.</para>
		/// </summary>
		/// <param name="inputName"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetProceduralFloat(string inputName);

		/// <summary>
		///   <para>Set a named Procedural float property.</para>
		/// </summary>
		/// <param name="inputName"></param>
		/// <param name="value"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetProceduralFloat(string inputName, float value);

		/// <summary>
		///   <para>Get a named Procedural vector property.</para>
		/// </summary>
		/// <param name="inputName"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Vector4 GetProceduralVector(string inputName);

		/// <summary>
		///   <para>Set a named Procedural vector property.</para>
		/// </summary>
		/// <param name="inputName"></param>
		/// <param name="value"></param>
		public void SetProceduralVector(string inputName, Vector4 value)
		{
			ProceduralMaterial.INTERNAL_CALL_SetProceduralVector(this, inputName, ref value);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetProceduralVector(ProceduralMaterial self, string inputName, ref Vector4 value);

		/// <summary>
		///   <para>Get a named Procedural color property.</para>
		/// </summary>
		/// <param name="inputName"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Color GetProceduralColor(string inputName);

		/// <summary>
		///   <para>Set a named Procedural color property.</para>
		/// </summary>
		/// <param name="inputName"></param>
		/// <param name="value"></param>
		public void SetProceduralColor(string inputName, Color value)
		{
			ProceduralMaterial.INTERNAL_CALL_SetProceduralColor(this, inputName, ref value);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetProceduralColor(ProceduralMaterial self, string inputName, ref Color value);

		/// <summary>
		///   <para>Get a named Procedural enum property.</para>
		/// </summary>
		/// <param name="inputName"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetProceduralEnum(string inputName);

		/// <summary>
		///   <para>Set a named Procedural enum property.</para>
		/// </summary>
		/// <param name="inputName"></param>
		/// <param name="value"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetProceduralEnum(string inputName, int value);

		/// <summary>
		///   <para>Get a named Procedural texture property.</para>
		/// </summary>
		/// <param name="inputName"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Texture2D GetProceduralTexture(string inputName);

		/// <summary>
		///   <para>Set a named Procedural texture property.</para>
		/// </summary>
		/// <param name="inputName"></param>
		/// <param name="value"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetProceduralTexture(string inputName, Texture2D value);

		/// <summary>
		///   <para>Checks if a named ProceduralProperty is cached for efficient runtime tweaking.</para>
		/// </summary>
		/// <param name="inputName"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsProceduralPropertyCached(string inputName);

		/// <summary>
		///   <para>Specifies if a named ProceduralProperty should be cached for efficient runtime tweaking.</para>
		/// </summary>
		/// <param name="inputName"></param>
		/// <param name="value"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void CacheProceduralProperty(string inputName, bool value);

		/// <summary>
		///   <para>Clear the Procedural cache.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ClearCache();

		/// <summary>
		///   <para>Set or get the Procedural cache budget.</para>
		/// </summary>
		public extern ProceduralCacheSize cacheSize { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Set or get the update rate in millisecond of the animated substance.</para>
		/// </summary>
		public extern int animationUpdateRate { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Triggers an asynchronous rebuild of this ProceduralMaterial's dirty textures.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RebuildTextures();

		/// <summary>
		///   <para>Triggers an immediate (synchronous) rebuild of this ProceduralMaterial's dirty textures.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RebuildTexturesImmediately();

		/// <summary>
		///   <para>Check if the ProceduralTextures from this ProceduralMaterial are currently being rebuilt.</para>
		/// </summary>
		public extern bool isProcessing { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Discard all the queued ProceduralMaterial rendering operations that have not started yet.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StopRebuilds();

		/// <summary>
		///   <para>Indicates whether cached data is available for this ProceduralMaterial's textures (only relevant for Cache and DoNothingAndCache loading behaviors).</para>
		/// </summary>
		public extern bool isCachedDataAvailable { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Should the ProceduralMaterial be generated at load time?</para>
		/// </summary>
		public extern bool isLoadTimeGenerated { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Get ProceduralMaterial loading behavior.</para>
		/// </summary>
		public extern ProceduralLoadingBehavior loadingBehavior { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Check if ProceduralMaterials are supported on the current platform.</para>
		/// </summary>
		public static extern bool isSupported { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Used to specify the Substance engine CPU usage.</para>
		/// </summary>
		public static extern ProceduralProcessorUsage substanceProcessorUsage { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Set or get an XML string of "input/value" pairs (setting the preset rebuilds the textures).</para>
		/// </summary>
		public extern string preset { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Get generated textures.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Texture[] GetGeneratedTextures();

		/// <summary>
		///   <para>This allows to get a reference to a ProceduralTexture generated by a ProceduralMaterial using its name.</para>
		/// </summary>
		/// <param name="textureName">The name of the ProceduralTexture to get.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern ProceduralTexture GetGeneratedTexture(string textureName);

		/// <summary>
		///   <para>Set or get the "Readable" flag for a ProceduralMaterial.</para>
		/// </summary>
		public extern bool isReadable { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
