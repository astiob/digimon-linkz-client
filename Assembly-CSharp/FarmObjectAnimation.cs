using Facility;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public static class FarmObjectAnimation
{
	public static IEnumerator PlayAnimation(GameObject farmObject, FacilityAnimationID layer)
	{
		return FarmObjectAnimation.OneshotAnimation(farmObject, layer, 0f, 1f);
	}

	public static IEnumerator ReverseAnimation(GameObject farmObject, FacilityAnimationID layer)
	{
		return FarmObjectAnimation.OneshotAnimation(farmObject, layer, 1f, -1f);
	}

	public static IEnumerator PlayAnimation(this FarmObject farmObject, FacilityAnimationID layer)
	{
		return FarmObjectAnimation.OneshotAnimationForFarmObject(farmObject, layer, 0f, 1f);
	}

	public static IEnumerator ReverseAnimation(this FarmObject farmObject, FacilityAnimationID layer)
	{
		return FarmObjectAnimation.OneshotAnimationForFarmObject(farmObject, layer, 1f, -1f);
	}

	private static IEnumerator OneshotAnimationForFarmObject(FarmObject farmObject, FacilityAnimationID animationID, float startNormalizedTime, float animationSpeed)
	{
		return FarmObjectAnimation.OneshotAnimation(farmObject.gameObject, animationID, startNormalizedTime, animationSpeed);
	}

	public static IEnumerator OneshotAnimation(GameObject farmObject, FacilityAnimationID animationID, float startNormalizedTime, float animationSpeed)
	{
		int layer = 1;
		FarmFacilityAnimationData.FacilityAnimationInfo hash = FarmDataManager.FacilityAnimationData.animation.SingleOrDefault((FarmFacilityAnimationData.FacilityAnimationInfo x) => x.animeId == animationID);
		if (hash == null)
		{
			yield break;
		}
		Animator animation = farmObject.GetComponent<Animator>();
		if (null == animation || !animation.enabled)
		{
			yield break;
		}
		while (!farmObject.gameObject.activeSelf)
		{
			yield return null;
		}
		animation.speed = animationSpeed;
		animation.Play(hash.stateHash, layer, startNormalizedTime);
		yield return null;
		if (0f < animationSpeed)
		{
			while (1f > animation.GetCurrentAnimatorStateInfo(layer).normalizedTime)
			{
				yield return null;
			}
		}
		else
		{
			while (0f < animation.GetCurrentAnimatorStateInfo(layer).normalizedTime)
			{
				yield return null;
			}
		}
		yield break;
	}
}
