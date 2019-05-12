using System;
using UnityEngine;

public class CommonResourcesDataMng : MonoBehaviour
{
	private static CommonResourcesDataMng instance;

	public static CommonResourcesDataMng Instance()
	{
		return CommonResourcesDataMng.instance;
	}

	protected virtual void Awake()
	{
		CommonResourcesDataMng.instance = this;
	}

	protected virtual void OnDestroy()
	{
		CommonResourcesDataMng.instance = null;
	}

	public string GetUniqueSkillPrefabPathByAttackEffectId(string attack_effect)
	{
		return "DeathblowEffects/" + attack_effect + "/prefab";
	}

	public string GetCommonSkillPrefabPathByAttackEffectId(string attack_effect)
	{
		return "InheritanceTechniqueEffects/" + attack_effect + "/prefab";
	}

	public string GetStagePrefabPathByAttackEffectId(string stageId)
	{
		return "Stages/" + stageId + "/prefab";
	}

	public string GetCameraMotionPrefabPathByCameraId(string cameraId)
	{
		return "CameraMotions/" + cameraId + "/prefab";
	}

	public string GetHitEffectPrefabPathByHitEffectId(string hitEffectId)
	{
		return "HitEffects/" + hitEffectId + "/prefab";
	}

	public string GetAlwaysEffectPrefabPathByAlwaysEffectId(string alwaysEffectId)
	{
		return "AlwaysEffects/" + alwaysEffectId + "/prefab";
	}

	public string GetSpawnPointPrefabPathBySpawnPointId(string spawnPointId)
	{
		return "SpawnPoints/" + spawnPointId + "/prefab";
	}
}
