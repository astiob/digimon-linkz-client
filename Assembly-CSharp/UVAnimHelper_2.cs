using System;
using System.Collections.Generic;
using UnityEngine;

public class UVAnimHelper_2 : MonoBehaviour
{
	[SerializeField]
	[Header("UVアニメデータ・name + speedU + speedV のみ設定してください")]
	private List<UVAnimHelper_2.UVAnimData> UVAnimDataList;

	private MeshRenderer mr;

	protected virtual void Awake()
	{
		this.mr = base.gameObject.GetComponent<MeshRenderer>();
		if (this.mr != null && this.UVAnimDataList != null && this.UVAnimDataList.Count > 0)
		{
			for (int i = 0; i < this.UVAnimDataList.Count; i++)
			{
				int j;
				for (j = 0; j < this.mr.materials.Length; j++)
				{
					string b = this.UVAnimDataList[i].matName + " (Instance)";
					if (this.mr.materials[j].name == b)
					{
						break;
					}
				}
				if (j < this.mr.materials.Length)
				{
					this.UVAnimDataList[i].mat = this.mr.materials[j];
				}
				else
				{
					this.UVAnimDataList[i].mat = null;
				}
			}
		}
	}

	protected virtual void Update()
	{
		for (int i = 0; i < this.UVAnimDataList.Count; i++)
		{
			if (this.UVAnimDataList[i].mat != null)
			{
				this.UVAnimDataList[i].totalU += this.UVAnimDataList[i].speedU;
				this.UVAnimDataList[i].totalV += this.UVAnimDataList[i].speedV;
				int num = (int)this.UVAnimDataList[i].totalU;
				float value = this.UVAnimDataList[i].totalU - (float)num;
				int num2 = (int)this.UVAnimDataList[i].totalV;
				float value2 = this.UVAnimDataList[i].totalV - (float)num2;
				this.UVAnimDataList[i].mat.SetFloat("_UVAnimOffsetX", value);
				this.UVAnimDataList[i].mat.SetFloat("_UVAnimOffsetY", value2);
			}
		}
	}

	protected virtual void OnDestroy()
	{
	}

	[Serializable]
	public class UVAnimData
	{
		public string matName;

		public Material mat;

		public float speedU;

		public float speedV;

		public float totalU;

		public float totalV;
	}
}
