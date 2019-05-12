using Monster;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterDetailsUI
{
	[Serializable]
	public sealed class CharacterStatusResistance
	{
		[SerializeField]
		private GameObject resistanceNone;

		[SerializeField]
		private GameObject resistanceFire;

		[SerializeField]
		private GameObject resistanceWater;

		[SerializeField]
		private GameObject resistanceThunder;

		[SerializeField]
		private GameObject resistanceNature;

		[SerializeField]
		private GameObject resistanceLight;

		[SerializeField]
		private GameObject resistanceDark;

		[SerializeField]
		private GameObject resistanceStun;

		[SerializeField]
		private GameObject resistanceSkillLock;

		[SerializeField]
		private GameObject resistanceSleep;

		[SerializeField]
		private GameObject resistanceParalysis;

		[SerializeField]
		private GameObject resistanceConfusion;

		[SerializeField]
		private GameObject resistancePoison;

		[SerializeField]
		private GameObject resistanceDeath;

		[SerializeField]
		private GameObject iconCoverEffectParentObject;

		private GameObject iconEffect;

		private Animation iconCoverAnimation;

		private GameObject GetEffectRootObject(GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM oldResistance, string newResistanceIds)
		{
			List<GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM> uniqueResistanceListByJson = MonsterResistanceData.GetUniqueResistanceListByJson(newResistanceIds);
			GameObject result = null;
			for (int i = 0; i < uniqueResistanceListByJson.Count; i++)
			{
				if ("1" == uniqueResistanceListByJson[i].none && "1" != oldResistance.none)
				{
					result = this.resistanceNone;
					break;
				}
				if ("1" == uniqueResistanceListByJson[i].fire && "1" != oldResistance.fire)
				{
					result = this.resistanceFire;
					break;
				}
				if ("1" == uniqueResistanceListByJson[i].water && "1" != oldResistance.water)
				{
					result = this.resistanceWater;
					break;
				}
				if ("1" == uniqueResistanceListByJson[i].thunder && "1" != oldResistance.thunder)
				{
					result = this.resistanceThunder;
					break;
				}
				if ("1" == uniqueResistanceListByJson[i].nature && "1" != oldResistance.nature)
				{
					result = this.resistanceNature;
					break;
				}
				if ("1" == uniqueResistanceListByJson[i].dark && "1" != oldResistance.dark)
				{
					result = this.resistanceDark;
					break;
				}
				if ("1" == uniqueResistanceListByJson[i].light && "1" != oldResistance.light)
				{
					result = this.resistanceLight;
					break;
				}
				if ("1" == uniqueResistanceListByJson[i].stun && "1" != oldResistance.stun)
				{
					result = this.resistanceStun;
					break;
				}
				if ("1" == uniqueResistanceListByJson[i].skillLock && "1" != oldResistance.skillLock)
				{
					result = this.resistanceSkillLock;
					break;
				}
				if ("1" == uniqueResistanceListByJson[i].sleep && "1" != oldResistance.sleep)
				{
					result = this.resistanceSleep;
					break;
				}
				if ("1" == uniqueResistanceListByJson[i].paralysis && "1" != oldResistance.paralysis)
				{
					result = this.resistanceParalysis;
					break;
				}
				if ("1" == uniqueResistanceListByJson[i].confusion && "1" != oldResistance.confusion)
				{
					result = this.resistanceConfusion;
					break;
				}
				if ("1" == uniqueResistanceListByJson[i].poison && "1" != oldResistance.poison)
				{
					result = this.resistancePoison;
					break;
				}
				if ("1" == uniqueResistanceListByJson[i].death && "1" != oldResistance.death)
				{
					result = this.resistanceDeath;
					break;
				}
			}
			return result;
		}

		private void OnFinalize()
		{
			this.iconCoverEffectParentObject.SetActive(false);
		}

		public int CreateResistanceIconEffect(GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM oldResistance, string newResistanceIds)
		{
			int num = 10;
			GameObject effectRootObject = this.GetEffectRootObject(oldResistance, newResistanceIds);
			GameObject original = Resources.Load<GameObject>("Cutscenes/NewFX11");
			this.iconEffect = UnityEngine.Object.Instantiate<GameObject>(original);
			Transform transform = this.iconEffect.transform;
			transform.SetParent(effectRootObject.transform);
			transform.localScale = Vector3.one;
			transform.localPosition = Vector3.zero;
			RenderFrontThanNGUI component = this.iconEffect.GetComponent<RenderFrontThanNGUI>();
			if (component != null)
			{
				num = component.GetSortOrder();
				UIPanel component2 = this.iconCoverEffectParentObject.GetComponent<UIPanel>();
				component2.sortingOrder = num + 1;
			}
			return num;
		}

		public void CreateResistanceCoverEffect()
		{
			Transform transform = this.iconCoverEffectParentObject.transform;
			GameObject original = Resources.Load<GameObject>("Cutscenes/NewFX10");
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
			Transform transform2 = gameObject.transform;
			transform2.parent = transform;
			transform2.localPosition = Vector3.zero;
			GameObject original2 = Resources.Load<GameObject>("UICommon/Parts/AwakeningParts");
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(original2);
			transform2 = gameObject2.transform;
			transform2.SetParent(transform);
			transform2.localPosition = Vector3.zero;
			transform2.localScale = Vector3.one;
			this.iconCoverAnimation = gameObject2.GetComponent<Animation>();
		}

		public void StartTranceEffect(Action finishAction)
		{
			this.iconCoverEffectParentObject.SetActive(true);
			SoundMng.Instance().TryPlaySE("SEInternal/Common/se_101", 0f, false, true, null, -1);
			this.iconCoverAnimation.Play("Awakening");
			EffectAnimatorEventTime component = this.iconCoverAnimation.GetComponent<EffectAnimatorEventTime>();
			component.SetEvent(0, finishAction);
			component.SetEvent(1, new Action(this.OnFinalize));
		}

		public void TranceEffectActiveSet(bool active)
		{
			if (null != this.iconEffect)
			{
				this.iconEffect.SetActive(active);
			}
		}
	}
}
