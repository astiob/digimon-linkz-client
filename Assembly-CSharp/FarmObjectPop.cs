using System;
using UnityEngine;

public class FarmObjectPop : MonoBehaviour
{
	[SerializeField]
	private GameObject arrow;

	public void SetActivePop(FarmObjectPop.PopType type, Transform cameraTransform, float adjustY)
	{
		Vector3 worldPosition = base.transform.position + cameraTransform.forward * 2f;
		if (type == FarmObjectPop.PopType.ARROW)
		{
			this.arrow.SetActive(true);
			Transform transform = this.arrow.transform;
			Vector3 localPosition = transform.localPosition;
			localPosition.y += adjustY;
			transform.localPosition = localPosition;
			transform.LookAt(worldPosition, cameraTransform.up);
		}
	}

	public void DestroyPop()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public enum PopType
	{
		ARROW
	}
}
