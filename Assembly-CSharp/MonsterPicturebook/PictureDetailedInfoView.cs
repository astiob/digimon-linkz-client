using System;
using UnityEngine;

namespace MonsterPicturebook
{
	[Serializable]
	public sealed class PictureDetailedInfoView
	{
		[SerializeField]
		private Transform viewParentObject;

		public UI_PictureDetailedInfo LoadUI(int skillCount)
		{
			string path = string.Empty;
			if (skillCount == 1)
			{
				path = "UI/Picturebook/OneSkillStatus";
			}
			else
			{
				path = "UI/Picturebook/TwoSkillStatus";
			}
			GameObject original = AssetDataMng.Instance().LoadObject(path, null, true) as GameObject;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
			Transform transform = gameObject.transform;
			transform.parent = this.viewParentObject;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			transform.name = "DetailedInfo";
			UIWidget component = this.viewParentObject.GetComponent<UIWidget>();
			DepthController.AddWidgetDepth_2(transform, component.depth);
			return gameObject.GetComponent<UI_PictureDetailedInfo>();
		}
	}
}
