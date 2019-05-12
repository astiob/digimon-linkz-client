using System;
using UnityEngine;

public class FriendshipUPFaceMark : MonoBehaviour
{
	private Camera farmCamera;

	public GameObject farmObject;

	private Camera mainCamera;

	public bool barrierOn;

	[SerializeField]
	private GameObject barrierObj;

	[SerializeField]
	private UISprite partsFace;

	private float scaleDifferenceY;

	private void Start()
	{
		base.Invoke("DestroyPref", 1.8f);
		FarmRoot instance = FarmRoot.Instance;
		this.farmCamera = instance.Camera;
		this.mainCamera = Singleton<GUIMain>.Instance.GetComponent<Camera>();
	}

	private void DestroyPref()
	{
		UnityEngine.Object.Destroy(base.gameObject);
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
			if (this.barrierOn)
			{
				this.barrierObj.SetActive(true);
				this.barrierOn = false;
			}
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
}
