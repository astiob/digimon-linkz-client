using System;
using UnityEngine;

namespace CharacterDetailsUI
{
	[Serializable]
	public sealed class CharacterStatusReinforcement
	{
		[SerializeField]
		private GameObject gaugeCoverEffectParentObject;

		private Animation levelUpAnimtion;

		private void OnFinalize()
		{
			this.gaugeCoverEffectParentObject.SetActive(false);
		}

		public void CreateLevelUpAnimation(int sortingOrder)
		{
			UIPanel component = this.gaugeCoverEffectParentObject.GetComponent<UIPanel>();
			component.sortingOrder = sortingOrder;
			Transform transform = this.gaugeCoverEffectParentObject.transform;
			GameObject original = Resources.Load<GameObject>("Cutscenes/NewFX6");
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
			Transform transform2 = gameObject.transform;
			transform2.parent = transform;
			transform2.localPosition = Vector3.zero;
			original = Resources.Load<GameObject>("UICommon/Parts/LevelUp");
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(original);
			transform2 = gameObject2.transform;
			transform2.parent = transform;
			transform2.localPosition = Vector3.zero;
			transform2.localScale = Vector3.one;
			this.levelUpAnimtion = gameObject2.GetComponent<Animation>();
		}

		public void ShowLevelUpParticle(Action finishAction)
		{
			this.gaugeCoverEffectParentObject.SetActive(true);
			this.levelUpAnimtion.Play("LevelUp");
			EffectAnimatorEventTime component = this.levelUpAnimtion.GetComponent<EffectAnimatorEventTime>();
			component.SetEvent(0, finishAction);
			component.SetEvent(1, new Action(this.OnFinalize));
			SoundMng.Instance().TryPlaySE("SEInternal/Common/se_101", 0f, false, true, null, -1);
		}
	}
}
