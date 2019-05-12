using System;
using System.Collections;
using UnityEngine;

public class ToleranceIcon : MonoBehaviour
{
	[SerializeField]
	private GameObject defaultIcon;

	[SerializeField]
	private GameObject strongIcon;

	[SerializeField]
	private GameObject weakIcon;

	[SerializeField]
	private GameObject drainIcon;

	[SerializeField]
	private GameObject disabled;

	private UIWidget uiWidget;

	private Coroutine animationCoroutine;

	private Strength[] lastIconTypes;

	private void Awake()
	{
		this.uiWidget = base.transform.GetComponent<UIWidget>();
	}

	public void SetActive(bool value)
	{
		if (base.gameObject.activeSelf != value)
		{
			NGUITools.SetActiveSelf(base.gameObject, value);
		}
		if (!value)
		{
			this.Stop();
		}
	}

	private void SetToleranceIcon(Strength iconType)
	{
		this.uiWidget.alpha = 1f;
		this.defaultIcon.SetActive(iconType == Strength.None);
		this.strongIcon.SetActive(iconType == Strength.Weak);
		this.weakIcon.SetActive(iconType == Strength.Strong);
		this.drainIcon.SetActive(iconType == Strength.Drain);
		this.disabled.SetActive(iconType == Strength.Invalid);
	}

	public void SetToleranceIcon(Strength[] iconTypes)
	{
		if (iconTypes == null)
		{
			return;
		}
		if (this.lastIconTypes != null && this.lastIconTypes.Length == iconTypes.Length)
		{
			bool flag = true;
			for (int i = 0; i < this.lastIconTypes.Length; i++)
			{
				if (this.lastIconTypes[i] != iconTypes[i])
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				return;
			}
		}
		this.Stop();
		this.animationCoroutine = base.StartCoroutine(this.Animation(iconTypes));
	}

	private IEnumerator Animation(Strength[] iconTypes)
	{
		this.lastIconTypes = iconTypes;
		int index = 0;
		if (iconTypes.Length == 1)
		{
			this.SetToleranceIcon(iconTypes[index]);
			yield break;
		}
		for (;;)
		{
			float time = 2f;
			this.SetToleranceIcon(iconTypes[index]);
			do
			{
				time -= Time.deltaTime;
				this.uiWidget.alpha = time / 2f;
				yield return null;
			}
			while (time > 0f);
			index++;
			if (index >= iconTypes.Length)
			{
				index = 0;
			}
		}
	}

	private void Stop()
	{
		if (this.animationCoroutine != null)
		{
			this.lastIconTypes = null;
			base.StopCoroutine(this.animationCoroutine);
		}
	}
}
