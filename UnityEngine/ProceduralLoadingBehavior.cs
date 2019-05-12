using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>ProceduralMaterial loading behavior.</para>
	/// </summary>
	public enum ProceduralLoadingBehavior
	{
		/// <summary>
		///   <para>Do not generate the textures. RebuildTextures() or RebuildTexturesImmediately() must be called to generate the textures.</para>
		/// </summary>
		DoNothing,
		/// <summary>
		///   <para>Generate the textures when loading to favor application's size (default on supported platform).</para>
		/// </summary>
		Generate,
		/// <summary>
		///   <para>Bake the textures to speed up loading and keep the ProceduralMaterial data so that it can still be tweaked and regenerated later on.</para>
		/// </summary>
		BakeAndKeep,
		/// <summary>
		///   <para>Bake the textures to speed up loading and discard the ProceduralMaterial data (default on unsupported platform).</para>
		/// </summary>
		BakeAndDiscard,
		/// <summary>
		///   <para>Generate the textures when loading and cache them to diskflash to speed up subsequent gameapplication startups.</para>
		/// </summary>
		Cache,
		/// <summary>
		///   <para>Do not generate the textures. RebuildTextures() or RebuildTexturesImmediately() must be called to generate the textures. After the textures have been generrated for the first time, they are cached to diskflash to speed up subsequent gameapplication startups.</para>
		/// </summary>
		DoNothingAndCache
	}
}
