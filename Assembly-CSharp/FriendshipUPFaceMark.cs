using System;
using UnityEngine;

public class FriendshipUPFaceMark : MonoBehaviour
{
	private Camera farmCamera;

	public GameObject farmObject;

	private Camera mainCamera;

	[SerializeField]
	private GameObject barrierObj;

	[SerializeField]
	private UISprite partsFace;

	[SerializeField]
	private Animation iconAnimation;

	[SerializeField]
	private EffectAnimatorEventTime eventTime;

	private float scaleDifferenceY;

	private void Start()
	{
		FarmRoot instance = FarmRoot.Instance;
		this.farmCamera = instance.Camera;
		this.mainCamera = GUIMain.GetOrthoCamera();
	}

	private void Update()
	{
		if (this.farmObject != null)
		{
			Vector3 position = this.farmCamera.WorldToScreenPoint(this.farmObject.transform.position + new Vector3(0f, this.scaleDifferenceY));
			Vector3 vector = this.mainCamera.ScreenToWorldPoint(position);
			Vector3 position2 = base.transform.position;
			position2.x = vector.x;
			position2.y = vector.y;
			base.transform.position = position2;
			float y;
			if (this.farmCamera.orthographicSize <= 9.25f)
			{
				y = base.gameObject.transform.localPosition.y + 290f - this.farmCamera.orthographicSize * 20f + this.scaleDifferenceY * 2f;
			}
			else
			{
				y = base.gameObject.transform.localPosition.y + 100f + this.scaleDifferenceY * 2f;
			}
			base.transform.localPosition = new Vector3(base.gameObject.transform.localPosition.x - 90f, y, base.gameObject.transform.localPosition.z);
		}
	}

	public void ChangeIcon(bool upFriendship, float digimonSizeY)
	{
		if (upFriendship)
		{
			this.partsFace.spriteName = "Farm02_icon_Smile";
		}
		else
		{
			this.partsFace.spriteName = "Farm02_icon_Amazed";
		}
		this.scaleDifferenceY = digimonSizeY;
	}

	public void StartAnimation()
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(true);
		}
		if (this.iconAnimation.isPlaying)
		{
			this.iconAnimation.Stop();
		}
		this.iconAnimation.Play();
		this.eventTime.SetEvent(0, new Action(this.DestroyIcon));
	}

	public void SetBarrier()
	{
		this.barrierObj.SetActive(true);
	}

	public void DestroyIcon()
	{
		base.gameObject.SetActive(false);
		if (this.barrierObj.activeSelf)
		{
			this.barrierObj.SetActive(false);
		}
	}
}
