using Monster;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cutscene
{
	public static class CutsceneCommon
	{
		private static void SetSubMeshRendering(SkinnedMeshRenderer skinMesh, MeshTopology meshTopology)
		{
			for (int i = 0; i < skinMesh.materials.Length; i++)
			{
				skinMesh.sharedMesh.SetIndices(skinMesh.sharedMesh.GetIndices(i), meshTopology, i);
			}
		}

		public static GameObject LoadMonsterModel(Transform parentTransform, string modelId)
		{
			GameObject gameObject = null;
			string filePath = MonsterObject.GetFilePath(modelId);
			GameObject gameObject2 = AssetDataMng.Instance().LoadObject(filePath, null, true) as GameObject;
			if (null != gameObject2)
			{
				gameObject = UnityEngine.Object.Instantiate<GameObject>(gameObject2);
				Transform transform = gameObject.transform;
				transform.parent = parentTransform;
			}
			return gameObject;
		}

		public static void InitializeMonsterPosition(GameObject monster, string growStep)
		{
			if (MonsterGrowStepData.IsGardenDigimonScope(growStep))
			{
				monster.transform.localPosition = Vector3.zero;
			}
			else if (MonsterGrowStepData.IsGrowingScope(growStep))
			{
				monster.transform.localPosition = new Vector3(0f, 0.3f, 0f);
			}
			else
			{
				monster.transform.localPosition = new Vector3(0f, 0.5f, 0f);
			}
		}

		public static void SetBillBoardCamera(GameObject monster, Camera camera)
		{
			BillBoardController[] componentsInChildren = monster.GetComponentsInChildren<BillBoardController>();
			if (componentsInChildren != null)
			{
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].SetUp(camera);
				}
			}
		}

		public static void SetWireFrameRendering(GameObject modelObject)
		{
			List<SkinnedMeshRenderer> compoSMR = CommonRender3DRT.GetCompoSMR(modelObject);
			if (compoSMR != null)
			{
				foreach (SkinnedMeshRenderer skinMesh in compoSMR)
				{
					CutsceneCommon.SetSubMeshRendering(skinMesh, MeshTopology.Lines);
				}
			}
		}

		public static List<Material[]> SetWireFrameRendering(GameObject modelObject, Material wireFrameMaterial)
		{
			List<Material[]> list = new List<Material[]>();
			List<SkinnedMeshRenderer> compoSMR = CommonRender3DRT.GetCompoSMR(modelObject);
			if (compoSMR != null)
			{
				foreach (SkinnedMeshRenderer skinnedMeshRenderer in compoSMR)
				{
					list.Add(skinnedMeshRenderer.materials);
					Material[] array = new Material[skinnedMeshRenderer.materials.Length];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = wireFrameMaterial;
					}
					skinnedMeshRenderer.materials = array;
					CutsceneCommon.SetSubMeshRendering(skinnedMeshRenderer, MeshTopology.Lines);
				}
			}
			return list;
		}

		public static void ResetRendering(GameObject modelObject, List<Material[]> materialsList)
		{
			List<SkinnedMeshRenderer> compoSMR = CommonRender3DRT.GetCompoSMR(modelObject);
			if (compoSMR != null && compoSMR.Count == materialsList.Count)
			{
				for (int i = 0; i < compoSMR.Count; i++)
				{
					compoSMR[i].materials = materialsList[i];
					CutsceneCommon.SetSubMeshRendering(compoSMR[i], MeshTopology.Triangles);
				}
			}
		}

		public static void ResetRendering(GameObject modelObject)
		{
			List<SkinnedMeshRenderer> compoSMR = CommonRender3DRT.GetCompoSMR(modelObject);
			if (compoSMR != null)
			{
				for (int i = 0; i < compoSMR.Count; i++)
				{
					CutsceneCommon.SetSubMeshRendering(compoSMR[i], MeshTopology.Triangles);
				}
			}
		}

		public static void ChangeMaterial(GameObject modelObject, Material changeMaterial)
		{
			List<SkinnedMeshRenderer> compoSMR = CommonRender3DRT.GetCompoSMR(modelObject);
			if (compoSMR != null)
			{
				foreach (SkinnedMeshRenderer skinnedMeshRenderer in compoSMR)
				{
					Material[] materials = skinnedMeshRenderer.materials;
					for (int i = 0; i < materials.Length; i++)
					{
						materials[i] = changeMaterial;
					}
					skinnedMeshRenderer.materials = materials;
				}
			}
		}

		public static void ChangeMaterial(GameObject modelObject, List<Material[]> materialsList)
		{
			List<SkinnedMeshRenderer> compoSMR = CommonRender3DRT.GetCompoSMR(modelObject);
			if (compoSMR != null && compoSMR.Count == materialsList.Count)
			{
				for (int i = 0; i < compoSMR.Count; i++)
				{
					compoSMR[i].materials = materialsList[i];
				}
			}
		}

		public static List<Material[]> GetMaterial(GameObject modelObject)
		{
			List<Material[]> list = new List<Material[]>();
			List<SkinnedMeshRenderer> compoSMR = CommonRender3DRT.GetCompoSMR(modelObject);
			if (compoSMR != null)
			{
				foreach (SkinnedMeshRenderer skinnedMeshRenderer in compoSMR)
				{
					list.Add(skinnedMeshRenderer.materials);
				}
			}
			return list;
		}
	}
}
