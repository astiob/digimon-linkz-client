using System;
using UnityEngine;

namespace Cutscene.EffectParts
{
	public sealed class FxLaser : MonoBehaviour
	{
		private const float OFFSET_EULER_ANGLE_Y = 72f;

		[SerializeField]
		private GameObject particleObject;

		public static GameObject LoadPrefab()
		{
			return AssetDataMng.Instance().LoadObject("Cutscenes/FxLaser", null, true) as GameObject;
		}

		public static FxLaser Create(GameObject resource, Transform parentObject)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(resource);
			gameObject.transform.parent = parentObject;
			return gameObject.GetComponent<FxLaser>();
		}

		public static float GetRotationEulerAngleY(int materialMonsterNum, int effectIndex)
		{
			int num = 360 / materialMonsterNum;
			return (float)(num * effectIndex);
		}

		public void SetTransform(float positionY, float eulerAngleY)
		{
			Transform transform = base.transform;
			transform.localPosition = new Vector3(0f, positionY, 0f);
			transform.localRotation = Quaternion.Euler(new Vector3(0f, eulerAngleY + 72f, 0f));
		}

		public void StartMoveEffect()
		{
			Animator component = base.gameObject.GetComponent<Animator>();
			component.SetBool("move", true);
		}
	}
}
