using System;
using UnityEngine;

namespace MonsterIcon
{
	public sealed class MonsterIcon
	{
		private Transform iconRootTransform;

		public MonsterThumbnail Thumbnail;

		public MonsterIconText Message;

		public MonsterIconNew New;

		public MonsterIconLock Lock;

		public MonsterIconArousal Arousal;

		public MonsterIconMedal Medal;

		public MonsterIconPlayerNo PlayerNo;

		public MonsterIconGimmick Gimmick;

		public void SetThumbnail(GameObject go)
		{
			this.Thumbnail = go.GetComponent<MonsterThumbnail>();
			global::Debug.Assert(null != this.Thumbnail, "Component Not Found : MonsterThumbnail プレハブを確認してください");
			this.iconRootTransform = this.Thumbnail.transform;
		}

		public void SetMessage(GameObject go)
		{
			this.Message = go.GetComponent<MonsterIconText>();
			global::Debug.Assert(null != this.Message, "Component Not Found : MonsterIconText プレハブを確認してください");
			MonsterIconTransform.AttachParts(this.iconRootTransform, go);
		}

		public void SetNewIcon(GameObject go)
		{
			this.New = go.GetComponent<MonsterIconNew>();
			global::Debug.Assert(null != this.New, "Component Not Found : MonsterIconNew プレハブを確認してください");
			MonsterIconTransform.AttachParts(this.iconRootTransform, go, this.New.GetPosition(), this.New.GetRotation());
		}

		public void SetLockIcon(GameObject go)
		{
			this.Lock = go.GetComponent<MonsterIconLock>();
			global::Debug.Assert(null != this.Lock, "Component Not Found : MonsterIconLock プレハブを確認してください");
			MonsterIconTransform.AttachParts(this.iconRootTransform, go, this.Lock.GetPosition());
		}

		public void SetArousal(GameObject go)
		{
			this.Arousal = go.GetComponent<MonsterIconArousal>();
			global::Debug.Assert(null != this.Arousal, "Component Not Found : MonsterIconArousal プレハブを確認してください");
			MonsterIconTransform.AttachParts(this.iconRootTransform, go, this.Arousal.GetPosition());
		}

		public void SetMedal(GameObject go)
		{
			this.Medal = go.GetComponent<MonsterIconMedal>();
			global::Debug.Assert(null != this.Medal, "Component Not Found : MonsterIconMedal プレハブを確認してください");
			MonsterIconTransform.AttachParts(this.iconRootTransform, go);
		}

		public void SetPlayerNo(GameObject go)
		{
			this.PlayerNo = go.GetComponent<MonsterIconPlayerNo>();
			global::Debug.Assert(null != this.PlayerNo, "Component Not Found : MonsterIconPlayerNo プレハブを確認してください");
			MonsterIconTransform.AttachParts(this.iconRootTransform, go, this.PlayerNo.GetPosition());
		}

		public void SetGimmickIcon(GameObject go)
		{
			this.Gimmick = go.GetComponent<MonsterIconGimmick>();
			global::Debug.Assert(null != this.Gimmick, "Component Not Found : MonsterIconGimmick プレハブを確認してください");
			MonsterIconTransform.AttachParts(this.iconRootTransform, go, this.Gimmick.GetPosition());
		}

		public void Copy(ref MonsterIcon dst)
		{
			dst.Thumbnail = UnityEngine.Object.Instantiate<MonsterThumbnail>(this.Thumbnail);
			dst.iconRootTransform = dst.Thumbnail.transform;
			GameObject gameObject = dst.Thumbnail.gameObject;
			if (null != this.Message)
			{
				dst.Message = gameObject.GetComponentInChildren<MonsterIconText>();
			}
			if (null != this.New)
			{
				dst.New = gameObject.GetComponentInChildren<MonsterIconNew>();
			}
			if (null != this.Lock)
			{
				dst.Lock = gameObject.GetComponentInChildren<MonsterIconLock>();
			}
			if (null != this.Arousal)
			{
				dst.Arousal = gameObject.GetComponentInChildren<MonsterIconArousal>();
			}
			if (null != this.Medal)
			{
				this.Medal = gameObject.GetComponentInChildren<MonsterIconMedal>();
			}
			if (null != this.PlayerNo)
			{
				dst.PlayerNo = gameObject.GetComponentInChildren<MonsterIconPlayerNo>();
			}
			if (null != this.Gimmick)
			{
				dst.Gimmick = gameObject.GetComponentInChildren<MonsterIconGimmick>();
			}
		}

		public GameObject GetRootObject()
		{
			return this.Thumbnail.gameObject;
		}

		public void Initialize(Transform parentObject, int iconSize, int depthOffset)
		{
			Transform transform = this.iconRootTransform;
			transform.parent = parentObject;
			transform.localScale = Vector3.one;
			transform.localPosition = Vector3.zero;
			MonsterIconTransform.SetSize(this.iconRootTransform, iconSize);
			DepthController.AddWidgetDepth_Static(transform, depthOffset);
			this.Thumbnail.Initialize();
		}
	}
}
