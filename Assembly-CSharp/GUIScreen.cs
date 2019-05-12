using System;
using UnityEngine;

public class GUIScreen : GUIBase
{
	[SerializeField]
	private bool haveIndicator_ = true;

	[SerializeField]
	private bool haveCommonUI_ = true;

	protected Transform stringBar;

	public bool haveIndicator
	{
		get
		{
			return this.haveIndicator_;
		}
	}

	public bool haveCommonUI
	{
		get
		{
			return this.haveCommonUI_;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.name.EndsWith("StringBar"))
			{
				this.stringBar = transform;
			}
		}
	}

	public override void ShowGUI()
	{
		if (this.haveIndicator_)
		{
			GUIManager.ShowGUI("FaceIndicator");
		}
		else
		{
			GUIManager.HideGUI("FaceIndicator");
		}
		if (this.haveCommonUI_)
		{
			GUIManager.ShowGUI("FaceUI");
		}
		else
		{
			GUIManager.HideGUI("FaceUI");
		}
	}

	public override void HideGUI()
	{
		if (this.haveIndicator_)
		{
			GUIManager.HideGUI("FaceIndicator");
		}
		if (this.haveCommonUI_)
		{
			GUIManager.HideGUI("FaceUI");
		}
	}
}
