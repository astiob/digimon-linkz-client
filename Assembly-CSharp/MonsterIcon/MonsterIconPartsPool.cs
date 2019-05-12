using System;
using UnityEngine;

namespace MonsterIcon
{
	public sealed class MonsterIconPartsPool : MonoBehaviour
	{
		public GameObject Thumbnail { get; private set; }

		public GameObject Message { get; private set; }

		public GameObject NewIcon { get; private set; }

		public GameObject LockIcon { get; private set; }

		public GameObject Arousal { get; private set; }

		public GameObject Medal { get; private set; }

		public GameObject PlayerNo { get; private set; }

		public GameObject GimmickIcon { get; private set; }

		public void SetThumbnail(GameObject go)
		{
			go.transform.parent = base.transform;
			this.Thumbnail = go;
		}

		public void SetMessage(GameObject go)
		{
			go.transform.parent = base.transform;
			this.Message = go;
		}

		public void SetNewIcon(GameObject go)
		{
			go.transform.parent = base.transform;
			this.NewIcon = go;
		}

		public void SetLockIcon(GameObject go)
		{
			go.transform.parent = base.transform;
			this.LockIcon = go;
		}

		public void SetArousal(GameObject go)
		{
			go.transform.parent = base.transform;
			this.Arousal = go;
		}

		public void SetMedal(GameObject go)
		{
			go.transform.parent = base.transform;
			this.Medal = go;
		}

		public void SetPlayerNo(GameObject go)
		{
			go.transform.parent = base.transform;
			this.PlayerNo = go;
		}

		public void SetGimmickIcon(GameObject go)
		{
			go.transform.parent = base.transform;
			this.GimmickIcon = go;
		}
	}
}
