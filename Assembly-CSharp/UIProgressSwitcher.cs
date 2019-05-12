using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(UIGaugeManager))]
public class UIProgressSwitcher : MonoBehaviour
{
	[SerializeField]
	private UIProgressSwitcher.SwitchingMode _switchingMode;

	[SerializeField]
	private GameObject[] _switchingObjects = new GameObject[0];

	private UIGaugeManager progress;

	public UIProgressSwitcher.SwitchingMode switchingMode
	{
		get
		{
			return this._switchingMode;
		}
		set
		{
			this._switchingMode = value;
		}
	}

	public GameObject[] switchingObjects
	{
		get
		{
			return this._switchingObjects;
		}
		set
		{
			this._switchingObjects = value;
		}
	}

	private void OnEnable()
	{
		if (this.progress == null)
		{
			this.progress = base.GetComponent<UIGaugeManager>();
		}
	}

	private void Update()
	{
		this.Apply();
	}

	public void Apply()
	{
		if (this.progress == null)
		{
			return;
		}
		if (this.switchingObjects.Length <= 0)
		{
			return;
		}
		UIProgressSwitcher.SwitchingMode switchingMode = this.switchingMode;
		int num = Mathf.FloorToInt(Mathf.Lerp(0f, (float)this.switchingObjects.Length, this.progress.GetValueToNormalize()));
		if (num >= this.switchingObjects.Length)
		{
			num = this.switchingObjects.Length - 1;
		}
		for (int i = 0; i < this.switchingObjects.Length; i++)
		{
			bool flag = num == i;
			if (this.switchingObjects[i].activeSelf != flag)
			{
				NGUITools.SetActiveSelf(this.switchingObjects[i], flag);
			}
		}
	}

	public enum SwitchingMode
	{
		Automatic
	}
}
