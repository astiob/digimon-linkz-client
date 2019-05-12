using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
	public struct MaterialReference
	{
		public int index;

		public TMP_FontAsset fontAsset;

		public TMP_SpriteAsset spriteAsset;

		public Material material;

		public bool isDefaultMaterial;

		public bool isFallbackMaterial;

		public Material fallbackMaterial;

		public float padding;

		public int referenceCount;

		public MaterialReference(int index, TMP_FontAsset fontAsset, TMP_SpriteAsset spriteAsset, Material material, float padding)
		{
			this.index = index;
			this.fontAsset = fontAsset;
			this.spriteAsset = spriteAsset;
			this.material = material;
			this.isDefaultMaterial = (material.GetInstanceID() == fontAsset.material.GetInstanceID());
			this.isFallbackMaterial = false;
			this.fallbackMaterial = null;
			this.padding = padding;
			this.referenceCount = 0;
		}

		public static bool Contains(MaterialReference[] materialReferences, TMP_FontAsset fontAsset)
		{
			int instanceID = fontAsset.GetInstanceID();
			int num = 0;
			while (num < materialReferences.Length && materialReferences[num].fontAsset != null)
			{
				if (materialReferences[num].fontAsset.GetInstanceID() == instanceID)
				{
					return true;
				}
				num++;
			}
			return false;
		}

		public static int AddMaterialReference(Material material, TMP_FontAsset fontAsset, MaterialReference[] materialReferences, Dictionary<int, int> materialReferenceIndexLookup)
		{
			int instanceID = material.GetInstanceID();
			int num = 0;
			if (materialReferenceIndexLookup.TryGetValue(instanceID, out num))
			{
				return num;
			}
			num = materialReferenceIndexLookup.Count;
			materialReferenceIndexLookup[instanceID] = num;
			materialReferences[num].index = num;
			materialReferences[num].fontAsset = fontAsset;
			materialReferences[num].spriteAsset = null;
			materialReferences[num].material = material;
			materialReferences[num].isDefaultMaterial = (instanceID == fontAsset.material.GetInstanceID());
			materialReferences[num].referenceCount = 0;
			return num;
		}

		public static int AddMaterialReference(Material material, TMP_SpriteAsset spriteAsset, MaterialReference[] materialReferences, Dictionary<int, int> materialReferenceIndexLookup)
		{
			int instanceID = material.GetInstanceID();
			int num = 0;
			if (materialReferenceIndexLookup.TryGetValue(instanceID, out num))
			{
				return num;
			}
			num = materialReferenceIndexLookup.Count;
			materialReferenceIndexLookup[instanceID] = num;
			materialReferences[num].index = num;
			materialReferences[num].fontAsset = materialReferences[0].fontAsset;
			materialReferences[num].spriteAsset = spriteAsset;
			materialReferences[num].material = material;
			materialReferences[num].isDefaultMaterial = true;
			materialReferences[num].referenceCount = 0;
			return num;
		}
	}
}
