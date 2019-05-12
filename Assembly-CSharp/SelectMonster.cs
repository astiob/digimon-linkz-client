using System;
using UnityEngine;

public sealed class SelectMonster : MonoBehaviour
{
	private const string layerName = "UI3D";

	[SerializeField]
	private GameObject boxUIGO;

	[SerializeField]
	private GameObject standardStatusPlateEmphasis;

	[SerializeField]
	private GameObject multiStatusPlateEmphasis;

	private void OnEnable()
	{
		bool flag = BattleStateManager.current.battleMode == BattleMode.Multi;
		this.standardStatusPlateEmphasis.SetActive(!flag);
		this.multiStatusPlateEmphasis.SetActive(flag);
		int newLayer = LayerMask.NameToLayer("UI3D");
		this.SetLayerRecursively(this.boxUIGO, newLayer);
	}

	private void SetLayerRecursively(GameObject obj, int newLayer)
	{
		if (null == obj)
		{
			return;
		}
		obj.layer = newLayer;
		foreach (object obj2 in obj.transform)
		{
			Transform transform = (Transform)obj2;
			if (!(null == transform))
			{
				this.SetLayerRecursively(transform.gameObject, newLayer);
			}
		}
	}
}
