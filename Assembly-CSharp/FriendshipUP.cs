using System;
using System.Collections;
using UnityEngine;

public class FriendshipUP : MonoBehaviour
{
	private Camera farmCamera;

	[NonSerialized]
	public GameObject farmObject;

	private Camera mainCamera;

	[SerializeField]
	private UISprite statusPart;

	private void Start()
	{
		FarmRoot instance = FarmRoot.Instance;
		this.farmCamera = ((!(instance != null)) ? null : instance.Camera);
		this.mainCamera = Singleton<GUIMain>.Instance.GetComponent<Camera>();
		base.StartCoroutine(this.DestroyPref(2f));
	}

	public void ViewFriendshipStatus(int upFriendshipValue)
	{
		int num = upFriendshipValue;
		this.statusPart.spriteName = "Common02_Friendship";
		this.statusPart.MakePixelPerfect();
		int num2 = 1;
		foreach (char c in num.ToString())
		{
			GameObject gameObject = new GameObject();
			gameObject.transform.SetParent(this.statusPart.transform.parent);
			gameObject.transform.localScale = Vector3.one;
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.statusPart.gameObject);
			gameObject2.SetActive(true);
			gameObject2.transform.SetParent(gameObject.transform);
			UISprite component = gameObject2.GetComponent<UISprite>();
			component.spriteName = "Common02_FriendshipN_" + c;
			component.MakePixelPerfect();
			gameObject.transform.localPosition = new Vector3((float)this.statusPart.width - this.statusPart.transform.localPosition.x + (float)(component.width * num2), 0f, 0f);
			num2++;
		}
	}

	private void Update()
	{
		if (this.farmObject != null)
		{
			Vector3 position = this.farmCamera.WorldToScreenPoint(this.farmObject.transform.position);
			Vector3 vector = this.mainCamera.ScreenToWorldPoint(position);
			Vector3 position2 = base.transform.position;
			position2.x = vector.x;
			position2.y = vector.y;
			base.transform.position = position2;
			base.transform.localPosition = new Vector3(base.gameObject.transform.localPosition.x - 90f, base.gameObject.transform.localPosition.y + 100f, base.gameObject.transform.localPosition.z);
		}
	}

	private IEnumerator DestroyPref(float witTime)
	{
		yield return new WaitForSeconds(witTime);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}
}
