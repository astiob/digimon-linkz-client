using System;
using UnityEngine;

public class ChipThumbnailAdvent : MonoBehaviour
{
	[Header("チップアイコン")]
	[SerializeField]
	private ChipIcon chipIcon;

	[SerializeField]
	[Header("チップアニメーション")]
	private Animation chipAnimation;

	private bool isSetData;

	public void SetData(GameWebAPI.RespDataMA_ChipM.Chip data)
	{
		if (data == null)
		{
			base.gameObject.SetActive(false);
			UnityEngine.Object.Destroy(base.gameObject);
			this.isSetData = false;
			return;
		}
		this.chipIcon.SetData(data, -1, -1);
		this.isSetData = true;
	}

	private void Update()
	{
		if (!this.chipAnimation.isPlaying && this.isSetData)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			base.enabled = false;
		}
	}
}
